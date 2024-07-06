using GestionDeTiendaParte2.BL;
using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

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
        public void EnviarCorreo(string asunto, string cuerpo, string correoElectronico)
        {
            administradorDeCorreos.SendEmailAsync(correoElectronico, asunto, cuerpo);
                   }

        [HttpGet("IniciarSesion")]
        public Model.Usuario IniciarSesion(string nombre, string clave)
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
