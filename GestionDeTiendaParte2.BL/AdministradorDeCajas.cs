using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public class AdministradorDeCajas : IAdministradorDeCajas
    {
        private GestionDeTiendaParte2.DA.DBContexto ElContextoBD;

        public AdministradorDeCajas(DA.DBContexto dbContext)
        {
            ElContextoBD = dbContext;
        }

        public bool TieneElUsuarioAlgunaCajaAbierta(int idUsuario)
        {
            try
            {
                return ElContextoBD.AperturasDeCaja
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
                return ElContextoBD.AperturasDeCaja
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
                return ElContextoBD.AperturasDeCaja
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
                return ElContextoBD.AperturasDeCaja
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
                var laNuevaAperturaDeCaja = new AperturaDeCaja
                {
                    UserId = userId,
                    FechaDeInicio = DateTime.Now,
                    Estado = EstadoAperturaCaja.Abierta,
                    Observaciones = "Se abrió la caja"
                };

                ElContextoBD.AperturasDeCaja.Add(laNuevaAperturaDeCaja);
                ElContextoBD.SaveChanges();
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
                var laNuevaAperturaDeCaja = new AperturaDeCaja
                {
                    UserId = userId,
                    Estado = EstadoAperturaCaja.Nueva,
                    Observaciones = "Se registró una nueva caja"
                };

                ElContextoBD.AperturasDeCaja.Add(laNuevaAperturaDeCaja);
                ElContextoBD.SaveChanges();
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
                var laCajaPorCerrar = BusqueUnaCajaActiva(userId);

                if (laCajaPorCerrar != null)
                {
                    laCajaPorCerrar.FechaDeCierre = DateTime.Now;
                    laCajaPorCerrar.Estado = EstadoAperturaCaja.Cerrada;
                    laCajaPorCerrar.Observaciones = "Se cerró la caja";

                    ElContextoBD.AperturasDeCaja.Update(laCajaPorCerrar);
                    ElContextoBD.SaveChanges();
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

                var lasVentas = ElContextoBD.Ventas
                                    .Where(v => v.IdAperturaCaja == idAperturaCaja && v.Estado == EstadoVenta.Terminada)
                                    .ToList();

                foreach (var venta in lasVentas)
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
