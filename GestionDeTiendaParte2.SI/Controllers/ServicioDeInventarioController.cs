using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeInventarioController : ControllerBase
    {
        private readonly BL.IAdministradorDeInventarios elAdministrador;

        public ServicioDeInventarioController(BL.IAdministradorDeInventarios elAdministrador)
        {
            this.elAdministrador = elAdministrador;
        }

        [HttpGet("ObtenerLista")]
        public ActionResult<List<Inventario>> ObtenerLista()
        {
            var lista = elAdministrador.ObtengaLaLista();
           
                return lista;
        }

        [HttpGet("FiltreLaLista")]
        public List<Model.Inventario> FiltreLaLista(string nombre)
        {
            return elAdministrador.FiltreLaLista(nombre);
        }

        [HttpGet("Historico")]
        public ActionResult<List<Historico>> ObtenerHistorico(int idInventario)
        {
            var historico = elAdministrador.ObtengaHistorico(idInventario);
            return historico;
        }

        [HttpGet("Detalles/{id}")]
        public ActionResult<Inventario> ObtenerDetalles(int id)
        {
            var inventario = elAdministrador.ObtengaElInventario(id);
            if (inventario == null)
            {
                return NotFound();
            }
            return inventario;
        }

        [HttpPost("Agregar")]
        public IActionResult AgregarInventario([FromBody] ModeloInventario inventario, string userName)
        {
            elAdministrador.Agregue(inventario, userName);
            return Ok();
        }

        [HttpPut("EditarInventario")]
        public IActionResult EditarInventario([FromBody] ModeloInventario inventario)
        {
            elAdministrador.Edite(inventario);
            return Ok();
        }
    }
}