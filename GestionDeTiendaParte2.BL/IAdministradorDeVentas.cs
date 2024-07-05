using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public interface IAdministradorDeVentas
    {
        public void AgregueUnaNuevaVenta(Model.ModeloCrearVenta nuevaVenta, Model.AperturaDeCaja cajaAbierta);
        public List<Model.ModeloVenta> BusqueVentasPorIdAperturaCaja(int idAperturaCaja);
        public List<Inventario> ObtenerTodosLosProductos();
        public Inventario BusqueProductoDelInventarioPorId(int idProducto);
        public void AgregueVentaDetalle(int idVentaEnCurso, ModeloInventario productoSeleccionado);
        
        public List<ModeloParaMostrarInventarioDeUnaVenta> ObtenerInventariosPorVenta(int idVenta);
        public Venta BusqueVentasPorId(int Id);
        public void ActualiceMontosEnUnaVenta(int idVentaEnCurso);
        public void ElimineVentaDetalle(int idVenta, int idInventario);
        public void AgregueDescuento(int id, Model.Venta ventaConDescuento);

        public void TermineLaVenta(int id, Model.Venta ventaConDescuento);
        public List<VentaDetalle> ObtenerVentaDetallesPorVenta(int idVenta);
        public Model.ModeloVentaDetalle BusqueVentaDetallePorIdInventarioYVenta(int idInventario, int idVenta);
        public ModeloVenta ConvertirAVentaModelo(Venta venta);
        public List<ModeloVenta> ConvertirAVentasModelo(List<Venta> ventas);


    }
}
