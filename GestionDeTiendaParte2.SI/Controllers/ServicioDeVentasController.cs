using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeVentasController : ControllerBase
    {

        private readonly BL.IAdministradorDeCajas elAdministradorDeCajas;
        private readonly BL.IAdministradorDeVentas elAdministradorDeVentas;

        public ServicioDeVentasController(BL.IAdministradorDeCajas administradorDeCajas, BL.IAdministradorDeVentas administradorDeVentas)
        {
            elAdministradorDeCajas = administradorDeCajas;
            elAdministradorDeVentas = administradorDeVentas;
        }

        [HttpGet("ObtengaVentas")]
        public ActionResult<List<ModeloVenta>> ObtengaVentas(int userID)
        {
            
                var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(userID);
                if (cajaAbierta == null)
                {
                    return NotFound("No hay caja activa para el usuario proporcionado.");
                }

                var listaDeVentas = elAdministradorDeVentas.BusqueVentasPorIdAperturaCaja(cajaAbierta.Id);
                return Ok(listaDeVentas);
            
           
        }

        [HttpGet("ObtengaTodoElInventario")]
        public ActionResult<List<ModeloInventario>> ObtengaTodoElInventario()
        {


            var listaDeInventario = elAdministradorDeVentas.ObtenerTodosLosProductos();
            return Ok(listaDeInventario);


        }


        [HttpPost("AgregueProductos")]
        public IActionResult AgregueProductosALaVenta([FromBody] ModeloAgregarInventarioALaVenta productosSeleccionados)
        {
            foreach (var producto in productosSeleccionados.productosSeleccionados)
            {
                if (producto.IsSelected)
                {
                    elAdministradorDeVentas.AgregueVentaDetalle(productosSeleccionados.idVenta, producto);
                }
            }

            elAdministradorDeVentas.ActualiceMontosEnUnaVenta(productosSeleccionados.idVenta);
            return Ok();
        }


        [HttpPost("ElimineProductos/{idVenta}")]
        public IActionResult ElimineProductosDeVenta(int idVenta, [FromBody] List<int> productosSeleccionados)
        {
            foreach (var productoId in productosSeleccionados)
            {
                elAdministradorDeVentas.ElimineVentaDetalle(idVenta, productoId);
            }

            return Ok();
        }

        [HttpGet("ObtengaListaProductosDeVenta")]
        public ActionResult<List<ModeloParaMostrarInventarioDeUnaVenta>> ObtengaListaProductosDeVenta(int idVenta)
        {
            var listaDeProductosDelInventario = elAdministradorDeVentas.ObtenerInventariosPorVenta(idVenta);
            return listaDeProductosDelInventario;
        }

        [HttpPost("Cree")]
        public IActionResult CreeVenta([FromBody] Model.ModeloCrearVenta nuevaVenta)
        {
            var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(nuevaVenta.UserID);
            if (cajaAbierta == null)
            {
                return NotFound("No hay caja activa para el usuario proporcionado.");
            }

            elAdministradorDeVentas.AgregueUnaNuevaVenta(nuevaVenta, cajaAbierta);
            return Ok();
        }

        [HttpGet("ObtengaVenta")]
        public ActionResult<Venta> ObtengaVenta(int idVenta)
        {
            var venta = elAdministradorDeVentas.BusqueVentasPorId(idVenta);
            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost("AgregueDescuento")]
        public IActionResult AgregueDescuentoAVenta( [FromBody] ModeloAgregarDescuento ventaModificada)
        {
            Venta ventaConDescuento = new Venta();
            ventaConDescuento.PorcentajeDesCuento = ventaModificada.Descuento;
            ventaConDescuento.Id = ventaModificada.IdVenta;
            elAdministradorDeVentas.AgregueDescuento(ventaModificada.IdVenta, ventaConDescuento);
            return Ok();
        }

        [HttpPost("TermineVenta")]
        public IActionResult TermineVenta(ModeloParaFinalizarVenta ventaParaFinalizar)
        {
            Model.Venta ventaAEnviarParaFinalizar = new Model.Venta();
            ventaAEnviarParaFinalizar.MetodoDePago = ventaParaFinalizar.MetodoDePago;
            elAdministradorDeVentas.TermineLaVenta(ventaParaFinalizar.IdVenta, ventaAEnviarParaFinalizar);
            return Ok();
        }
    }
}
