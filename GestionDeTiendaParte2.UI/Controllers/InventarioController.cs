using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Net.Http.Headers;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        private readonly HttpClient httpClient;

        public InventarioController()
        {
            httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(string nombre)
        {
           

            List<Model.Inventario> lista;
            var httpClient = new HttpClient();
            try
            {

                var respuesta = await httpClient.GetAsync("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/ObtenerLista");
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse);

                if (nombre is null)
                    return View(lista);
                else
                { 
  
                    var query = new Dictionary<string, string>()
                    {
                        ["nombre"] = nombre.ToString()
                    };

                    var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/FiltreLaLista", query);
                    var response = await httpClient.GetAsync(uri);
                    string apiResponse2 = await response.Content.ReadAsStringAsync();

                    var listaFiltrada = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse2);
                    return View(listaFiltrada);
                }
            }
            catch (Exception ex)
            {
                return View(ex);
            }

        }

        public async Task<ActionResult> Historico(int idInventario)
        {
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["idInventario"] = idInventario.ToString()
                };
                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/Historico", query);
                var response = await httpClient.GetAsync(uri);

                string apiResponse = await response.Content.ReadAsStringAsync();
                var historico = JsonConvert.DeserializeObject<List<Historico>>(apiResponse);

                return View(historico);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public async Task<ActionResult> Details(int id)
        {
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["id"] = id.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/Detalles", query);
                var response = await httpClient.GetAsync(uri);


                string apiResponse = await response.Content.ReadAsStringAsync();
                var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);

                return View(inventario);
            }
            catch (Exception ex)
            {
                return View(ex.Message);
            }
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Inventario inventario)
        {
            try
            {
                string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                var query = new Dictionary<string, string>()
                {
                    ["userName"] = userName
                };
                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/Agregar", query);

                var jsonContent = JsonConvert.SerializeObject(inventario);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> Edit(int id)
        {
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["id"] = id.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/Detalles", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);

                return View(inventario);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Inventario inventario)
        {
            try
            {
                int idInventario = inventario.id;

                var query = new Dictionary<string, string>()
                {
                    ["idInventario"] = idInventario.ToString()
                };

                var uri2 = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeInventario/Detalles", query);
                var response = await httpClient.GetAsync(uri2);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var inventarioBuscado = JsonConvert.DeserializeObject<Inventario>(apiResponse);

                string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                Model.ModeloInventario inventarioEditado = new ModeloInventario (); 
                inventarioEditado.id = inventario.id;
                inventarioEditado.Nombre = inventario.Nombre;
                inventarioEditado.Categoria = inventario.Categoria;
                inventarioEditado.Precio = inventario.Precio;
                inventarioEditado.Cantidad = inventarioBuscado.Cantidad;


                inventarioEditado.UserName = userName;
                
                string uri = "https://apicomercio.azurewebsites.net/api/ServicioDeInventario/EditarInventario";

                var jsonContent = JsonConvert.SerializeObject(inventarioEditado);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                using (var httpClient = new HttpClient())
                {
                    var response2 = await httpClient.PutAsync(uri, content);
                    response2.EnsureSuccessStatusCode();
                }

                return RedirectToAction(nameof(Index));
           }
            catch (HttpRequestException ex)
            {
                ViewBag.ErrorMessage = $"Error al actualizar el inventario: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                ViewBag.ErrorMessage = $"Ocurrió un error inesperado: {ex.Message}";
                return View();
            }
        }
    }
    }
