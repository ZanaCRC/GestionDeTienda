using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public class ModeloUsuario
    {

        public int Id { get; set; }
       
        public string Nombre { get; set; }
      
        public string CorreoElectronico { get; set; }
       
        public string Clave { get; set; }
        public Rol Rol { get; set; }

        public bool EsExterno { get; set; }

       
        public int IntentosFallidos { get; set; } 
        public bool EstaBloqueado { get; set; } 
        public DateTime? FechaBloqueo { get; set; }

      

    }
}
