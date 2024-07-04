using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public interface IAdministradorDeCajas
    {

        public bool TieneElUsuarioAlgunaCajaAbierta(int IdUsuario);
        public AperturaDeCaja BusqueUnaCajaActiva(int IdUsuario);
        public AperturaDeCaja BusqueUnaCajaCerrada(int IdUsuario);
        public void AbraUnaCaja(int UserId);
        public void CierreUnaCaja(int UserId);
        public InformacionCaja RealiceLosCalculosDeLaCaja(int IdAperturaCaja);
        public void RegistreUnaCaja(int UserId);
        public AperturaDeCaja BusqueUnaCajaNueva(int IdUsuario);
    }
}
