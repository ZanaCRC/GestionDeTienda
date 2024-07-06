using GestionDeTiendaParte2.UI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http;
using Microsoft.AspNetCore.WebUtilities;
using GestionDeTiendaParte2.Model;
namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly HttpClient httpClient;
        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
            httpClient = new HttpClient();
        }

        public IActionResult Index()
        {
            return View();
        }
       


[HttpPost]
[ValidateAntiForgeryToken]
public async Task<ActionResult> DePermisoAUsuario(int id)
        {
            try
            {
                var httpClient = new HttpClient();
                var query = new Dictionary<string, string>()
                {
                    ["id"] = id.ToString()

                }; 

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeUsuarios/DePermisos", query);
                var response = await httpClient.PostAsync(uri, null);
                return RedirectToAction("Index");
            }
            catch (Exception e)
            {

                return View(e);
            }
        }


    public async Task<ActionResult> AdministrarUsuarios()
        {
            var uri = "https://localhost:7001/api/ServicioDeUsuarios/ObtenerListaDeUsuarios";
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<Model.Usuario> ListaDeUsuarios = JsonConvert.DeserializeObject<List<Model.Usuario>>(apiResponse);
                return View(ListaDeUsuarios);
            }
            else
            {
                
                ViewBag.ErrorMessage = "Error al obtener la lista de inventario desde el servidor.";
                return View();
            }
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public async Task<IActionResult> CerrarSesion()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Loguearse", "Login");
            
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
