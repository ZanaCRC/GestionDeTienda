﻿using GestionDeTiendaParte1.Model;
using Microsoft.EntityFrameworkCore;


namespace GestionDeTiendaParte1.DA
{
    public class DBContext : DbContext
    {
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<Inventario> Inventarios { get; set; }
        public DbSet<AjusteDeInventario> AjusteDeInventarios { get; set; }
        public DbSet<AperturaDeCaja> AperturasDeCaja { get; set; }
        public DbSet<Venta> Ventas { get; set; }
        public DbSet<VentaDetalle> VentaDetalles { get; set; }
        public DbSet<Historico> Historico { get; set; }

        public DBContext(DbContextOptions options) : base(options)
        {
        }
    }
}
