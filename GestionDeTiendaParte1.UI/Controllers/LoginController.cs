using Microsoft.AspNetCore.Mvc;
using GestionDeTiendaParte1.DA;
using GestionDeTiendaParte1.Model;
using Microsoft.EntityFrameworkCore;
using GestionDeTiendaParte1.BL;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;
using GestionDeTiendaParte1.UI.Models;

namespace GestionDeTiendaParte1.UI.Controllers
{
    public class LoginController : Controller
    {
        private BL.IAdministradorDeUsuarios ElAdministrador;
        private readonly IAdministradorDeCorreos ElMensajero;

        public LoginController(BL.IAdministradorDeUsuarios elAdministrador, BL.IAdministradorDeCorreos elMensajero)
        {
            ElAdministrador = elAdministrador;
            ElMensajero = elMensajero;  
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
                    ElMensajero.SendEmailAsync(usuario.CorreoElectronico, "Registro exitoso", "Bienvenido a la tienda");
                    return RedirectToAction("Index", "Home");
                }

                // Si llegamos aquí, es porque hubo un fallo en el registro
                return View(usuario);
            }
            return View(usuario);
        }


        [HttpGet]
        public IActionResult Loguearse()
        {
            return View();
        }

        [HttpPost]
        public async Task<ActionResult> Loguearse(UI.Models.UsuarioLoginViewModel usuario)
        {
            if (ModelState.IsValid)
            {
                Usuario elUsuario = ElAdministrador.IniciarSesion(usuario.Nombre, usuario.Clave);
                if (elUsuario != null && elUsuario.EsExterno == false)
                {
                    // Usuario autenticado correctamente
                    List<Claim> claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name, elUsuario.Nombre),
                        new Claim(ClaimTypes.Email, elUsuario.CorreoElectronico),
                        new Claim(ClaimTypes.Role, elUsuario.Rol.ToString()),
                        new Claim("UserId", elUsuario.Id.ToString())

                };
                    ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                    AuthenticationProperties prop = new AuthenticationProperties
                    {
                        IsPersistent = true,
                       // ExpiresUtc = DateTime.UtcNow.AddMinutes(20)
                       AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), prop);

                    // Enviar correo de inicio de sesión exitoso
                    string asunto = $"Inicio de sesión del usuario {elUsuario.Nombre}.";
                    string cuerpo = $"Usted inicio sesión el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}.";
                    ElMensajero.SendEmailAsync(elUsuario.CorreoElectronico, asunto, cuerpo);

                    return RedirectToAction("Index", "Home");
                }
                if (elUsuario != null && elUsuario.EsExterno )
                {
                    ViewData["Error"] = "El usuario ya está logueado con Google o Facebook";
                    return View();
                }
                else
                {
                    // Verificar si el usuario está bloqueado
                    Usuario usuarioBloqueado = ElAdministrador.ObtenerUsuarioPorNombre(usuario.Nombre);
                    if (usuarioBloqueado != null && usuarioBloqueado.EstaBloqueado)
                    {
                        // Enviar correo de usuario bloqueado o intento de inicio de sesión mientras está bloqueado
                        string asunto = usuarioBloqueado.IntentosFallidos >= 3 ? "Usuario Bloqueado." : $"Intento de inicio de sesión del usuario {usuarioBloqueado.Nombre} bloqueado.";
                        string cuerpo = $"Le informamos que la cuenta del usuario {usuarioBloqueado.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {usuarioBloqueado.FechaBloqueo.Value.AddMinutes(10):dd/MM/yyyy} a las {usuarioBloqueado.FechaBloqueo.Value.AddMinutes(10):HH:mm}.";
                        ElMensajero.SendEmailAsync(usuarioBloqueado.CorreoElectronico, asunto, cuerpo);
                    }
                    ViewData["Error"] = "Usuario o contraseña incorrectos";
                    return View();
                }
            }
            return View();
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

                if (resultado)
                {
                    // Cambio de contraseña exitoso
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
