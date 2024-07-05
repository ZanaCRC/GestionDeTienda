using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public class ModeloVentaDetalle
    {
        public int Id { get; set; }
       // [ForeignKey("Venta")]
        public int Id_Venta { get; set; }
       // public Venta Venta { get; set; }

      //  [ForeignKey("Inventario")]
        public int Id_Inventario { get; set; }
    //    public Inventario Inventario { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double Monto { get; set; }
        public double MontoDescuento { get; set; }
    }
}
