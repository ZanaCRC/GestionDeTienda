﻿// <auto-generated />
using System;
using GestionDeTiendaParte2.DA;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace GestionDeTiendaParte2.DA.Migrations
{
    [DbContext(typeof(DBContexto))]
    [Migration("20240615101048_Comercio")]
    partial class Comercio
    {
        /// <inheritdoc />
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "8.0.6")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("GestionDeTiendaParte1.Model.AjusteDeInventario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Ajuste")
                        .HasColumnType("int");

                    b.Property<int>("CantidadActual")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("Id_Inventario")
                        .HasColumnType("int");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Tipo")
                        .HasColumnType("int");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("Id_Inventario");

                    b.HasIndex("UserId");

                    b.ToTable("AjusteDeInventarios");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.AperturaDeCaja", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaDeCierre")
                        .HasColumnType("datetime2");

                    b.Property<DateTime>("FechaDeInicio")
                        .HasColumnType("datetime2");

                    b.Property<string>("Observaciones")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("UserId")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.HasIndex("UserId");

                    b.ToTable("AperturasDeCaja");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Historico", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("ElNombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("ElPrecio")
                        .HasColumnType("float");

                    b.Property<int>("ElTipoDeModificacion")
                        .HasColumnType("int");

                    b.Property<DateTime>("FechaYHora")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdInventario")
                        .HasColumnType("int");

                    b.Property<int>("LaCategoria")
                        .HasColumnType("int");

                    b.Property<string>("NombreUsuario")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("IdInventario");

                    b.ToTable("Historico");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Inventario", b =>
                {
                    b.Property<int>("id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("Categoria")
                        .HasColumnType("int");

                    b.Property<bool>("IsSelected")
                        .HasColumnType("bit");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.HasKey("id");

                    b.ToTable("Inventarios");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Usuario", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<string>("Clave")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CorreoElectronico")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("EsExterno")
                        .HasColumnType("bit");

                    b.Property<bool>("EstaBloqueado")
                        .HasColumnType("bit");

                    b.Property<DateTime?>("FechaBloqueo")
                        .HasColumnType("datetime2");

                    b.Property<int>("IntentosFallidos")
                        .HasColumnType("int");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Rol")
                        .HasColumnType("int");

                    b.HasKey("Id");

                    b.ToTable("Usuarios");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Venta", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Estado")
                        .HasColumnType("int");

                    b.Property<DateTime>("Fecha")
                        .HasColumnType("datetime2");

                    b.Property<int>("IdAperturaCaja")
                        .HasColumnType("int");

                    b.Property<int>("MetodoDePago")
                        .HasColumnType("int");

                    b.Property<double>("MontoDescuento")
                        .HasColumnType("float");

                    b.Property<string>("NombreCliente")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<double>("PorcentajeDesCuento")
                        .HasColumnType("float");

                    b.Property<double>("Subtotal")
                        .HasColumnType("float");

                    b.Property<double>("Total")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("IdAperturaCaja");

                    b.ToTable("Ventas");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.VentaDetalle", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("Id"));

                    b.Property<int>("Cantidad")
                        .HasColumnType("int");

                    b.Property<int>("Id_Inventario")
                        .HasColumnType("int");

                    b.Property<int>("Id_Venta")
                        .HasColumnType("int");

                    b.Property<double>("Monto")
                        .HasColumnType("float");

                    b.Property<double>("MontoDescuento")
                        .HasColumnType("float");

                    b.Property<double>("Precio")
                        .HasColumnType("float");

                    b.HasKey("Id");

                    b.HasIndex("Id_Inventario");

                    b.HasIndex("Id_Venta");

                    b.ToTable("VentaDetalles");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.AjusteDeInventario", b =>
                {
                    b.HasOne("GestionDeTiendaParte1.Model.Inventario", "Inventario")
                        .WithMany("AjusteDeInventarios")
                        .HasForeignKey("Id_Inventario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionDeTiendaParte1.Model.Usuario", "Usuario")
                        .WithMany("Ajustes")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventario");

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.AperturaDeCaja", b =>
                {
                    b.HasOne("GestionDeTiendaParte1.Model.Usuario", "Usuario")
                        .WithMany("Aperturas")
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Usuario");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Historico", b =>
                {
                    b.HasOne("GestionDeTiendaParte1.Model.Inventario", "Inventario")
                        .WithMany("Historicos")
                        .HasForeignKey("IdInventario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventario");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Venta", b =>
                {
                    b.HasOne("GestionDeTiendaParte1.Model.AperturaDeCaja", "AperturaDeCaja")
                        .WithMany("Ventas")
                        .HasForeignKey("IdAperturaCaja")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("AperturaDeCaja");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.VentaDetalle", b =>
                {
                    b.HasOne("GestionDeTiendaParte1.Model.Inventario", "Inventario")
                        .WithMany("VentaDetalles")
                        .HasForeignKey("Id_Inventario")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("GestionDeTiendaParte1.Model.Venta", "Venta")
                        .WithMany("Detalles")
                        .HasForeignKey("Id_Venta")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Inventario");

                    b.Navigation("Venta");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.AperturaDeCaja", b =>
                {
                    b.Navigation("Ventas");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Inventario", b =>
                {
                    b.Navigation("AjusteDeInventarios");

                    b.Navigation("Historicos");

                    b.Navigation("VentaDetalles");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Usuario", b =>
                {
                    b.Navigation("Ajustes");

                    b.Navigation("Aperturas");
                });

            modelBuilder.Entity("GestionDeTiendaParte1.Model.Venta", b =>
                {
                    b.Navigation("Detalles");
                });
#pragma warning restore 612, 618
        }
    }
}
