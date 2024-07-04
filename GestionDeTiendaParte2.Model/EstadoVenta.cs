using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public enum EstadoVenta
    {
        [Description("En Proceso")]
        Proceso=1,
        Terminada = 2
    }
}
