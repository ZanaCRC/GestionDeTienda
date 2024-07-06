 

using GestionDeTiendaParte2.Model;

namespace GestionDeTiendaParte2.BL
{
    public class AdministradorDeUsuarios : IAdministradorDeUsuarios
    {
        private DA.DBContexto ElContextoBD;

        public AdministradorDeUsuarios(DA.DBContexto elContexto)
        {
            ElContextoBD = elContexto;
        }

        public Model.Usuario ObtenerUsuarioPorNombre(string nombre)
        {
            return ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == nombre);
        }

        public List<Model.Usuario> ObtenerTodosLosUsuarios()
        {
            var todosLosUsuarios = ElContextoBD.Usuarios.ToList(); 
            var usuariosFiltrados = todosLosUsuarios.Where(u => u.Rol == Model.Rol.Restringido).ToList(); 
            return usuariosFiltrados;
        }
        public void DePermisos(int id)
        {
            var usuario = ElContextoBD.Usuarios.Find(id);
            if (usuario != null)
            {
                usuario.Rol = Model.Rol.ConPermiso; 
                ElContextoBD.Usuarios.Update(usuario);
                ElContextoBD.SaveChanges();
            }
        }


        public Usuario IniciarSesion(string username, string clave)
        {
            Usuario? usuario = ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == username);

            if (usuario != null)
            {
                if (usuario.EstaBloqueado && usuario.FechaBloqueo.HasValue && usuario.FechaBloqueo.Value.AddMinutes(10) > DateTime.Now||usuario.Rol==Model.Rol.Restringido)
                {
                    return null; 
                }
                else if (usuario.Clave == clave)
                {
                    usuario.IntentosFallidos = 0; 
                    usuario.EstaBloqueado = false;
                    usuario.FechaBloqueo = null;
                    ElContextoBD.SaveChanges();
                    return usuario;
                }
                else
                {
                    usuario.IntentosFallidos += 1;
                    if (usuario.IntentosFallidos >= 3)
                    {
                        usuario.EstaBloqueado = true;
                        usuario.FechaBloqueo = DateTime.Now;
                    }
                    ElContextoBD.SaveChanges();
                    return null; 
                }
            }

            return null;
        }

        public bool CambiarClave(string elUsername, string laNuevaClave)
        {
            var usuario = ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == elUsername);

            if (usuario != null && !usuario.EsExterno)
            {
                usuario.Clave = laNuevaClave;
                ElContextoBD.SaveChanges();
                return true;
            }

            return false;
        }

        public string RegistrarUsuario(string nombre, string correoElectronico, string clave)
        {
            if (ElContextoBD.Usuarios.Any(u => u.Nombre == nombre))
            {
                return "NombreExistente";
            }
            if (ElContextoBD.Usuarios.Any(u => u.CorreoElectronico == correoElectronico))
            {
                return "CorreoExistente";
            }

            Model.Usuario elUsuarioGrande = new Model.Usuario
            {
                Nombre = nombre,
                CorreoElectronico = correoElectronico,
                Clave = clave,
                Rol = Model.Rol.Restringido,
                IntentosFallidos = 0,
                EstaBloqueado = false,
                FechaBloqueo = null,
                EsExterno = false
            };

            ElContextoBD.Usuarios.Add(elUsuarioGrande);
            ElContextoBD.SaveChanges();

            return "Exito";
        }

        public Usuario GuardarOActualizarUsuarioExterno(string nombre, string correo)
        {
            var usuario = ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == nombre && u.CorreoElectronico == correo);

            if (usuario == null)
            {
                usuario = new Usuario
                {
                    Nombre = nombre,
                    Rol = Rol.Restringido,   
                    CorreoElectronico = correo,
                    Clave = "",
                    IntentosFallidos = 0,
                    EstaBloqueado = false,
                    FechaBloqueo = null,
                    EsExterno = true
                };
                ElContextoBD.Usuarios.Add(usuario);
            }
            else
            {
                if (usuario.Nombre != nombre)
                {
                    usuario.Nombre = nombre;
                }
            }

            ElContextoBD.SaveChanges();
            return usuario;
        }





    }
}
