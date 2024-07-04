using GestionDeTiendaParte2.Model;
using Microsoft.EntityFrameworkCore;


namespace GestionDeTiendaParte2.DA
{
    public class DBContexto : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<AjusteDeInventario> AjusteDeInventarios { get; set; }
        public DbSet<AperturaDeCaja> AperturasDeCaja { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<Historico> Historico { get; set; }

        public DBContexto(DbContextOptions options) : base(options)
        {
        }
    }
}
