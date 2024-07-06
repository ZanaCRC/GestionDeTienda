using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class AjusteDeInventarioController : Controller
    {
        private readonly BL.IAdministradorDeAjustesDeInventarios ElAdministradorDeInventarios;
        private readonly BL.IAdministradorDeUsuarios ElAdministradorDeUsuarios;
        private readonly HttpClient httpClient;
        int userID = 0;

        public AjusteDeInventarioController(BL.IAdministradorDeAjustesDeInventarios administrador,
                                            BL.IAdministradorDeUsuarios elAdministradorDeUsuarios)
        {
            ElAdministradorDeInventarios = administrador;
            ElAdministradorDeUsuarios = elAdministradorDeUsuarios;
            httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(string nombre)
        {
            List<Inventario> lista;
            try
            {
                var respuesta = await httpClient.GetAsync("https://localhost:7001/api/ServicioDeAjusteDeInventario/Liste");
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Inventario>>(apiResponse);

                if (string.IsNullOrEmpty(nombre))
                {
                    return View(lista);
                }
                else
                {
                    var filteredList = lista.Where(x => x.Nombre.Contains(nombre)).ToList();
                    return View(filteredList);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> ListaDeAjustes(int ajuste)
        {
            List<ModeloAjusteDeInventario> lista;
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["ajuste"] = ajuste.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeAjusteDeInventario/ObtengaListaDeAjustes", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<ModeloAjusteDeInventario>>(apiResponse);

                return View(lista);
            }
            catch (Exception ex)
            { 
                return View();
            }
        }

        public async Task<ActionResult> DetalleDelAjuste(int detalleAjuste)
        {
            List<ModeloAjusteDeInventario> lista;
            try
            {
                var query = new Dictionary<string, string>()
                {
                    ["detalleAjuste"] = detalleAjuste.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeAjusteDeInventario/ObtengaListaDeAjustesParaDetalle", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<ModeloAjusteDeInventario>>(apiResponse);

                return View(lista);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        public async Task<ActionResult> AgregarAjuste(int idInventario)
        {
            try
            {
                TempData["IdDeInventario"] = idInventario;
                var query = new Dictionary<string, string>()
                {
                    ["idInventario"] = idInventario.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeAjusteDeInventario/ObtengaInventarioPorId", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);
                ViewBag.CantidadActual = inventario.Cantidad;

                var ajuste = new AjusteDeInventario();
                return View(ajuste);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarAjuste(AjusteDeInventario nuevoAjuste)
        {
            try
            {
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                {
                    var groupId = userIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                nuevoAjuste.UserId = userID;
                int idInventario = int.Parse(TempData["IdDeInventario"].ToString());
                nuevoAjuste.Id_Inventario = idInventario;



                var modelo = new ModeloAgregarAjuste
                {
                    Id = nuevoAjuste.Id,
                    Id_Inventario = nuevoAjuste.Id_Inventario,
                    CantidadActual = nuevoAjuste.CantidadActual,
                    Ajuste = nuevoAjuste.Ajuste,
                    Tipo = nuevoAjuste.Tipo,
                    Observaciones = nuevoAjuste.Observaciones,
                    Fecha = nuevoAjuste.Fecha,
                    UserId = nuevoAjuste.UserId
                };






                string json = JsonConvert.SerializeObject(modelo);
                var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                var byteContent = new ByteArrayContent(buffer);
                byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                var response = await httpClient.PostAsync("https://localhost:7001/api/ServicioDeAjusteDeInventario/AgregueAjuste", byteContent);

                if (response.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error, por favor intente de nuevo.");
                    var inventario = await ObtenerInventarioPorId(idInventario);
                    nuevoAjuste.CantidadActual = inventario.Cantidad;
                    ViewBag.CantidadActual = nuevoAjuste.CantidadActual;
                    return View(nuevoAjuste);
                }
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        private async Task<Inventario> ObtenerInventarioPorId(int idInventario)
        {
            var query = new Dictionary<string, string>()
            {
                ["idInventario"] = idInventario.ToString()
            };

            var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeAjusteDeInventario/ObtengaInventarioPorId", query);
            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);
            return inventario;
        }
    }
}
