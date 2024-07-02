using GestionDeTiendaParte1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public interface IAdministradorDeVentas
    {
        public void AgregueUnaNuevaVenta(Model.Venta nuevaVenta, Model.AperturaDeCaja cajaAbierta);
        public List<Venta> BusqueVentasPorIdAperturaCaja(int IdAperturaCaja);
        public List<Inventario> ObtenerTodosLosProductos();
        public Inventario BusqueProductoDelInventarioPorId(int idProducto);
        public void AgregueVentaDetalle(int idVentaEnCurso, Inventario productoSeleccionado);
        public List<Inventario> ObtenerInventariosPorVenta(int idVenta);

        public Venta BusqueVentasPorId(int Id);
        public void ActualiceMontosEnUnaVenta(int idVentaEnCurso);
        public void ElimineVentaDetalle(int idVenta, int idInventario);
        public void AgregueDescuento(int id, Model.Venta ventaConDescuento);

        public void TermineLaVenta(int id, Model.Venta ventaConDescuento);
        public List<VentaDetalle> ObtenerVentaDetallesPorVenta(int idVenta);
        public Model.VentaDetalle BusqueVentaDetallePorIdInventarioYVenta(int idInventario, int idVenta);


    }
}
