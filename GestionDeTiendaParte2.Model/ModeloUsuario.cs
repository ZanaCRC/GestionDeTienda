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
        
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Correo Electrónico es requerido.")]
        public string CorreoElectronico { get; set; }
        [Required(ErrorMessage = "La Clave es requerida.")]
        public string Clave { get; set; }
       
    }
}
