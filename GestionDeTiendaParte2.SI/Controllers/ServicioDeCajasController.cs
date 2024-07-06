using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Mvc;


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

        [HttpPost("AbraUnaCaja")]
        public IActionResult AbraUnaCaja(int userID)
        {
            elAdministradorDeCajas.AbraUnaCaja(userID);
            return Ok();
        }

        [HttpPost("CierreUnaCaja")]
        public IActionResult CierreUnaCaja(int userID)
        {
            elAdministradorDeCajas.CierreUnaCaja(userID);
            return Ok();
        }

        [HttpGet("LaCajaEstaActiva")]
        public ActionResult<AperturaDeCaja> LaCajaEstaActiva(int userID)
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

        [HttpPost("RegistreUnaCaja")]
        public ActionResult<AperturaDeCaja> RegistreUnaCaja(int userID)
        {
            elAdministradorDeCajas.RegistreUnaCaja(userID);
            var nuevaCaja = elAdministradorDeCajas.BusqueUnaCajaNueva(1);
            return Ok(nuevaCaja); 
        }
    }
}
