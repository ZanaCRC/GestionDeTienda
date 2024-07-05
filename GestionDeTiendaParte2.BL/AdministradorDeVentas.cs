using GestionDeTiendaParte2.Model;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public class AdministradorDeVentas : IAdministradorDeVentas
    {
        private readonly DA.DBContexto _dbContext;

        public AdministradorDeVentas(DA.DBContexto dbContext)
        {
            _dbContext = dbContext;
        }

        public void AgregueUnaNuevaVenta(Model.ModeloCrearVenta nuevaVentaNombre, Model.AperturaDeCaja cajaAbierta)
        {
            try
            {
                Model.Venta nuevaVenta = new Venta();
                nuevaVenta.Fecha = DateTime.Now;
                nuevaVenta.IdAperturaCaja = cajaAbierta.Id;
                nuevaVenta.Estado = Model.EstadoVenta.Proceso;
                nuevaVenta.NombreCliente = nuevaVentaNombre.NombreCliente;

                _dbContext.Ventas.Add(nuevaVenta);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al agregar una nueva venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public List<Model.Venta> BusqueVentasPorIdAperturaCaja(int idAperturaCaja)
        {
            try
            {
                var ventas = _dbContext.Ventas
                                      .Where(v => v.IdAperturaCaja == idAperturaCaja)
                                      .ToList();

                foreach (var venta in ventas)
                {
                    ActualiceMontosEnUnaVenta(venta.Id);
                }

                return ventas;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al buscar ventas por ID de apertura de caja.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public Model.Venta BusqueVentasPorId(int id)
        {
            try
            {
                var venta = _dbContext.Ventas.Find(id);
                return venta;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al buscar venta por ID.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public List<Model.Inventario> ObtenerTodosLosProductos()
        {
            try
            {
                var productos = _dbContext.Inventarios
                                          .Select(p => new Model.Inventario
                                          {
                                              id = p.id,
                                              Nombre = p.Nombre,
                                              Cantidad = p.Cantidad,
                                              Precio = p.Precio
                                             
                                          })
                                          .ToList();
                return productos;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al obtener todos los productos del inventario.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }


        public Model.Inventario BusqueProductoDelInventarioPorId(int idProducto)
        {
            try
            {
                var producto = _dbContext.Inventarios.Find(idProducto);
                return producto;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al buscar producto del inventario por ID.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public void AgregueVentaDetalle(int idVentaEnCurso, Inventario productoSeleccionado)
        {
            try
            {
                Model.Inventario productoDelInventario = BusqueProductoDelInventarioPorId(productoSeleccionado.id);
                Model.VentaDetalle ventaDetalle = BusqueVentaDetallePorIdInventarioYVenta(productoDelInventario.id, idVentaEnCurso);

                if (ventaDetalle == null)
                {
                    if (productoDelInventario.Cantidad >= productoSeleccionado.Cantidad)
                    {
                        Model.VentaDetalle nuevoVentaDetalle = new Model.VentaDetalle
                        {
                            Id_Venta = idVentaEnCurso,
                            Cantidad = productoSeleccionado.Cantidad,
                            Precio = productoDelInventario.Precio,
                            Monto = productoSeleccionado.Cantidad * productoDelInventario.Precio,
                            Id_Inventario = productoDelInventario.id
                        };

                        productoDelInventario.Cantidad -= productoSeleccionado.Cantidad;

                        _dbContext.Inventarios.Update(productoDelInventario);
                        _dbContext.VentaDetalles.Add(nuevoVentaDetalle);
                    }
                    else
                    {
                        // Manejo de caso donde no hay suficiente cantidad en inventario
                        // throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
                    }
                }
                else
                {
                    if (productoDelInventario.Cantidad >= ventaDetalle.Cantidad + productoSeleccionado.Cantidad)
                    {
                        ventaDetalle.Cantidad += productoSeleccionado.Cantidad;
                        ventaDetalle.Monto = ventaDetalle.Cantidad * ventaDetalle.Precio;

                        productoDelInventario.Cantidad -= productoSeleccionado.Cantidad;

                        _dbContext.Inventarios.Update(productoDelInventario);
                        _dbContext.VentaDetalles.Update(ventaDetalle);
                    }
                    else
                    {
                        // Manejo de caso donde no hay suficiente cantidad en inventario
                        // throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
                    }
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al agregar detalle de venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }



        public Model.VentaDetalle BusqueVentaDetallePorIdInventarioYVenta(int idInventario, int idVenta)
        {
            try
            {
                var ventaDetalle = _dbContext.VentaDetalles
                                           .FirstOrDefault(vd => vd.Id_Inventario == idInventario && vd.Id_Venta == idVenta);

                return ventaDetalle;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al buscar detalle de venta por ID de inventario y venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public void ElimineVentaDetalle(int idVenta, int idInventario)
        {
            try
            {
                var ventaDetalle = _dbContext.VentaDetalles
                                      .FirstOrDefault(vd => vd.Id_Venta == idVenta && vd.Id_Inventario == idInventario);

                if (ventaDetalle != null)
                {
                    var inventario = _dbContext.Inventarios.FirstOrDefault(i => i.id == idInventario);
                    if (inventario != null)
                    {
                        inventario.Cantidad += ventaDetalle.Cantidad;
                        _dbContext.Inventarios.Update(inventario);
                    }

                    _dbContext.VentaDetalles.Remove(ventaDetalle);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al eliminar detalle de venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public void ActualiceMontosEnUnaVenta(int idVentaEnCurso)
        {
            try
            {
                Model.Venta venta = BusqueVentasPorId(idVentaEnCurso);

                var ventaDetalles = _dbContext.VentaDetalles
                                      .Where(vd => vd.Id_Venta == venta.Id)
                                      .ToList();

                double sumaMontos = ventaDetalles.Sum(vd => vd.Monto);
                double sumaMontosDescuentos = ventaDetalles.Sum(vd => vd.MontoDescuento);

                venta.Subtotal = sumaMontos;
                venta.Total = venta.Subtotal - sumaMontosDescuentos;

                _dbContext.Ventas.Update(venta);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al actualizar montos en una venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public List<Model.Inventario> ObtenerInventariosPorVenta(int idVenta)
        {
            try
            {
                var idsInventarios = _dbContext.VentaDetalles
                                              .Where(vd => vd.Id_Venta == idVenta)
                                              .Select(vd => vd.Id_Inventario)
                                              .ToList();

                var inventarios = _dbContext.Inventarios
                                        .Where(inv => idsInventarios.Contains(inv.id))
                                        .ToList();

                foreach (var inventario in inventarios)
                {
                    inventario.VentaDetalles = new List<Model.VentaDetalle>();
                    inventario.VentaDetalles.Add(BusqueVentaDetallePorIdInventarioYVenta(inventario.id, idVenta));
                }

                return inventarios;
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al obtener inventarios por venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }
        public void AgregueDescuento(int id, Model.Venta ventaConDescuento)
        {
            try
            {
                Model.Venta ventaAModificar = BusqueVentasPorId(id);
                ventaAModificar.PorcentajeDesCuento = ventaConDescuento.PorcentajeDesCuento;
                ventaAModificar.MontoDescuento = (ventaAModificar.PorcentajeDesCuento) / 100 * ventaAModificar.Subtotal;
                ventaAModificar.Total = ventaAModificar.Subtotal - ventaAModificar.MontoDescuento;

                _dbContext.Ventas.Update(ventaAModificar);

                List<Model.VentaDetalle> listaDeVentaDetalles = ObtenerVentaDetallesPorVenta(id);

                foreach (var detalle in listaDeVentaDetalles)
                {
                    detalle.MontoDescuento = (ventaAModificar.PorcentajeDesCuento) / 100 * detalle.Monto;
                    _dbContext.VentaDetalles.Update(detalle);
                }

                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al agregar descuento a la venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

        public List<Model.VentaDetalle> ObtenerVentaDetallesPorVenta(int idVenta)
        {
            try
            {
                var ventaDetalles = _dbContext.VentaDetalles
                    .Where(vd => vd.Id_Venta == idVenta)
                    .ToList();

                return ventaDetalles;
            }
            catch (Exception ex)
            {
                
                throw; 
            }
        }

        public void TermineLaVenta(int id, Model.Venta ventaConDescuento)
        {
            try
            {
                Model.Venta ventaAModificar = BusqueVentasPorId(id);
                ventaAModificar.MetodoDePago = ventaConDescuento.MetodoDePago;
                ventaAModificar.Estado = Model.EstadoVenta.Terminada;

                _dbContext.Ventas.Update(ventaAModificar);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                // Log.Error(ex, "Error al terminar la venta.");
                throw; // Re-lanza la excepción para que sea manejada en un nivel superior si es necesario
            }
        }

    }
}
