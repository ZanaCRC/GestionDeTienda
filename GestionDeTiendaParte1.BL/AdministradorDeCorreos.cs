using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using System.ComponentModel.DataAnnotations;

namespace GestionDeTiendaParte1.BL
{
    public class AdministradorDeCorreos : IAdministradorDeCorreos
    {
        private readonly IConfiguration _configuration;

        public AdministradorDeCorreos(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            string correo = _configuration.GetSection("Credenciales:Correo").Value;
            string contrasena = _configuration.GetSection("Credenciales:Clave").Value; 

            var elCliente = new SmtpClient("smtp.gmail.com")
            {
                Port = 587,
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(correo, contrasena)
            };

            var direccionDeCorreo = new MailAddress(correo);
            var mensaje = new MailMessage
            {
                From = direccionDeCorreo,
                Subject = subject,
                Body = message,
                IsBodyHtml = true
            };
            mensaje.To.Add(email);

            try
            {
                await elCliente.SendMailAsync(mensaje);
            }
            catch (Exception ex)
            {
                
                throw;
            }
        }

    }
}