using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.Model
{
    public class Historico
    {
        public int Id { get; set; }
        public TipoModificacion ElTipoDeModificacion { get; set; }
        public DateTime FechaYHora { get; set; }
        public String NombreUsuario { get; set; }
     
        public String ElNombre { get; set; }
        public Categoria LaCategoria { get; set; }
        public double ElPrecio { get; set; }

        [ForeignKey("Inventario")]
        public int IdInventario { get; set; }
        public Inventario Inventario { get; set; }

    }
}
