 

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
    public Usuario IniciarSesion(string username, string clave)
        {
            Usuario? usuario = ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == username);

            if (usuario != null)
            {
                if (usuario.EstaBloqueado && usuario.FechaBloqueo.HasValue && usuario.FechaBloqueo.Value.AddMinutes(10) > DateTime.Now)
                {
                    // Usuario bloqueado y aún no ha pasado el tiempo de bloqueo
                    return null; // O manejar de otra manera específica
                }
                else if (usuario.Clave == clave)
                {
                    usuario.IntentosFallidos = 0; // Reiniciar intentos fallidos
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
                    return null; // O manejar de otra manera específica
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
                Rol = Model.Rol.Normal,
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
            // Buscar un usuario por su proveedor y proveedorUserId
            var usuario = ElContextoBD.Usuarios.FirstOrDefault(u => u.Nombre == nombre && u.CorreoElectronico == correo);

            if (usuario == null)
            {
                // Si no existe, crear uno nuevo y guardarlo en la base de datos
                usuario = new Usuario
                {
                    Nombre = nombre,
                    Rol = Rol.Normal,   
                    CorreoElectronico = correo,
                    Clave = "",// Asumiendo que todos los usuarios externos inician con un rol normal
                    IntentosFallidos = 0,
                    EstaBloqueado = false,
                    FechaBloqueo = null,
                    EsExterno = true
                };
                ElContextoBD.Usuarios.Add(usuario);
            }
            else
            {
                // Si existe, actualizar su nombre si es necesario
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
