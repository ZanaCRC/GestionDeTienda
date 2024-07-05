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
        public ActionResult<List<Venta>> ObtenerVentas(int userID)
        {
            
                var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(userID);
                if (cajaAbierta == null)
                {
                    return NotFound("No hay caja activa para el usuario proporcionado.");
                }

                var listaDeVentas = elAdministradorDeVentas.BusqueVentasPorIdAperturaCaja(cajaAbierta.Id);
                return Ok(listaDeVentas);
            
           
        }


        [HttpPost("AgregarProductos/{idVenta}")]
        public IActionResult AgregarProductosALaVenta(int idVenta, [FromBody] List<Inventario> productosSeleccionados)
        {
            foreach (var producto in productosSeleccionados)
            {
                if (producto.IsSelected)
                {
                    elAdministradorDeVentas.AgregueVentaDetalle(idVenta, producto);
                }
            }

            elAdministradorDeVentas.ActualiceMontosEnUnaVenta(idVenta);
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

        [HttpGet("ListaProductos/{idVenta}")]
        public ActionResult<List<Inventario>> ObtenerListaProductosDeVenta(int idVenta)
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

        [HttpGet("ObtenerVenta/{idVenta}")]
        public ActionResult<Venta> ObtenerVenta(int idVenta)
        {
            var venta = elAdministradorDeVentas.BusqueVentasPorId(idVenta);
            if (venta == null)
            {
                return NotFound();
            }

            return venta;
        }

        [HttpPost("AgregarDescuento/{idVenta}")]
        public IActionResult AgregarDescuentoAVenta(int idVenta, [FromBody] Venta ventaModificada)
        {
            elAdministradorDeVentas.AgregueDescuento(idVenta, ventaModificada);
            return Ok();
        }

        [HttpPost("TerminarVenta/{idVenta}")]
        public IActionResult TerminarVenta(int idVenta, [FromBody] Venta ventaModificada)
        {
            elAdministradorDeVentas.TermineLaVenta(idVenta, ventaModificada);
            return Ok();
        }
    }
}
