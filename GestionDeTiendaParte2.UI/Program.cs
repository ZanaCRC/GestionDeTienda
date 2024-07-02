using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.Google;
using Microsoft.AspNetCore.Authentication.Facebook;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<GestionDeTiendaParte1.BL.IAdministradorDeUsuarios, GestionDeTiendaParte1.BL.AdministradorDeUsuarios>();
builder.Services.AddScoped<GestionDeTiendaParte1.BL.IAdministradorDeCajas, GestionDeTiendaParte1.BL.AdministradorDeCajas>();
builder.Services.AddScoped<GestionDeTiendaParte1.BL.IAdministradorDeVentas, GestionDeTiendaParte1.BL.AdministradorDeVentas>();
builder.Services.AddTransient<GestionDeTiendaParte1.BL.IAdministradorDeCorreos, GestionDeTiendaParte1.BL.AdministradorDeCorreos>();
builder.Services.AddScoped<GestionDeTiendaParte1.BL.IAdministradorDeAjustesDeInventarios, GestionDeTiendaParte1.BL.AdministradorDeAjustesDeInventarios>();
builder.Services.AddScoped<GestionDeTiendaParte1.BL.IAdministradorDeInventarios, GestionDeTiendaParte1.BL.AdministradorDeInventarios>();


builder.Configuration.AddUserSecrets<Program>();
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<GestionDeTiendaParte1.DA.DBContext>(x => x.UseSqlServer(connectionString));

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultSignInScheme = CookieAuthenticationDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = CookieAuthenticationDefaults.AuthenticationScheme;

})
.AddCookie(options =>
{
    options.LoginPath = "/Login/Loguearse";
    options.AccessDeniedPath = "/Login/Loguearse";
    options.ExpireTimeSpan = TimeSpan.FromMinutes(15);
})
.AddGoogle(options =>
{
    options.ClientId = builder.Configuration.GetSection("GoogleKeys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("GoogleKeys:ClientSecret").Value;
})
.AddFacebook(options =>
{
    options.ClientId = builder.Configuration.GetSection("Facebook_Keys:ClientId").Value;
    options.ClientSecret = builder.Configuration.GetSection("Facebook_Keys:ClientSecret").Value;
});



var app = builder.Build();




// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


//Reveal the secret on web 😉


app.Run();
