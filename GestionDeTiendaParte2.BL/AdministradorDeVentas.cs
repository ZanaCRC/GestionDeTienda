using GestionDeTiendaParte2.Model;
using Microsoft.EntityFrameworkCore;
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

        public List<Model.ModeloVenta> BusqueVentasPorIdAperturaCaja(int idAperturaCaja)
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
                List<ModeloVenta> modeloVentas = ConvertirAVentasModelo(ventas);
                return modeloVentas;
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

        public void AgregueVentaDetalle(int idVentaEnCurso, ModeloInventario productoSeleccionado)
        {
            try
            {
                Model.Inventario productoDelInventario = BusqueProductoDelInventarioPorId(productoSeleccionado.id);
                Model.ModeloVentaDetalle modeloVentaDetalle = BusqueVentaDetallePorIdInventarioYVenta(productoDelInventario.id, idVentaEnCurso);

                if (modeloVentaDetalle == null)
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
                        throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
                    }
                }
                else
                {
                    if (productoDelInventario.Cantidad >= modeloVentaDetalle.Cantidad + productoSeleccionado.Cantidad)
                    {
                        modeloVentaDetalle.Cantidad += productoSeleccionado.Cantidad;
                        modeloVentaDetalle.Monto = modeloVentaDetalle.Cantidad * modeloVentaDetalle.Precio;

                        productoDelInventario.Cantidad -= productoSeleccionado.Cantidad;

                        _dbContext.Inventarios.Update(productoDelInventario);

                        // Verifica si la entidad ya está siendo rastreada
                        var ventaDetalleExistente = _dbContext.VentaDetalles
                            .Local
                            .FirstOrDefault(vd => vd.Id == modeloVentaDetalle.Id);

                        if (ventaDetalleExistente != null)
                        {
                            _dbContext.Entry(ventaDetalleExistente).State = EntityState.Detached;
                        }

                        var ventaDetalle = new VentaDetalle
                        {
                            Id = modeloVentaDetalle.Id,
                            Id_Venta = modeloVentaDetalle.Id_Venta,
                            Id_Inventario = modeloVentaDetalle.Id_Inventario,
                            Cantidad = modeloVentaDetalle.Cantidad,
                            Precio = modeloVentaDetalle.Precio,
                            Monto = modeloVentaDetalle.Monto,
                            MontoDescuento = modeloVentaDetalle.MontoDescuento
                        };

                        _dbContext.VentaDetalles.Update(ventaDetalle);
                    }
                    else
                    {
                        // Manejo de caso donde no hay suficiente cantidad en inventario
                        throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
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


        public ModeloVenta ConvertirAVentaModelo(Venta venta)
        {
            var modeloVenta = new ModeloVenta
            {
                Id = venta.Id,
                Fecha = venta.Fecha,
                NombreCliente = venta.NombreCliente,
                Total = venta.Total,
                Subtotal = venta.Subtotal,
                PorcentajeDesCuento = venta.PorcentajeDesCuento,
                MontoDescuento = venta.MontoDescuento,
                Estado = venta.Estado,
                IdAperturaCaja = venta.IdAperturaCaja,
                MetodoDePago = venta.MetodoDePago
            };

            return modeloVenta;
        }

        public List<ModeloVenta> ConvertirAVentasModelo(List<Venta> ventas)
        {
            var modelosVentas = new List<ModeloVenta>();

            foreach (var venta in ventas)
            {
                var modeloVenta = new ModeloVenta
                {
                    Id = venta.Id,
                    Fecha = venta.Fecha,
                    NombreCliente = venta.NombreCliente,
                    Total = venta.Total,
                    Subtotal = venta.Subtotal,
                    PorcentajeDesCuento = venta.PorcentajeDesCuento,
                    MontoDescuento = venta.MontoDescuento,
                    Estado = venta.Estado,
                    IdAperturaCaja = venta.IdAperturaCaja,
                    MetodoDePago = venta.MetodoDePago
                };

                modelosVentas.Add(modeloVenta);
            }

            return modelosVentas;
        }


        //ayuda dios
        public Model.ModeloVentaDetalle BusqueVentaDetallePorIdInventarioYVenta(int idInventario, int idVenta)
        {
            try
            {
                var ventaDetalle = _dbContext.VentaDetalles
                                           .FirstOrDefault(vd => vd.Id_Inventario == idInventario && vd.Id_Venta == idVenta);

                if (ventaDetalle == null)
                {
                    
                    return null; 
                }

                var modeloVentaDetalle = new ModeloVentaDetalle
                {
                    Id = ventaDetalle.Id,
                    Id_Venta = ventaDetalle.Id_Venta,
                    Id_Inventario = ventaDetalle.Id_Inventario,
                    Cantidad = ventaDetalle.Cantidad,
                    Precio = ventaDetalle.Precio,
                    Monto = ventaDetalle.Monto,
                    MontoDescuento = ventaDetalle.MontoDescuento
                };

                return modeloVentaDetalle;
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

        public List<ModeloParaMostrarInventarioDeUnaVenta> ObtenerInventariosPorVenta(int idVenta)
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

                var modelosParaMostrar = new List<ModeloParaMostrarInventarioDeUnaVenta>();

                foreach (var inventario in inventarios)
                {
                    var modeloVentaDetalle = BusqueVentaDetallePorIdInventarioYVenta(inventario.id, idVenta);

                    if (modeloVentaDetalle != null)
                    {
                        var modeloParaMostrar = new ModeloParaMostrarInventarioDeUnaVenta
                        {
                            id = inventario.id,
                            Nombre = inventario.Nombre,
                            Categoria = inventario.Categoria,
                            Cantidad = modeloVentaDetalle.Cantidad,
                            Precio = inventario.Precio,
                            MontoDescuento = modeloVentaDetalle.MontoDescuento,
                            Monto = modeloVentaDetalle.Monto
                        };

                        modelosParaMostrar.Add(modeloParaMostrar);
                    }
                }

                return modelosParaMostrar;
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
