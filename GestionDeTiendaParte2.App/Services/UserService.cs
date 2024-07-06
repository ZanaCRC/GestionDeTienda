using GestionDeTiendaParte2.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
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
        
    }
}
