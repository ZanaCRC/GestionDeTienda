using GestionDeTiendaParte1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;

namespace GestionDeTiendaParte1.UI.Controllers
{
    [Authorize]
    public class CajasController : Controller
    {
        private BL.IAdministradorDeCajas ElAdministradorDeCajas;
        // GET: CajasController
        int userID = 0;
        public CajasController(BL.IAdministradorDeCajas elAdministrador)
        {
            ElAdministradorDeCajas = elAdministrador;
          
        }
        
        public ActionResult Index(string accion)
        {
            var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (UserIdClaim != null)
            {
                var groupId = UserIdClaim.Value;
                userID = int.Parse(groupId);
            }


            if (accion == "cerrar") {
                ElAdministradorDeCajas.CierreUnaCaja(userID);
               


            } else if (accion =="abrir") { 
            ElAdministradorDeCajas.AbraUnaCaja(userID);
            

            }

            Model.AperturaDeCaja CajaAbierta= ElAdministradorDeCajas.BusqueUnaCajaActiva(userID);
        
           Model.InformacionCaja informacionRelacionadaALaCajaPorMostrar = new Model.InformacionCaja();
            if (CajaAbierta != null)
            {
               
                informacionRelacionadaALaCajaPorMostrar = ElAdministradorDeCajas.RealiceLosCalculosDeLaCaja(CajaAbierta.Id);
                informacionRelacionadaALaCajaPorMostrar.Caja = CajaAbierta;
            }
            else { 
                ElAdministradorDeCajas.RegistreUnaCaja(userID);
                informacionRelacionadaALaCajaPorMostrar.Caja = ElAdministradorDeCajas.BusqueUnaCajaNueva(1);
            }
            
           


            return View(informacionRelacionadaALaCajaPorMostrar);
        }

     
       

     

      
    }
}
