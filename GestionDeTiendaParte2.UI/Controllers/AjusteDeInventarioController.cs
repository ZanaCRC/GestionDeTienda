using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class AjusteDeInventarioController : Controller
    {
        private BL.IAdministradorDeAjustesDeInventarios ElAdministrador;
        private BL.IAdministradorDeUsuarios ElAdministradorDeUsuarios;
        int userID = 0;
        public AjusteDeInventarioController(BL.IAdministradorDeAjustesDeInventarios administrador, BL.IAdministradorDeUsuarios elAdministradorDeUsuarios)
        {
            ElAdministrador = administrador;
            ElAdministradorDeUsuarios = elAdministradorDeUsuarios;
        }

        public ActionResult Index(string nombre)
        {

            List<Model.Inventario> lista;

            lista = ElAdministrador.ObtengaLaLista();

            if (nombre is null)
                return View(lista);
            else
            {
                List<Model.Inventario> listaDeInventarioFiltrada;
                listaDeInventarioFiltrada = lista.Where(x => x.Nombre.Contains(nombre)).ToList();
                return View(listaDeInventarioFiltrada);
            }
            return View(lista);
        }

        public ActionResult ListaDeAjustes(int ajuste)
        {

            List<Model.AjusteDeInventario> lista;

            lista = ElAdministrador.ObtengaListaDeAjustes(ajuste);


            return View(lista);
        }

        public ActionResult DetalleDelAjuste(int detalleAjuste)

        {
            List<Model.AjusteDeInventario> lista;

            lista = ElAdministrador.ObtengaListaDeAjustesParaDetalle(detalleAjuste);

            return View(lista);
        }
        public ActionResult AgregarAjuste(int idInventario)
        {
            TempData["IdDeInventario"] = idInventario;
            var inventario = ElAdministrador.ObtenerInventarioPorId(idInventario);
            ViewBag.CantidadActual = inventario.Cantidad;
            var ajuste = new GestionDeTiendaParte2.Model.AjusteDeInventario();
            return View(ajuste);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarAjuste(Model.AjusteDeInventario nuevoAjuste)
        {
           
            try
            {
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim != null)
                {
                    var groupId = UserIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                nuevoAjuste.UserId = userID;

                int idInventario;
                idInventario = int.Parse(TempData["IdDeInventario"].ToString());
                nuevoAjuste.Id_Inventario = idInventario;

                if (ElAdministrador.AgregueAjuste(nuevoAjuste))
                {
                    return RedirectToAction(nameof(Index));
                }
                else
                {
                    ModelState.AddModelError("", "Ocurrió un error, por favor intente de nuevo.");
                   
                    var inventario = ElAdministrador.ObtenerInventarioPorId(idInventario);
                    
                    nuevoAjuste.CantidadActual = inventario.Cantidad;
                    ViewBag.CantidadActual = nuevoAjuste.CantidadActual;
                    return View(nuevoAjuste);
                }
                
            }
            catch
            {
                return View();
            }
        }
    }
}
