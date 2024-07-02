using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public interface IAdministradorDeCorreos
    {
      Task SendEmailAsync(string email, string subject, string message);
    }
}
