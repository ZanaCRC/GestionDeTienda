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
        private readonly DA.DBContexto ElContextoBD;

        public AdministradorDeVentas(DA.DBContexto dbContext)
        {
            ElContextoBD = dbContext;
        }

        public void AgregueUnaNuevaVenta(Model.ModeloCrearVenta nuevaVentaNombre, Model.AperturaDeCaja cajaAbierta)
        {
            try
            {
                Model.Venta laNuevaVenta = new Venta();
                laNuevaVenta.Fecha = DateTime.Now;
                laNuevaVenta.IdAperturaCaja = cajaAbierta.Id;
                laNuevaVenta.Estado = Model.EstadoVenta.Proceso;
                laNuevaVenta.NombreCliente = nuevaVentaNombre.NombreCliente;

                ElContextoBD.Ventas.Add(laNuevaVenta);
                ElContextoBD.SaveChanges();
            }
            catch (Exception ex)
            {
                
                throw; 
            }
        }

        public List<Model.ModeloVenta> BusqueVentasPorIdAperturaCaja(int idAperturaCaja)
        {
            try
            {
                var lasVentas = ElContextoBD.Ventas
                                      .Where(v => v.IdAperturaCaja == idAperturaCaja)
                                      .ToList();

                foreach (var venta in lasVentas)
                {
                    ActualiceMontosEnUnaVenta(venta.Id);
                }
                List<ModeloVenta> modeloVentas = ConvertirAVentasModelo(lasVentas);
                return modeloVentas;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public Model.Venta BusqueVentasPorId(int id)
        {
            try
            {
                var laVenta = ElContextoBD.Ventas.Find(id);
                return laVenta;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Model.Inventario> ObtenerTodosLosProductos()
        {
            try
            {
                var losProductos = ElContextoBD.Inventarios
                                          .Select(p => new Model.Inventario
                                          {
                                              id = p.id,
                                              Nombre = p.Nombre,
                                              Cantidad = p.Cantidad,
                                              Precio = p.Precio
                                             
                                          })
                                          .ToList();
                return losProductos;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }


        public Model.Inventario BusqueProductoDelInventarioPorId(int idProducto)
        {
            try
            {
                var elProducto = ElContextoBD.Inventarios.Find(idProducto);
                return elProducto;
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public void AgregueVentaDetalle(int idVentaEnCurso, ModeloInventario elProductoSeleccionado)
        {
            try
            {
                Model.Inventario elProductoDelInventario = BusqueProductoDelInventarioPorId(elProductoSeleccionado.id);
                Model.ModeloVentaDetalle elModeloVentaDetalle = BusqueVentaDetallePorIdInventarioYVenta(elProductoDelInventario.id, idVentaEnCurso);

                if (elModeloVentaDetalle == null)
                {
                    if (elProductoDelInventario.Cantidad >= elProductoSeleccionado.Cantidad)
                    {
                        Model.VentaDetalle nuevoVentaDetalle = new Model.VentaDetalle
                        {
                            Id_Venta = idVentaEnCurso,
                            Cantidad = elProductoSeleccionado.Cantidad,
                            Precio = elProductoDelInventario.Precio,
                            Monto = elProductoSeleccionado.Cantidad * elProductoDelInventario.Precio,
                            Id_Inventario = elProductoDelInventario.id
                        };

                        ElContextoBD.VentaDetalles.Add(nuevoVentaDetalle);
                    }
                    else
                    {
                        throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
                    }
                }
                else
                {
                    if (elProductoDelInventario.Cantidad >= elModeloVentaDetalle.Cantidad + elProductoSeleccionado.Cantidad)
                    {
                        elModeloVentaDetalle.Cantidad += elProductoSeleccionado.Cantidad;
                        elModeloVentaDetalle.Monto = elModeloVentaDetalle.Cantidad * elModeloVentaDetalle.Precio;

                        var laVentaDetalleExistente = ElContextoBD.VentaDetalles
                            .Local
                            .FirstOrDefault(vd => vd.Id == elModeloVentaDetalle.Id);

                        if (laVentaDetalleExistente != null)
                        {
                            ElContextoBD.Entry(laVentaDetalleExistente).State = EntityState.Detached;
                        }

                        var laVentaDetalle = new VentaDetalle
                        {
                            Id = elModeloVentaDetalle.Id,
                            Id_Venta = elModeloVentaDetalle.Id_Venta,
                            Id_Inventario = elModeloVentaDetalle.Id_Inventario,
                            Cantidad = elModeloVentaDetalle.Cantidad,
                            Precio = elModeloVentaDetalle.Precio,
                            Monto = elModeloVentaDetalle.Monto,
                            MontoDescuento = elModeloVentaDetalle.MontoDescuento
                        };

                        ElContextoBD.VentaDetalles.Update(laVentaDetalle);
                    }
                    else
                    {
                        throw new InvalidOperationException("No hay suficiente cantidad en inventario.");
                    }
                }

                ElContextoBD.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public ModeloVenta ConvertirAVentaModelo(Venta laVenta)
        {
            var elModeloVenta = new ModeloVenta
            {
                Id = laVenta.Id,
                Fecha = laVenta.Fecha,
                NombreCliente = laVenta.NombreCliente,
                Total = laVenta.Total,
                Subtotal = laVenta.Subtotal,
                PorcentajeDesCuento = laVenta.PorcentajeDesCuento,
                MontoDescuento = laVenta.MontoDescuento,
                Estado = laVenta.Estado,
                IdAperturaCaja = laVenta.IdAperturaCaja,
                MetodoDePago = laVenta.MetodoDePago
            };

            return elModeloVenta;
        }

        public List<ModeloVenta> ConvertirAVentasModelo(List<Venta> lasVentas)
        {
            var losModelosVentas = new List<ModeloVenta>();

            foreach (var laVenta in lasVentas)
            {
                var elModeloVenta = new ModeloVenta
                {
                    Id = laVenta.Id,
                    Fecha = laVenta.Fecha,
                    NombreCliente = laVenta.NombreCliente,
                    Total = laVenta.Total,
                    Subtotal = laVenta.Subtotal,
                    PorcentajeDesCuento = laVenta.PorcentajeDesCuento,
                    MontoDescuento = laVenta.MontoDescuento,
                    Estado = laVenta.Estado,
                    IdAperturaCaja = laVenta.IdAperturaCaja,
                    MetodoDePago = laVenta.MetodoDePago
                };

                losModelosVentas.Add(elModeloVenta);
            }

            return losModelosVentas;
        }

        public Model.ModeloVentaDetalle BusqueVentaDetallePorIdInventarioYVenta(int idInventario, int idVenta)
        {
            try
            {
                var laVentaDetalle = ElContextoBD.VentaDetalles
                                           .FirstOrDefault(vd => vd.Id_Inventario == idInventario && vd.Id_Venta == idVenta);

                if (laVentaDetalle == null)
                {
                    
                    return null; 
                }

                var elModeloVentaDetalle = new ModeloVentaDetalle
                {
                    Id = laVentaDetalle.Id,
                    Id_Venta = laVentaDetalle.Id_Venta,
                    Id_Inventario = laVentaDetalle.Id_Inventario,
                    Cantidad = laVentaDetalle.Cantidad,
                    Precio = laVentaDetalle.Precio,
                    Monto = laVentaDetalle.Monto,
                    MontoDescuento = laVentaDetalle.MontoDescuento
                };

                return elModeloVentaDetalle;
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        public void ElimineVentaDetalle(int idVenta, int idInventario)
        {
            try
            {
                var laVentaDetalle = ElContextoBD.VentaDetalles
                                      .FirstOrDefault(vd => vd.Id_Venta == idVenta && vd.Id_Inventario == idInventario);

                if (laVentaDetalle != null)
                {
                    var elInventario = ElContextoBD.Inventarios.FirstOrDefault(i => i.id == idInventario);
                    if (elInventario != null)
                    {
                        elInventario.Cantidad += laVentaDetalle.Cantidad;
                        ElContextoBD.Inventarios.Update(elInventario);
                    }

                    ElContextoBD.VentaDetalles.Remove(laVentaDetalle);
                    ElContextoBD.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public void ActualiceMontosEnUnaVenta(int idVentaEnCurso)
        {
            try
            {
                Model.Venta laVenta = BusqueVentasPorId(idVentaEnCurso);

                var ventaDetalles = ElContextoBD.VentaDetalles
                                      .Where(vd => vd.Id_Venta == laVenta.Id)
                                      .ToList();

                double laSumaMontos = ventaDetalles.Sum(vd => vd.Monto);
                double laSumaMontosDescuentos = ventaDetalles.Sum(vd => vd.MontoDescuento);

                laVenta.Subtotal = laSumaMontos;
                laVenta.Total = laVenta.Subtotal - laSumaMontosDescuentos;

                ElContextoBD.Ventas.Update(laVenta);
                ElContextoBD.SaveChanges();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public List<ModeloParaMostrarInventarioDeUnaVenta> ObtenerInventariosPorVenta(int idVenta)
        {
            try
            {
                var losIdsInventarios = ElContextoBD.VentaDetalles
                                              .Where(vd => vd.Id_Venta == idVenta)
                                              .Select(vd => vd.Id_Inventario)
                                              .ToList();

                var losInventarios = ElContextoBD.Inventarios
                                        .Where(inv => losIdsInventarios.Contains(inv.id))
                                        .ToList();

                var losModelosParaMostrar = new List<ModeloParaMostrarInventarioDeUnaVenta>();

                foreach (var elInventario in losInventarios)
                {
                    var elModeloVentaDetalle = BusqueVentaDetallePorIdInventarioYVenta(elInventario.id, idVenta);

                    if (elModeloVentaDetalle != null)
                    {
                        var modeloParaMostrar = new ModeloParaMostrarInventarioDeUnaVenta
                        {
                            id = elInventario.id,
                            Nombre = elInventario.Nombre,
                            Categoria = elInventario.Categoria,
                            Cantidad = elModeloVentaDetalle.Cantidad,
                            Precio = elInventario.Precio,
                            MontoDescuento = elModeloVentaDetalle.MontoDescuento,
                            Monto = elModeloVentaDetalle.Monto
                        };

                        losModelosParaMostrar.Add(modeloParaMostrar);
                    }
                }

                return losModelosParaMostrar;
            }
            catch (Exception ex)
            {
                throw; 
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

                ElContextoBD.Ventas.Update(ventaAModificar);

                List<Model.VentaDetalle> listaDeVentaDetalles = ObtenerVentaDetallesPorVenta(id);

                foreach (var elDetalle in listaDeVentaDetalles)
                {
                    elDetalle.MontoDescuento = (ventaAModificar.PorcentajeDesCuento) / 100 * elDetalle.Monto;
                    ElContextoBD.VentaDetalles.Update(elDetalle);
                }

                ElContextoBD.SaveChanges();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public List<Model.VentaDetalle> ObtenerVentaDetallesPorVenta(int idVenta)
        {
            try
            {
                var laVentaDetalles = ElContextoBD.VentaDetalles
                    .Where(vd => vd.Id_Venta == idVenta)
                    .ToList();

                return laVentaDetalles;
            }
            catch (Exception ex)
            {
                
                throw; 
            }
        }

        public string TermineLaVenta(int id, Model.Venta ventaConDescuento)
        {
            try
            {
                Model.Venta ventaAModificar = BusqueVentasPorId(id);
                ventaAModificar.MetodoDePago = ventaConDescuento.MetodoDePago;
                ventaAModificar.Estado = Model.EstadoVenta.Terminada;

                // Disminuir las cantidades en inventario
                var detallesDeVenta = ElContextoBD.VentaDetalles.Where(vd => vd.Id_Venta == id).ToList();
                StringBuilder productosNoDisponibles = new StringBuilder();

                foreach (var detalle in detallesDeVenta)
                {
                    var productoInventario = ElContextoBD.Inventarios.Find(detalle.Id_Inventario);
                    if (productoInventario != null && productoInventario.Cantidad >= detalle.Cantidad)
                    {
                        productoInventario.Cantidad -= detalle.Cantidad;
                        ElContextoBD.Inventarios.Update(productoInventario);
                    }
                    else
                    {
                        if (productoInventario != null)
                        {
                            productosNoDisponibles.AppendLine($"Producto: {productoInventario.Nombre}, Cantidad requerida: {detalle.Cantidad}, Cantidad disponible: {productoInventario.Cantidad}");
                        }
                    }
                }

                if (productosNoDisponibles.Length > 0)
                {
                    return $"Los siguientes productos no están disponibles en la cantidad requerida:\n{productosNoDisponibles.ToString()}";
                }

                ElContextoBD.Ventas.Update(ventaAModificar);
                ElContextoBD.SaveChanges();

                return "Venta finalizada con éxito.";
            }
            catch (Exception ex)
            {
                // Manejo de la excepción y posible logging
                return $"Se produjo un error al finalizar la venta. Por favor, inténtelo de nuevo. Error: {ex.Message}";
            }
        }


    }
}
