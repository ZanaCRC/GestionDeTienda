using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace GestionDeTiendaParte2.SI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicioDeCajasController : ControllerBase
    {
        private readonly BL.IAdministradorDeCajas elAdministradorDeCajas;

        public ServicioDeCajasController(BL.IAdministradorDeCajas elAdministradorDeCajas)
        {
            this.elAdministradorDeCajas = elAdministradorDeCajas;
        }

        [HttpPost("AbrirCaja")]
        public IActionResult AbrirCaja(int userID)
        {
            elAdministradorDeCajas.AbraUnaCaja(userID);
            return Ok();
        }

        [HttpPost("CerrarCaja")]
        public IActionResult CerrarCaja(int userID)
        {
            elAdministradorDeCajas.CierreUnaCaja(userID);
            return Ok();
        }

        [HttpGet("CajaActiva")]
        public ActionResult<AperturaDeCaja> CajaActiva(int userID)
        {
            var cajaAbierta = elAdministradorDeCajas.BusqueUnaCajaActiva(userID);
            if (cajaAbierta == null)
            {
                return NotFound();
            }
            return cajaAbierta;
        }

        [HttpGet("InformacionCaja")]
        public ActionResult<InformacionCaja> InformacionCaja(int idCaja)
        {
            var informacionCaja = elAdministradorDeCajas.RealiceLosCalculosDeLaCaja(idCaja);
            if (informacionCaja == null)
            {
                return NotFound();
            }
            return informacionCaja;
        }

        [HttpPost("RegistrarCaja")]
        public ActionResult<AperturaDeCaja> RegistrarCaja(int userID)
        {
            elAdministradorDeCajas.RegistreUnaCaja(userID);
            var nuevaCaja = elAdministradorDeCajas.BusqueUnaCajaNueva(1);
            return Ok(nuevaCaja); // Use Ok to return a 200 status code with the result
        }
    }
}
