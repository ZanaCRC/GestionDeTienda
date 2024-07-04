using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public enum TipoInventario
    {
        [Description("Aumento")]
        Aumento,
        [Description("Disminución")]
        Disminucion
    }
}
