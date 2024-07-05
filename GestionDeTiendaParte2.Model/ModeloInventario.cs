using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public class ModeloInventario
    {


        public int id { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido.")]
        public string Nombre { get; set; }
        public Categoria Categoria { get; set; }
        public int Cantidad { get; set; }
        [Required(ErrorMessage = "El Precio es requerido.")]
        public double Precio { get; set; }


        public string UserName { get; set; }






    }
}
