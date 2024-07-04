using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GestionDeTiendaParte2.DA.Migrations
{
    /// <inheritdoc />
    public partial class Comercio : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Inventarios",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Categoria = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    IsSelected = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Inventarios", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "Usuarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Nombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CorreoElectronico = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Clave = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Rol = table.Column<int>(type: "int", nullable: false),
                    EsExterno = table.Column<bool>(type: "bit", nullable: false),
                    IntentosFallidos = table.Column<int>(type: "int", nullable: false),
                    EstaBloqueado = table.Column<bool>(type: "bit", nullable: false),
                    FechaBloqueo = table.Column<DateTime>(type: "datetime2", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Usuarios", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historico",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ElTipoDeModificacion = table.Column<int>(type: "int", nullable: false),
                    FechaYHora = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreUsuario = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ElNombre = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    LaCategoria = table.Column<int>(type: "int", nullable: false),
                    ElPrecio = table.Column<double>(type: "float", nullable: false),
                    IdInventario = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historico", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Historico_Inventarios_IdInventario",
                        column: x => x.IdInventario,
                        principalTable: "Inventarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AjusteDeInventarios",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Inventario = table.Column<int>(type: "int", nullable: false),
                    CantidadActual = table.Column<int>(type: "int", nullable: false),
                    Ajuste = table.Column<int>(type: "int", nullable: false),
                    Tipo = table.Column<int>(type: "int", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AjusteDeInventarios", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AjusteDeInventarios_Inventarios_Id_Inventario",
                        column: x => x.Id_Inventario,
                        principalTable: "Inventarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AjusteDeInventarios_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AperturasDeCaja",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    FechaDeInicio = table.Column<DateTime>(type: "datetime2", nullable: false),
                    FechaDeCierre = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Observaciones = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AperturasDeCaja", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AperturasDeCaja_Usuarios_UserId",
                        column: x => x.UserId,
                        principalTable: "Usuarios",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Ventas",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Fecha = table.Column<DateTime>(type: "datetime2", nullable: false),
                    NombreCliente = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Total = table.Column<double>(type: "float", nullable: false),
                    Subtotal = table.Column<double>(type: "float", nullable: false),
                    PorcentajeDesCuento = table.Column<double>(type: "float", nullable: false),
                    MontoDescuento = table.Column<double>(type: "float", nullable: false),
                    Estado = table.Column<int>(type: "int", nullable: false),
                    IdAperturaCaja = table.Column<int>(type: "int", nullable: false),
                    MetodoDePago = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Ventas", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Ventas_AperturasDeCaja_IdAperturaCaja",
                        column: x => x.IdAperturaCaja,
                        principalTable: "AperturasDeCaja",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VentaDetalles",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Id_Venta = table.Column<int>(type: "int", nullable: false),
                    Id_Inventario = table.Column<int>(type: "int", nullable: false),
                    Cantidad = table.Column<int>(type: "int", nullable: false),
                    Precio = table.Column<double>(type: "float", nullable: false),
                    Monto = table.Column<double>(type: "float", nullable: false),
                    MontoDescuento = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VentaDetalles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VentaDetalles_Inventarios_Id_Inventario",
                        column: x => x.Id_Inventario,
                        principalTable: "Inventarios",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VentaDetalles_Ventas_Id_Venta",
                        column: x => x.Id_Venta,
                        principalTable: "Ventas",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AjusteDeInventarios_Id_Inventario",
                table: "AjusteDeInventarios",
                column: "Id_Inventario");

            migrationBuilder.CreateIndex(
                name: "IX_AjusteDeInventarios_UserId",
                table: "AjusteDeInventarios",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_AperturasDeCaja_UserId",
                table: "AperturasDeCaja",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Historico_IdInventario",
                table: "Historico",
                column: "IdInventario");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_Id_Inventario",
                table: "VentaDetalles",
                column: "Id_Inventario");

            migrationBuilder.CreateIndex(
                name: "IX_VentaDetalles_Id_Venta",
                table: "VentaDetalles",
                column: "Id_Venta");

            migrationBuilder.CreateIndex(
                name: "IX_Ventas_IdAperturaCaja",
                table: "Ventas",
                column: "IdAperturaCaja");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AjusteDeInventarios");

            migrationBuilder.DropTable(
                name: "Historico");

            migrationBuilder.DropTable(
                name: "VentaDetalles");

            migrationBuilder.DropTable(
                name: "Inventarios");

            migrationBuilder.DropTable(
                name: "Ventas");

            migrationBuilder.DropTable(
                name: "AperturasDeCaja");

            migrationBuilder.DropTable(
                name: "Usuarios");
        }
    }
}
