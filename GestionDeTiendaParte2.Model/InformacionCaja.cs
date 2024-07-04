using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public  class InformacionCaja
    {
        public double AcumuladoTarjeta { get; set; }
        public double AcumuladoEfectivo { get; set; }
        public double AcumuladoSinpeMovil { get; set; }
        public AperturaDeCaja Caja { get; set; }

    }
}
