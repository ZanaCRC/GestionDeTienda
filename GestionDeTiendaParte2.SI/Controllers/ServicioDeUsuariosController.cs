using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
        // GET: api/<ServicioDeUsuariosController>
        [HttpGet]
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<ServicioDeUsuariosController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<ServicioDeUsuariosController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<ServicioDeUsuariosController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<ServicioDeUsuariosController>/5
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
