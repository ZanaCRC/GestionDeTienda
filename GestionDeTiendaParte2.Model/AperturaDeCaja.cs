using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.Model
{
    public class AperturaDeCaja
    {
        public int Id { get; set; }

        [ForeignKey("Usuario")]
        public int UserId { get; set; }
        public Usuario Usuario { get; set; }
        public DateTime FechaDeInicio { get; set; }
        public DateTime FechaDeCierre { get; set; }
        public string Observaciones { get; set; }

        public EstadoAperturaCaja Estado { get; set; }
        public ICollection<Venta> Ventas { get; set; }

    }
}
