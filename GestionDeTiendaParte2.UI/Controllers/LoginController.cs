using Microsoft.AspNetCore.Mvc;
using GestionDeTiendaParte2.DA;
using GestionDeTiendaParte2.Model;
using Microsoft.EntityFrameworkCore;
using GestionDeTiendaParte2.BL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using GestionDeTiendaParte2.UI.Models;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using Microsoft.AspNetCore.WebUtilities;

namespace GestionDeTiendaParte2.UI.Controllers
{
    public class LoginController : Controller
    {
        private BL.IAdministradorDeUsuarios ElAdministrador;
        private readonly IAdministradorDeCorreos ElMensajero;
        private readonly DA.DBContexto ElContexto;
        private readonly HttpClient httpClient;

        public LoginController(BL.IAdministradorDeUsuarios elAdministrador, BL.IAdministradorDeCorreos elMensajero, DBContexto elcontexto)
        {
            ElAdministrador = elAdministrador;
            ElMensajero = elMensajero;  
            ElContexto = elcontexto;
            httpClient = new HttpClient();  
        }

        [HttpGet]
        public IActionResult Registrarse()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registrarse(UI.Models.UsuarioViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                string resultado = ElAdministrador.RegistrarUsuario(usuario.Nombre, usuario.CorreoElectronico, usuario.Clave);
                if (resultado == "NombreExistente")
                {
                    ModelState.AddModelError("", "El nombre de usuario ya existe. Por favor, elige otro.");
                }
                else if (resultado == "CorreoExistente")
                {
                    ModelState.AddModelError("", "El correo electrónico ya está registrado. Por favor, utiliza otro.");
                }
                else if (resultado == "Exito")
                {
                    ElMensajero.SendEmailAsync(usuario.CorreoElectronico, "Solicitud de creación de usuario", "Cuenta de usuario creada satisfactoriamente para el usuario " + usuario.Nombre);
                    return RedirectToAction("Index", "Home");
                }

                // Si llegamos aquí, es porque hubo un fallo en el registro
                return View(usuario);
            }
            return View(usuario);
        }
        [HttpGet]
        public IActionResult Loguearse() {

            return View();
        }

      
        [HttpPost]
        public async Task<ActionResult> Loguearse(UsuarioLoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                // Construir los parámetros de consulta usando QueryHelpers
                var queryParams = new Dictionary<string, string>
        {
            { "nombre", usuario.Nombre },
            { "clave", usuario.Clave }
        };

                // Construir la cadena de consulta
                var queryString = QueryHelpers.AddQueryString("", queryParams);

                // URI para llamar al método de inicio de sesión en la API
                var uri = $"https://localhost:7001/api/ServicioDeLogin/IniciarSesion{queryString}";

                // Enviar la solicitud GET a la API
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Usuario elUsuario = JsonConvert.DeserializeObject<Usuario>(apiResponse);

                if (response.IsSuccessStatusCode && elUsuario != null && !elUsuario.EsExterno)
                {
                    // Crear claims para la autenticación
                    List<Claim> claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, elUsuario.Nombre),
                new Claim(ClaimTypes.Email, elUsuario.CorreoElectronico),
                new Claim(ClaimTypes.Role, elUsuario.Rol.ToString()),
                new Claim("UserId", elUsuario.Id.ToString())
            };

                    // Crear identidad de claims
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties prop = new AuthenticationProperties
                    {
                        IsPersistent = true,
                        AllowRefresh = true
                    };

                    // Autenticar al usuario
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), prop);

                    // Enviar correo de inicio de sesión
                    await EnviarCorreoInicioSesion(elUsuario.CorreoElectronico, elUsuario.Nombre);

                    // Redirigir al usuario a la página de inicio
                    return RedirectToAction("Index", "Home");
                }
                else if (elUsuario != null && elUsuario.EsExterno)
                {
                    // Manejar caso de usuario externo (Google o Facebook)
                    ViewData["Error"] = "El usuario ya está logueado con Google o Facebook";
                    return View();
                }
                else
                {
                    // Manejar caso de credenciales incorrectas
                    ViewData["Error"] = "Usuario o contraseña incorrectos";
                    return View();
                }
            }

            // Si el modelo no es válido, volver a mostrar la vista de login
            return View();
        }


        private async Task EnviarCorreoInicioSesion(string correoElectronico, string nombre)
        {
            // Construir los parámetros de consulta usando QueryHelpers
            var queryParams = new Dictionary<string, string>
    {
        { "asunto", $"Inicio de sesión del usuario {nombre}." },
        { "cuerpo", $"Usted inició sesión el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}." },
        { "correoElectronico", correoElectronico }
    };

            // Construir la cadena de consulta
            var queryString = QueryHelpers.AddQueryString("", queryParams);

            // URI para llamar al método de enviar correo en la API
            var uri = $"https://localhost:7001/api/ServicioDeLogin/EnviarCorreo{queryString}";

            // Enviar la solicitud POST a la API
            var response = await httpClient.PostAsync(uri, null);
            response.EnsureSuccessStatusCode();
        }

            public async Task LoguearseConGoogle()
        {
            await HttpContext.ChallengeAsync(GoogleDefaults.AuthenticationScheme, new
                AuthenticationProperties
            {
                RedirectUri = Url.Action("GoogleResponse")
            });
        }

        public async Task LoguearseConFacebook()
        {
            await HttpContext.ChallengeAsync(FacebookDefaults.AuthenticationScheme, new
                AuthenticationProperties
            {
                RedirectUri = Url.Action("FacebookResponse")
            });
        }

        [HttpGet]
        public IActionResult CambioDeClave()
        {
            return View();
        }

        [HttpPost]
        public IActionResult CambioDeClave(CambioDeClaveViewModel model)
        {
            
                var resultado = ElAdministrador.CambiarClave(model.ElNombre, model.NuevaClave);

            string elCorreo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value; // Ehte eh //

            if (resultado)
            {
                 
                string asunto = $"Cambio de clave";
                string cuerpo = $"Le informamos que el cambio de clave de la cuenta del usuario {model.ElNombre} se ejecutó satisfactoriamente";
                ElMensajero.SendEmailAsync(elCorreo, asunto, cuerpo);
                return RedirectToAction("Index", "Home");
                }
                else
                {
                    // Manejar el caso de error (usuario no encontrado o es externo)
                    ViewData["Error"] = "Algo ha salido mal.";
                    return View();
            }
            

            
        }

        public async Task<IActionResult> GoogleResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.ToList();
            if (claims != null)
            {
                string nombre = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string correo = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Guardar o actualizar el usuario en la base de datos y obtener el usuario
                var usuario = ElAdministrador.GuardarOActualizarUsuarioExterno(nombre, correo);

                // Crear una nueva lista de claims incluyendo el ID del usuario de la base de datos
                var newClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("UserId", usuario.Id.ToString()),
            // Agrega aquí más claims según sea necesario
        };

                var claimsIdentity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Loguearse", "Login");
        }

        public async Task<IActionResult> FacebookResponse()
        {
            var result = await HttpContext.AuthenticateAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            var claims = result.Principal.Identities.FirstOrDefault()?.Claims.ToList();
            if (claims != null)
            {
                string nombre = claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                string correo = claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

                // Guardar o actualizar el usuario en la base de datos y obtener el usuario
                var usuario = ElAdministrador.GuardarOActualizarUsuarioExterno(nombre, correo);

                // Crear una nueva lista de claims incluyendo el ID del usuario de la base de datos
                var newClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("UserId", usuario.Id.ToString()),
            // Agrega aquí más claims según sea necesario
        };

                var claimsIdentity = new ClaimsIdentity(newClaims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = true
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);

                return RedirectToAction("Index", "Home");
            }

            return RedirectToAction("Loguearse", "Login");
        }

    }
}
