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
                var userIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (userIdClaim != null)
                {
                    var groupId = userIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                using (var httpClient = new HttpClient())
                {
                    if (accion == "cerrar")
                    {
                        var cerrarResponse = await httpClient.PostAsync($"https://apicomercio.azurewebsites.net/api/ServicioDeCajas/CierreUnaCaja?userID={userID}", null);
                        cerrarResponse.EnsureSuccessStatusCode();
                    }
                    else if (accion == "abrir")
                    {
                        var abrirResponse = await httpClient.PostAsync($"https://apicomercio.azurewebsites.net/api/ServicioDeCajas/AbraUnaCaja?userID={userID}", null);
                        abrirResponse.EnsureSuccessStatusCode();
                    }

                    var response = await httpClient.GetAsync($"https://apicomercio.azurewebsites.net/api/ServicioDeCajas/LaCajaEstaActiva?userID={userID}");
                    response.EnsureSuccessStatusCode();
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    var cajaAbierta = JsonConvert.DeserializeObject<AperturaDeCaja>(apiResponse);

                    InformacionCaja informacionRelacionadaALaCajaPorMostrar = new InformacionCaja();

                    if (cajaAbierta != null)
                    {
                        var informacionCajaResponse = await httpClient.GetAsync($"https://apicomercio.azurewebsites.net/api/ServicioDeCajas/InformacionCaja?idCaja={cajaAbierta.Id}");
                        string informacionApiResponse = await informacionCajaResponse.Content.ReadAsStringAsync();
                        informacionRelacionadaALaCajaPorMostrar = JsonConvert.DeserializeObject<InformacionCaja>(informacionApiResponse);
                        informacionRelacionadaALaCajaPorMostrar.Caja = cajaAbierta;
                    }
                    else
                    {
                        string postUrl = $"https://apicomercio.azurewebsites.net/api/ServicioDeCajas/RegistreCaja?userID={userID}";
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
                return View();
            }
        }

    }
}
