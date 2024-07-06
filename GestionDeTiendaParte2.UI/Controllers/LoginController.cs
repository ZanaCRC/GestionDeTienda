﻿using Microsoft.AspNetCore.Mvc;
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
using static System.Runtime.InteropServices.JavaScript.JSType;

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
                var queryParams = new Dictionary<string, string>
        {
            { "nombre", usuario.Nombre },
            { "clave", usuario.Clave }
        };
                var queryString = QueryHelpers.AddQueryString("", queryParams);

                var uri = $"https://apicomercio.azurewebsites.net/api/ServicioDeLogin/IniciarSesion{queryString}";

                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                Usuario elUsuario = JsonConvert.DeserializeObject<Usuario>(apiResponse);
                if (elUsuario == null)
                {
                    ViewData["Error"] = "Usuario o contraseña incorrecto"; return View();
                }
                else if (elUsuario.Rol == Model.Rol.Restringido) {
                    ViewData["ErrorRestringido"] = "Usuario sin permisos"; return View(); 
                }
                if (response.IsSuccessStatusCode && elUsuario != null && !elUsuario.EsExterno)
                {
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
                        AllowRefresh = true
                    };

                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), prop);

                    await EnviarCorreo(elUsuario.CorreoElectronico, elUsuario.Nombre, $"Inicio de sesión del usuario {elUsuario.Nombre}.", $"Usted inició sesión el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}.");

                    return RedirectToAction("Index", "Home");
                }
                else if (elUsuario != null && elUsuario.EsExterno)
                {
                    ViewData["Error"] = "El usuario ya está logueado con Google o Facebook";
                    return View();
                }
                else if(elUsuario != null && elUsuario.EstaBloqueado)
                {
                    await EnviarCorreo(elUsuario.CorreoElectronico, elUsuario.Nombre, $"Usuario Bloqueado. Intento de inicio de sesión del usuario {elUsuario.Nombre} bloqueado.", $"Le informamos que la cuenta del usuario {elUsuario.Nombre} se encuentra bloqueada por 10 minutos. Por favor ingrese el día {elUsuario.FechaBloqueo.Value.AddMinutes(10):dd/MM/yyyy} a las {elUsuario.FechaBloqueo.Value.AddMinutes(10):HH:mm}.");
                      
                   
                    ViewData["Error"] = "El usuario esta bloqueado";
                    return View();
                }
            }

            return View();
        }


        private async Task EnviarCorreo(string correoElectronico, string nombre, string asunto, string cuerpo)
        {
            var queryParams = new Dictionary<string, string>
    {
        { "asunto", asunto },
        { "cuerpo", cuerpo },
        { "correoElectronico", correoElectronico }
    };

            var queryString = QueryHelpers.AddQueryString("", queryParams);

            var uri = $"https://apicomercio.azurewebsites.net/api/ServicioDeLogin/EnviarCorreo{queryString}";

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

            string elCorreo = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            if (resultado)
            {
                 
                string asunto = $"Cambio de clave";
                string cuerpo = $"Le informamos que el cambio de clave de la cuenta del usuario {model.ElNombre} se ejecutó satisfactoriamente";
                ElMensajero.SendEmailAsync(elCorreo, asunto, cuerpo);
                return RedirectToAction("Index", "Home");
                }
                else
                {
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

                var usuario = ElAdministrador.GuardarOActualizarUsuarioExterno(nombre, correo);

                var newClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("UserId", usuario.Id.ToString()),
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

                var usuario = ElAdministrador.GuardarOActualizarUsuarioExterno(nombre, correo);
                var newClaims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, usuario.Nombre),
            new Claim("UserId", usuario.Id.ToString()),
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
