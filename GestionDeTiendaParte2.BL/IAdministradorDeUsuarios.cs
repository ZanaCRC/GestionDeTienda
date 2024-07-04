using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace GestionDeTiendaParte2.BL
{
    public interface IAdministradorDeUsuarios
    {
        public string RegistrarUsuario(string nombre, string correoElectronico, string clave);
        public Usuario IniciarSesion(string correoElectronico, string clave);
        public Usuario ObtenerUsuarioPorNombre(string nombre);
        public Usuario GuardarOActualizarUsuarioExterno(string nombre, string correo);
        public bool CambiarClave(string elUsername, string laNuevaClave);
    }
}
