using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public enum TipoModificacion
    {
        [Description("Creación")]
        Creacion = 1,
        [Description("Edición")]
        Edicion =2 
    }
}
