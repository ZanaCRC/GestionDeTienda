using GestionDeTiendaParte2.App.Interfaces;
using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.WebUtilities;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.App.Services
{
    public class UserService : IUserService
    {
        HttpClient httpClient;
        string uriApi = "https://webappjunio2024.azurewebsites.net/api/user/";
        public UserService()
        {
            httpClient = new HttpClient();
        }
        public async Task<Usuario> IniciarSesion(string nombre, string clave)
        {
            var queryParams = new Dictionary<string, string>
            {
                { "nombre", nombre },
                { "clave", clave }
            };

            var uri = QueryHelpers.AddQueryString("https://apicomercio.azurewebsites.net/api/ServicioDeLogin/IniciarSesion", queryParams);

            var response = await httpClient.GetAsync(uri);
            var apiResponse = await response.Content.ReadAsStringAsync();
            var elUsuario = JsonConvert.DeserializeObject<Usuario>(apiResponse);

            if (response.IsSuccessStatusCode && elUsuario != null && !elUsuario.EsExterno)
            {
                return elUsuario;
            }
            else
            {
                return null;
            }
        }

        public async Task<List<Model.Inventario>> ObtenerLista()
        {
            List<Model.Inventario> lista = new List<Model.Inventario>();

            var uri = "https://apicomercio.azurewebsites.net/api/ServicioDeInventario/ObtenerLista";

            var response = await httpClient.GetAsync(uri);
            if (response.IsSuccessStatusCode)
            {
                string apiResponse = await response.Content.ReadAsStringAsync();
                lista = JsonConvert.DeserializeObject<List<Model.Inventario>>(apiResponse);
            }

            return lista;
        }
    }
}

