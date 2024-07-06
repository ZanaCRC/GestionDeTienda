using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class ServicioDeUsuariosController : ControllerBase
    {


        private readonly BL.IAdministradorDeUsuarios elAdministradorDeUsuarios;

        public ServicioDeUsuariosController(BL.IAdministradorDeUsuarios AdministradorDeUsuarios)
        {
            elAdministradorDeUsuarios = AdministradorDeUsuarios;
            
        }
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }

        [HttpGet("ObtenerListaDeUsuarios")]
        public ActionResult<List<Usuario>> ObtenerListaDeUsuarios()
        {
            return elAdministradorDeUsuarios.ObtenerTodosLosUsuarios();
        }
        [HttpPost("DePermisos")]
        public IActionResult DePermisos(int id)
        {
            elAdministradorDeUsuarios.DePermisos(id);
            return Ok();
        }


    }
}
