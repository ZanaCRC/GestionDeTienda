using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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

        [HttpGet("ObtenerVentas")]
        public ActionResult<List<ModeloVenta>> ObtenerVentas(int userID)
        {
            
                var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(userID);
                if (cajaAbierta == null)
                {
                    return NotFound("No hay caja activa para el usuario proporcionado.");
                }

                var listaDeVentas = elAdministradorDeVentas.BusqueVentasPorIdAperturaCaja(cajaAbierta.Id);
                return Ok(listaDeVentas);
            
           
        }

        [HttpGet("ObtenerTodoElInventario")]
        public ActionResult<List<ModeloInventario>> ObtenerTodoElInventario()
        {


            var listaDeInventario = elAdministradorDeVentas.ObtenerTodosLosProductos();
            return Ok(listaDeInventario);


        }


        [HttpPost("AgregarProductos")]
        public IActionResult AgregarProductosALaVenta([FromBody] ModeloAgregarInventarioALaVenta productosSeleccionados)
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


        [HttpPost("EliminarProductos/{idVenta}")]
        public IActionResult EliminarProductosDeVenta(int idVenta, [FromBody] List<int> productosSeleccionados)
        {
            foreach (var productoId in productosSeleccionados)
            {
                elAdministradorDeVentas.ElimineVentaDetalle(idVenta, productoId);
            }

            return Ok();
        }

        [HttpGet("ObtenerListaProductosDeVenta")]
        public ActionResult<List<ModeloParaMostrarInventarioDeUnaVenta>> ObtenerListaProductosDeVenta(int idVenta)
        {
            var listaDeProductosDelInventario = elAdministradorDeVentas.ObtenerInventariosPorVenta(idVenta);
            return listaDeProductosDelInventario;
        }

        [HttpPost("Crear")]
        public IActionResult CrearVenta([FromBody] Model.ModeloCrearVenta nuevaVenta)
        {
            var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(nuevaVenta.UserID);
            if (cajaAbierta == null)
            {
                return NotFound("No hay caja activa para el usuario proporcionado.");
            }

            elAdministradorDeVentas.AgregueUnaNuevaVenta(nuevaVenta, cajaAbierta);
            return Ok();
        }

        [HttpGet("ObtenerVenta")]
        public ActionResult<Venta> ObtenerVenta(int idVenta)
        {
            var venta = elAdministradorDeVentas.BusqueVentasPorId(idVenta);
            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost("AgregarDescuento")]
        public IActionResult AgregarDescuentoAVenta( [FromBody] ModeloAgregarDescuento ventaModificada)
        {
            Venta ventaConDescuento = new Venta();
            ventaConDescuento.PorcentajeDesCuento = ventaModificada.Descuento;
            ventaConDescuento.Id = ventaModificada.IdVenta;
            elAdministradorDeVentas.AgregueDescuento(ventaModificada.IdVenta, ventaConDescuento);
            return Ok();
        }

        [HttpPost("TerminarVenta")]
        public IActionResult TerminarVenta(ModeloParaFinalizarVenta ventaParaFinalizar)
        {
            Model.Venta ventaAEnviarParaFinalizar = new Model.Venta();
            ventaAEnviarParaFinalizar.MetodoDePago = ventaParaFinalizar.MetodoDePago;
            elAdministradorDeVentas.TermineLaVenta(ventaParaFinalizar.IdVenta, ventaAEnviarParaFinalizar);
            return Ok();
        }
    }
}
