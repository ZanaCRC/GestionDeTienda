using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddSwaggerGen();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<GestionDeTiendaParte2.BL.IAdministradorDeUsuarios, GestionDeTiendaParte2.BL.AdministradorDeUsuarios>();
builder.Services.AddScoped<GestionDeTiendaParte2.BL.IAdministradorDeCajas, GestionDeTiendaParte2.BL.AdministradorDeCajas>();
builder.Services.AddScoped<GestionDeTiendaParte2.BL.IAdministradorDeVentas, GestionDeTiendaParte2.BL.AdministradorDeVentas>();
builder.Services.AddTransient<GestionDeTiendaParte2.BL.IAdministradorDeCorreos, GestionDeTiendaParte2.BL.AdministradorDeCorreos>();
builder.Services.AddScoped<GestionDeTiendaParte2.BL.IAdministradorDeAjustesDeInventarios, GestionDeTiendaParte2.BL.AdministradorDeAjustesDeInventarios>();
builder.Services.AddScoped<GestionDeTiendaParte2.BL.IAdministradorDeInventarios, GestionDeTiendaParte2.BL.AdministradorDeInventarios>();


builder.Configuration.AddUserSecrets<Program>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionDeTiendaParte2.DA.DBContexto>(x => x.UseSqlServer(connectionString));


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
