using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public enum Rol
    {
        Administrador = 1,
        [Description("Usuario Normal")]
        Restringido = 2, 
        ConPermiso = 3
    }
}
