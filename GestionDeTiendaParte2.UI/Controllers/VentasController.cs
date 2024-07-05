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

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeVentas/ObtenerVentas", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                //error 500
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


            var uri = "https://localhost:7001/api/ServicioDeVentas/ObtenerTodoElInventario";
            var response = await httpClient.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                List<Model.ModeloInventario> listaDeProductosDelInventario = JsonConvert.DeserializeObject<List<Model.ModeloInventario>>(apiResponse);
                return View(listaDeProductosDelInventario);
            }
            else
            {
                // Manejar el caso cuando la solicitud no fue exitosa (por ejemplo, error 500)
                // Puedes retornar una vista con un mensaje de error o manejarlo de acuerdo a tus necesidades.
                // Por ejemplo:
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
                // Obtener el Id de Venta desde TempData
                int IdVenta = (int)TempData["IdGuardado"];

                // Crear el objeto ModeloAgregarInventarioALaVenta
                var modeloAgregar = new ModeloAgregarInventarioALaVenta
                {
                    idVenta = IdVenta,
                    productosSeleccionados = productosSeleccionados
                };

                // Preparar los datos para enviar a la API
                var uri = "https://localhost:7001/api/ServicioDeVentas/AgregarProductos";
                var jsonContent = JsonConvert.SerializeObject(modeloAgregar);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Enviar la solicitud POST a la API
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

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeVentas/ObtenerListaProductosDeVenta", query);
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
                var uri = $"https://localhost:7001/api/ServicioDeVentas/EliminarProductos/{IdVenta}";
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

                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeVentas/ObtenerListaProductosDeVenta", query);
                var response = await httpClient.GetAsync(uri);
                string apiResponse = await response.Content.ReadAsStringAsync();
                //error 500
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

                // Crear el objeto ModeloCrearVenta
                var modeloCrearVenta = new ModeloCrearVenta
                {
                    NombreCliente = nuevaVenta.NombreCliente, // Asumimos que Venta tiene una propiedad NombreCliente
                    UserID = userID
                };

                // Convertir el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(modeloCrearVenta);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Construir la URI para la API
                var uri = "https://localhost:7001/api/ServicioDeVentas/Crear";

                // Enviar el objeto a la API
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

            var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeVentas/ObtenerVenta", query);
            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            //error 500
           
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

                // Crear el objeto ModeloCrearVenta
                var modelo = new ModeloAgregarDescuento
                {
                   IdVenta = (int)TempData["IdDeVentaAModificar"],
                   Descuento = ventaModificada.PorcentajeDesCuento
                };

                // Convertir el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Construir la URI para la API
                var uri = "https://localhost:7001/api/ServicioDeVentas/AgregarDescuento";

                // Enviar el objeto a la API
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

            var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeVentas/ObtenerVenta", query);
            var response = await httpClient.GetAsync(uri);
            string apiResponse = await response.Content.ReadAsStringAsync();
            //error 500

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



                // Crear el objeto ModeloCrearVenta
                var modelo = new ModeloParaFinalizarVenta
                {
                    IdVenta = (int)TempData["IdDeVentaAModificar"],
                    MetodoDePago = ventaModificada.MetodoDePago,
                };

                // Convertir el objeto a JSON
                var jsonContent = JsonConvert.SerializeObject(modelo);
                var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

                // Construir la URI para la API
                var uri = "https://localhost:7001/api/ServicioDeVentas/TerminarVenta";

                // Enviar el objeto a la API
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
