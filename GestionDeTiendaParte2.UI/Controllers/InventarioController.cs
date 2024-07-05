﻿using GestionDeTiendaParte2.Model;
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

                var respuesta = await httpClient.GetAsync("https://localhost:7001/api/ServicioDeInventario/ObtenerLista");
                string apiResponse = await respuesta.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse);

                if (nombre is null)
                    return View(lista);
                else
                {

                    var httpClient2 = new HttpClient();
                    string json = JsonConvert.SerializeObject(lista);
                    var buffer = System.Text.Encoding.UTF8.GetBytes(json);
                    var byteContent = new ByteArrayContent(buffer);
                    byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");

                    var uri = $"https://localhost:7195/api/ServicioDeInventario/FiltreLaLista?nombre={nombre}";


                    var response = await httpClient.PostAsync(uri, byteContent);

                    response.EnsureSuccessStatusCode();

                    string apiResponse2 = await response.Content.ReadAsStringAsync();
                    var listaFiltrada = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse2);
                    return View(listaFiltrada);
                }
            }
            catch (Exception ex)
            {
                return View();
            }



        }

        public async Task<ActionResult> Historico()
        {
            var response = await httpClient.GetAsync("https://localhost:7001/api/ServicioDeInventario/Historico");
            response.EnsureSuccessStatusCode();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var historico = JsonConvert.DeserializeObject<List<Historico>>(apiResponse);

            return View(historico);
        }

        public async Task<ActionResult> Details(int id)
        {
            var response = await httpClient.GetAsync($"https://localhost:7001/api/ServicioDeInventario/Detalles/{id}");
            response.EnsureSuccessStatusCode();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);

            return View(inventario);
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
                var uri = QueryHelpers.AddQueryString("https://localhost:7001/api/ServicioDeInventario/Agregar", query);

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
            var response = await httpClient.GetAsync($"https://localhost:7001/api/ServicioDeInventario/Detalles/{id}");
            response.EnsureSuccessStatusCode();
            string apiResponse = await response.Content.ReadAsStringAsync();
            var inventario = JsonConvert.DeserializeObject<Inventario>(apiResponse);

            return View(inventario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Edit(Inventario inventario)
        {
            try
            {
                // Obtener el id del inventario
                int idInventario = inventario.id;

                // Obtener el inventario actual desde la API
                var response = await httpClient.GetAsync($"https://localhost:7001/api/ServicioDeInventario/Detalles/{idInventario}");
                response.EnsureSuccessStatusCode();
                string apiResponse = await response.Content.ReadAsStringAsync();
                var inventarioBuscado = JsonConvert.DeserializeObject<Inventario>(apiResponse);

                // Obtener el nombre de usuario actual
                string userName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;





                Model.ModeloInventario inventarioEditado = new ModeloInventario (); 
                inventarioEditado.id = inventario.id;
                inventarioEditado.Nombre = inventario.Nombre;
                inventarioEditado.Categoria = inventario.Categoria;
                inventarioEditado.Precio = inventario.Precio;
                inventarioEditado.Cantidad = inventarioBuscado.Cantidad;


                // Actualizar los datos del inventario buscado
                inventarioEditado.UserName = userName;
                
                // Construir la URL del endpoint de la API
                string uri = "https://localhost:7001/api/ServicioDeInventario/EditarInventario";

                // Serializar el inventario actualizado y enviarlo al servidor
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
                // Captura y maneja errores HTTP (como 400, 500, etc.) que podrían surgir al llamar a la API
                ViewBag.ErrorMessage = $"Error al actualizar el inventario: {ex.Message}";
                return View();
            }
            catch (Exception ex)
            {
                // Captura cualquier otra excepción inesperada
                ViewBag.ErrorMessage = $"Ocurrió un error inesperado: {ex.Message}";
                return View();
            }
        }
    }
    }
