using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeAjusteDeInventarioController : ControllerBase
    {
        private readonly BL.IAdministradorDeAjustesDeInventarios ElAdministradorDeAjustesDeInventario;

        public ServicioDeAjusteDeInventarioController(BL.IAdministradorDeAjustesDeInventarios elAdministrador)
        {
            ElAdministradorDeAjustesDeInventario = elAdministrador;
        }

        [HttpGet("Liste")]
        public ActionResult<List<Inventario>> ObtengaLaLista()
        {
            var lista = ElAdministradorDeAjustesDeInventario.ObtengaLaLista();
            return Ok(lista);
        }

        [HttpGet("ObtengaListaDeAjustes")]
        public ActionResult<List<Model.ModeloAjusteDeInventario>> ObtengaListaDeAjustes(int ajuste)
        {
            var lista = ElAdministradorDeAjustesDeInventario.ObtengaListaDeAjustes(ajuste);
            return Ok(lista);
        }

        [HttpGet("ObtengaListaDeAjustesParaDetalle")]
        public ActionResult<List<ModeloAjusteDeInventario>> ObtengaListaDeAjustesParaDetalle(int detalleAjuste)
        {
            var lista = ElAdministradorDeAjustesDeInventario.ObtengaListaDeAjustesParaDetalle(detalleAjuste);
            return Ok(lista);
        }

        [HttpGet("ObtengaInventarioPorId")]
        public ActionResult<Inventario> ObtengaInventarioPorId(int idInventario)
        {
            var inventario = ElAdministradorDeAjustesDeInventario.ObtenerInventarioPorId(idInventario);
            return Ok(inventario);
        }

        [HttpPost("AgregueAjuste")]
        public ActionResult<bool> AgregueAjuste(ModeloAgregarAjuste nuevoAjuste)
        {
            var resultado = ElAdministradorDeAjustesDeInventario.AgregueAjuste(nuevoAjuste);
            return Ok(resultado);
        }
    }
}
