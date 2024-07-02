using GestionDeTiendaParte1.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace GestionDeTiendaParte1.UI.Controllers
{
    [Authorize]
    public class InventarioController : Controller
    {
        private BL.IAdministradorDeInventarios ElAdministrador;

        public InventarioController(BL.IAdministradorDeInventarios administrador)
        {
            ElAdministrador = administrador;
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
                listaDeInventarioFiltrada = lista.Where(x => x. Nombre.Contains(nombre)).ToList();
                return View(listaDeInventarioFiltrada);
            }
        }

        public ActionResult Historico()
        {
            var historico = ElAdministrador.ObtengaHistorico();

            return View(historico);
        }


        // GET: AjusteDeInventariosController/Details/5
        public ActionResult Details(int id)
        {
            Model.Inventario inventario;

            inventario = ElAdministrador.ObtengaElInventario(id);

            return View(inventario);
        }

        // GET: AjusteDeInventariosController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: AjusteDeInventariosController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.Inventario inventario)
        {
            try
            {
              
                string UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                
                
                ElAdministrador.Agregue(inventario, UserName);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        // GET: AjusteDeInventariosController/Edit/5
        public ActionResult Edit(int id)
        {
            Model.Inventario inventario;
            inventario = ElAdministrador.ObtengaElInventario(id);

            return View(inventario);
        }

        // POST: AjusteDeInventariosController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(Model.Inventario inventario)
        {
            try
            {
                string UserName = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;
                ElAdministrador.Edite(inventario, UserName);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

    }
}
