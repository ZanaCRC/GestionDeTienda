using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        private readonly HttpClient httpClient;
        public int userID { get; set; }

        public VentasController()
        {
            httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index()
        {

            var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (userIdClaim == null)
            {
                ViewBag.ErrorMessage = "No se encontró el UserId en las Claims del usuario.";
                return View();
            }

            var userID = int.Parse(userIdClaim.Value);

            List<Model.Venta> listaDeVentas;

            var httpClient = new HttpClient();
            try
            {

                var query = new Dictionary<string, string>()
                {
                    ["userID"] = userID.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaVentas", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                listaDeVentas = JsonConvert.DeserializeObject<List<Model.Venta>>(apiResponse);

                return View(listaDeVentas);
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public async Task<ActionResult> AgregarProductosALaVenta(int idVenta)
        {
            TempData["IdGuardado"] = idVenta;


            var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaTodoElInventario";
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<Model.ModeloInventario> listaDeProductosDelInventario = JsonConvert.DeserializeObject<List<Model.ModeloInventario>>(apiResponse);
                return View(listaDeProductosDelInventario);
            }
            else
            {
                ViewBag.ErrorMessage = "Error al obtener la lista de inventario desde el servidor.";
                return View();
            }

        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarProductosALaVenta(List<ModeloInventario> productosSeleccionados)
        {
            if (productosSeleccionados != null)
            {
                int IdVenta = (int)TempData["IdGuardado"];

                var modeloAgregar = new ModeloAgregarInventarioALaVenta
                {
                    idVenta = IdVenta,
                    productosSeleccionados = productosSeleccionados
                };

                var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeVentas/AgregueProductos";
                var jsonContent = JsonConvert.SerializeObject(modeloAgregar);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction("Index", "Ventas");
            }
            else
            {
                return RedirectToAction("Index", "Ventas");
            }
        }



        public async Task<ActionResult> EditarProductosDeVenta(int idVenta)
        {
            TempData["IdGuardado"] = idVenta;

            try
            {

                var query = new Dictionary<string, string>()
                {
                    ["idVenta"] = idVenta.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaListaProductosDeVenta", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
               
                var listaDeProductosDelInventario = JsonConvert.DeserializeObject<List<ModeloInventario>>(apiResponse);

                return View(listaDeProductosDelInventario);
            }
            catch (Exception ex)
            {
                return View();
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> EditarProductosDeVenta(List<int> productosSeleccionados)
        {
            if (productosSeleccionados != null)
            {
                int IdVenta = (int)TempData["IdGuardado"];
                var uri = $"https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ElimineProductos/{IdVenta}";
                var jsonContent = JsonConvert.SerializeObject(productosSeleccionados);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            else
            {
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<ActionResult> ListaProductosDeVenta(int idVenta)
        {
            TempData["IdGuardado"] = idVenta;

            try
            {

                var query = new Dictionary<string, string>()
                {
                    ["idVenta"] = idVenta.ToString()
                };

                var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaListaProductosDeVenta", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                var listaDeProductosDelInventario = JsonConvert.DeserializeObject<List<ModeloParaMostrarInventarioDeUnaVenta>>(apiResponse);

                return View(listaDeProductosDelInventario);
            }
            catch (Exception ex)
            {
                return View();
            }
  

            
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(Venta nuevaVenta)
        {
            try
            {
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim != null)
                {
                    var groupId = UserIdClaim.Value;
                    userID = int.Parse(groupId);
                }
                var modeloCrearVenta = new ModeloCrearVenta
                {
                    NombreCliente = nuevaVenta.NombreCliente,
                    UserID = userID
                };

                var jsonContent = JsonConvert.SerializeObject(modeloCrearVenta);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeVentas/Cree";

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }


        public async Task<ActionResult> AgregarDescuentoAVenta(int idVenta)
        {
            var query = new Dictionary<string, string>()
            {
                ["idVenta"] = idVenta.ToString()
            };

            var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaVenta", query);
            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
           
            var ventaAEditar = JsonConvert.DeserializeObject<Venta>(apiResponse);

            TempData["IdDeVentaAModificar"] = idVenta;
            return View(ventaAEditar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> AgregarDescuentoAVenta(Venta ventaModificada)
        {
            try
            {
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim != null)
                {
                    var groupId = UserIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                var modelo = new ModeloAgregarDescuento
                {
                   IdVenta = (int)TempData["IdDeVentaAModificar"],
                   Descuento = ventaModificada.PorcentajeDesCuento
                };

                var jsonContent = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeVentas/AgregueDescuento";

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }





         
        }

        public async Task<ActionResult> TerminarVenta(int idVenta)
        {
            var query = new Dictionary<string, string>()
            {
                ["idVenta"] = idVenta.ToString()
            };

            var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeVentas/ObtengaVenta", query);
            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();

            var ventaAEditar = JsonConvert.DeserializeObject<Venta>(apiResponse);

            TempData["IdDeVentaAModificar"] = idVenta;
            return View(ventaAEditar);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> TerminarVenta(Venta ventaModificada)
        {
            try
            {
                
                int idVentaAModificar = (int)TempData["IdDeVentaAModificar"];


                var modelo = new ModeloParaFinalizarVenta
                {
                    IdVenta = (int)TempData["IdDeVentaAModificar"],
                    MetodoDePago = ventaModificada.MetodoDePago,
                };

                var jsonContent = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeVentas/TermineVenta";

                var response = await httpClient.PostAsync(uri, content);
                response.EnsureSuccessStatusCode();



                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
