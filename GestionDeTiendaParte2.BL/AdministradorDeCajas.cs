using GestionDeTiendaParte1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public class AdministradorDeCajas : IAdministradorDeCajas
    {
        private readonly DA.DBContexto _dbContext;

        public AdministradorDeCajas(DA.DBContexto dbContext)
        {
            _dbContext = dbContext;
        }

        public bool TieneElUsuarioAlgunaCajaAbierta(int idUsuario)
        {
            try
            {
                return _dbContext.AperturasDeCaja
                                .Any(ac => ac.UserId == idUsuario && ac.Estado == EstadoAperturaCaja.Abierta);
            }
            catch (Exception ex)
            {
                
                return false; 
            }
        }

        public AperturaDeCaja BusqueUnaCajaActiva(int idUsuario)
        {
            try
            {
                return _dbContext.AperturasDeCaja
                                .FirstOrDefault(ac => ac.UserId == idUsuario && ac.Estado == EstadoAperturaCaja.Abierta);
            }
            catch (Exception ex)
            {
               
                return null; 
            }
        }

        public AperturaDeCaja BusqueUnaCajaCerrada(int idUsuario)
        {
            try
            {
                return _dbContext.AperturasDeCaja
                                .FirstOrDefault(ac => ac.UserId == idUsuario && ac.Estado == EstadoAperturaCaja.Cerrada);
            }
            catch (Exception ex)
            {
                
                return null; 
            }
        }

        public AperturaDeCaja BusqueUnaCajaNueva(int idUsuario)
        {
            try
            {
                return _dbContext.AperturasDeCaja
                                .FirstOrDefault(ac => ac.UserId == idUsuario && ac.Estado == EstadoAperturaCaja.Nueva);
            }
            catch (Exception ex)
            {
                
                return null; 
            }
        }

        public void AbraUnaCaja(int userId)
        {
            try
            {
                var nuevaAperturaDeCaja = new AperturaDeCaja
                {
                    UserId = userId,
                    FechaDeInicio = DateTime.Now,
                    Estado = EstadoAperturaCaja.Abierta,
                    Observaciones = "Se abrió la caja"
                };

                _dbContext.AperturasDeCaja.Add(nuevaAperturaDeCaja);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
              
                throw; 
            }
        }

        public void RegistreUnaCaja(int userId)
        {
            try
            {
                var nuevaAperturaDeCaja = new AperturaDeCaja
                {
                    UserId = userId,
                    Estado = EstadoAperturaCaja.Nueva,
                    Observaciones = "Se registró una nueva caja"
                };

                _dbContext.AperturasDeCaja.Add(nuevaAperturaDeCaja);
                _dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                throw; 
            }
        }

        public void CierreUnaCaja(int userId)
        {
            try
            {
                var cajaPorCerrar = BusqueUnaCajaActiva(userId);

                if (cajaPorCerrar != null)
                {
                    cajaPorCerrar.FechaDeCierre = DateTime.Now;
                    cajaPorCerrar.Estado = EstadoAperturaCaja.Cerrada;
                    cajaPorCerrar.Observaciones = "Se cerró la caja";

                    _dbContext.AperturasDeCaja.Update(cajaPorCerrar);
                    _dbContext.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public InformacionCaja RealiceLosCalculosDeLaCaja(int idAperturaCaja)
        {
            try
            {
                var informacionDeLosCalculos = new InformacionCaja();

                var ventas = _dbContext.Ventas
                                    .Where(v => v.IdAperturaCaja == idAperturaCaja && v.Estado == EstadoVenta.Terminada)
                                    .ToList();

                foreach (var venta in ventas)
                {
                    switch (venta.MetodoDePago)
                    {
                        case MetodoDePago.Efectivo:
                            informacionDeLosCalculos.AcumuladoEfectivo += venta.Total;
                            break;
                        case MetodoDePago.Tarjeta:
                            informacionDeLosCalculos.AcumuladoTarjeta += venta.Total;
                            break;
                        case MetodoDePago.SinpeMovil:
                            informacionDeLosCalculos.AcumuladoSinpeMovil += venta.Total;
                            break;
                        
                        default:
                            break;
                    }
                }

                return informacionDeLosCalculos;
            }
            catch (Exception ex)
            {
              
                throw;
            }
        }
    }

}
