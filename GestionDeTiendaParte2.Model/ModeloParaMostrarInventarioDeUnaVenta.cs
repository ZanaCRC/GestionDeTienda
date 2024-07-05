using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.Model
{
    public class ModeloParaMostrarInventarioDeUnaVenta
    {
        public int id { get; set; }
        public string Nombre { get; set; }
        public Categoria Categoria { get; set; }
        public int Cantidad { get; set; }
        public double Precio { get; set; }
        public double MontoDescuento { get; set; }
        public double Monto { get; set; }
        public double Subtotal => Monto - MontoDescuento;
    }
}
