using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.App.Interfaces
{
    public interface IUserService
    {
        public Task<Usuario> IniciarSesion(string nombre, string clave);
    }
}
