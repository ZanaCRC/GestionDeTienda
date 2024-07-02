using System.ComponentModel.DataAnnotations;

namespace GestionDeTiendaParte1.Model
{
    public class Usuario
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El Nombre es requerido.")]
        public string Nombre { get; set; }
        [Required(ErrorMessage = "El Correo Electrónico es requerido.")]
        public string CorreoElectronico { get; set; }
        [Required(ErrorMessage = "La Clave es requerida.")]
        public string Clave { get; set; }
        public Rol Rol { get; set; }

        public bool EsExterno { get; set; }

        public ICollection<AjusteDeInventario> Ajustes { get; set; }
        public ICollection<AperturaDeCaja> Aperturas { get; set; }
        public int IntentosFallidos { get; set; } // Para contar los intentos de inicio de sesión fallidos
        public bool EstaBloqueado { get; set; } // Para indicar si el usuario está bloqueado
        public DateTime? FechaBloqueo { get; set; }
    }
}
