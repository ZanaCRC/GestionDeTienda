using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.Model
{
    public class Venta
    {
        public int Id { get; set; }
        public DateTime Fecha { get; set; }
        [Required(ErrorMessage = "El nombre del cliente es requerido.")]
        public string NombreCliente { get; set; }
        public double Total { get; set; }
        public double Subtotal { get; set; }
        [Required(ErrorMessage = "Se requiere porcentaje de descuento")]
        public double PorcentajeDesCuento { get; set; }
        public double MontoDescuento { get; set; }
        public EstadoVenta Estado { get; set; }
        [ForeignKey("AperturaDeCaja")]
        public int IdAperturaCaja { get; set; }
        public MetodoDePago MetodoDePago { get; set; }
        public AperturaDeCaja AperturaDeCaja { get; set; }
        public ICollection<VentaDetalle> Detalles { get; set; }
        


    }
}
