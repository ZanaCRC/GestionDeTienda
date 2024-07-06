using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ServicioDeUsuariosController : ControllerBase
    {


        private readonly BL.IAdministradorDeUsuarios ElAdministradorDeUsuarios;

        public ServicioDeUsuariosController(BL.IAdministradorDeUsuarios administradorDeUsuarios)
        {
            ElAdministradorDeUsuarios = administradorDeUsuarios;
            
        }

        [HttpGet("ObtenerListaDeUsuarios")]
        public ActionResult<List<Usuario>> ObtenerListaDeUsuarios()
        {
            return ElAdministradorDeUsuarios.ObtenerTodosLosUsuarios();
        }
        [HttpPost("DePermisos")]
        public IActionResult DePermisos(int id)
        {
            ElAdministradorDeUsuarios.DePermisos(id);
            return Ok();
        }


    }
}
