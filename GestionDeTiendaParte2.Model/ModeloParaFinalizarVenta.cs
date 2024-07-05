using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public class ModeloParaFinalizarVenta
    {
        public MetodoDePago MetodoDePago { get; set; }
        public int IdVenta { get; set; }
    }
}
