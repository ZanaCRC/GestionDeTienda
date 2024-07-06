using GestionDeTiendaParte2.BL;
using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeLogin : ControllerBase
    {
        private readonly IAdministradorDeUsuarios administradorDeUsuarios;
        private readonly IAdministradorDeCorreos administradorDeCorreos;

        public ServicioDeLogin(IAdministradorDeUsuarios administradorDeUsuarios, IAdministradorDeCorreos administradorDeCorreos)
        {
            this.administradorDeUsuarios = administradorDeUsuarios;
            this.administradorDeCorreos = administradorDeCorreos;
        }

        [HttpPost("RegistrarUsuario")]
        public string RegistrarUsuario(string nombre, string correoElectronico, string clave)
        {
            return administradorDeUsuarios.RegistrarUsuario(nombre, correoElectronico, clave);
        }

        [HttpPost("EnviarCorreo")]
        public Task EnviarCorreo(Model.ModeloUsuario elUsuario)
        {
            string asunto = $"Inicio de sesión del usuario {elUsuario.Nombre}.";
            string cuerpo = $"Usted inicio sesión el día {DateTime.Now:dd/MM/yyyy} a las {DateTime.Now:HH:mm}.";
            return administradorDeCorreos.SendEmailAsync(elUsuario.CorreoElectronico, asunto, cuerpo);
        }

        [HttpGet("IniciarSesion")]
        public Usuario IniciarSesion(string nombre, string clave)
        {
            return administradorDeUsuarios.IniciarSesion(nombre, clave);
        }

        [HttpGet("ObtenerUsuarioPorNombre")]
        public Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            return administradorDeUsuarios.ObtenerUsuarioPorNombre(nombre);
        }

        [HttpPut("CambiarClave")]
        public bool CambiarClave(string nombre, string nuevaClave)
        {
            return administradorDeUsuarios.CambiarClave(nombre, nuevaClave);
        }

        [HttpPost("GuardarOActualizarUsuarioExterno")]
        public Usuario GuardarOActualizarUsuarioExterno(string nombre, string correoElectronico)
        {
            return administradorDeUsuarios.GuardarOActualizarUsuarioExterno(nombre, correoElectronico);
        }
    }
}
