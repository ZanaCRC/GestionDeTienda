using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class CajasController : Controller
    {
        private readonly HttpClient httpClient;
        int userID = 0;

        public CajasController()
        {
            httpClient = new HttpClient();
        }

        public async Task<ActionResult> Index(string accion)
        {
            try
            {
                // Obtén el userID del claim
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                {
                    var groupId = userIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                using (var httpClient = new HttpClient())
                {
                    // Manejar las acciones de abrir o cerrar caja
                    if (accion == "cerrar")
                    {
                        var cerrarResponse = await httpClient.PostAsync($"https://localhost:7001/api/ServicioDeCajas/CerrarCaja?userID={userID}", null);
                        cerrarResponse.EnsureSuccessStatusCode();
                    }
                    else if (accion == "abrir")
                    {
                        var abrirResponse = await httpClient.PostAsync($"https://localhost:7001/api/ServicioDeCajas/AbrirCaja?userID={userID}", null);
                        abrirResponse.EnsureSuccessStatusCode();
                    }

                    // Obtener la caja activa
                    var response = await httpClient.GetAsync($"https://localhost:7001/api/ServicioDeCajas/CajaActiva?userID={userID}");
                    response.EnsureSuccessStatusCode();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var cajaAbierta = JsonConvert.DeserializeObject<AperturaDeCaja>(apiResponse);

                    InformacionCaja informacionRelacionadaALaCajaPorMostrar = new InformacionCaja();

                    if (cajaAbierta != null)
                    {
                        // Obtener la información de la caja
                        var informacionCajaResponse = await httpClient.GetAsync($"https://localhost:7001/api/ServicioDeCajas/InformacionCaja?idCaja={cajaAbierta.Id}");
                        string informacionApiResponse = await informacionCajaResponse.Content.ReadAsStringAsync();
                        informacionRelacionadaALaCajaPorMostrar = JsonConvert.DeserializeObject<InformacionCaja>(informacionApiResponse);
                        informacionRelacionadaALaCajaPorMostrar.Caja = cajaAbierta;
                    }
                    else
                    {
                        // Registrar una nueva caja si no hay una abierta
                        string postUrl = $"https://localhost:7001/api/ServicioDeCajas/RegistrarCaja?userID={userID}";
                        var registrarResponse = await httpClient.PostAsync(postUrl, null);

                        if (registrarResponse.StatusCode == System.Net.HttpStatusCode.NotFound)
                        {
                            throw new Exception($"Endpoint not found: {postUrl}");
                        }

                        registrarResponse.EnsureSuccessStatusCode();
                        string nuevaCajaApiResponse = await registrarResponse.Content.ReadAsStringAsync();
                        informacionRelacionadaALaCajaPorMostrar.Caja = JsonConvert.DeserializeObject<AperturaDeCaja>(nuevaCajaApiResponse);
                    }

                    return View(informacionRelacionadaALaCajaPorMostrar);
                }
            }
            catch (Exception ex)
            {
                // Manejo de errores
                return View();
            }
        }

    }
}
