using GestionDeTiendaParte2.Model;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace GestionDeTiendaParte2.UI.Controllers
{
    [Authorize]
    public class VentasController : Controller
    {
        public int userID { get; set; }

        private BL.IAdministradorDeCajas ElAdministradorDeCajas;

        private BL.IAdministradorDeVentas ElAdministradorDeVentas;
        // GET: VentasController

        public VentasController(BL.IAdministradorDeCajas administradorDeCajas, BL.IAdministradorDeVentas administradorDeVentas )
        {
          ElAdministradorDeCajas = administradorDeCajas;
           ElAdministradorDeVentas = administradorDeVentas;
            userID = 0;

        }
        public ActionResult Index()
        {
            var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
            if (UserIdClaim != null)
            {
                var groupId = UserIdClaim.Value;
                userID = int.Parse(groupId);
            }

            Model.AperturaDeCaja CajaAbierta = ElAdministradorDeCajas.BusqueUnaCajaActiva(userID);
            List<Venta> listaDeVentas = ElAdministradorDeVentas.BusqueVentasPorIdAperturaCaja(CajaAbierta.Id);


            return View(listaDeVentas);
        }

        public ActionResult AgregarProductosALaVenta(int idVenta)
        {

            bool hayError = false;
            if (TempData.TryGetValue("ErrorAgregarVenta", out var tempDataValue))
            {
                if (Boolean.TryParse(tempDataValue?.ToString(), out var parsedValue))
                {
                    hayError = parsedValue;
                }
            }


            TempData["IdGuardado"] = idVenta;
           
            List<Inventario> listaDeProductosDelInventario = ElAdministradorDeVentas.ObtenerTodosLosProductos();

            if (hayError) {
                ViewData["Error"] = "No hay stock suficiente de un producto";
            }

            return View(listaDeProductosDelInventario);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarProductosALaVenta(List<Inventario> productosSeleccionados)
        {
            if (productosSeleccionados != null)
            {
                int IdVenta = (int)TempData["IdGuardado"];

                foreach (var producto in productosSeleccionados)
                {
                    if (producto.IsSelected)
                    {
                        ElAdministradorDeVentas.AgregueVentaDetalle(IdVenta, producto);
                    }
                }

                ElAdministradorDeVentas.ActualiceMontosEnUnaVenta(IdVenta);
                return RedirectToAction("Index", "Ventas");
            }
            else
            {
                return RedirectToAction("Index", "Ventas");
            }
        }

       

    public ActionResult EditarProductosDeVenta(int idVenta)
        {

            TempData["IdGuardado"] = idVenta;
            List<Inventario> listaDeProductosDelInventario = ElAdministradorDeVentas.ObtenerInventariosPorVenta(idVenta);


            return View(listaDeProductosDelInventario);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditarProductosDeVenta(List<int> productosSeleccionados)
        {
            if (productosSeleccionados != null )
            { int IdVenta = (int)TempData["IdGuardado"];
                for (int i = 0; i < productosSeleccionados.Count; i++)
                {

                   
                    ElAdministradorDeVentas.ElimineVentaDetalle(IdVenta, productosSeleccionados[i]);
                   
                   



                }
                return RedirectToAction(nameof(Index));
            }
            else
            {
               
                return RedirectToAction(nameof(Index)); 
            }
        }
        public ActionResult ListaProductosDeVenta(int idVenta)
        {

            TempData["IdGuardado"] = idVenta;
            List<Inventario> listaDeProductosDelInventario = ElAdministradorDeVentas.ObtenerInventariosPorVenta(idVenta);


            return View(listaDeProductosDelInventario);
        }
        

        // GET: VentasController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: VentasController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(Model.Venta nuevaVenta)
        {
            try
            {
                var UserIdClaim = User.Claims.FirstOrDefault(c => c.Type == "UserId");
                if (UserIdClaim != null)
                {
                    var groupId = UserIdClaim.Value;
                    userID = int.Parse(groupId);
                }

                //Consigue el userID
                Model.AperturaDeCaja cajaAbierta = ElAdministradorDeCajas.BusqueUnaCajaActiva(userID);

                ElAdministradorDeVentas.AgregueUnaNuevaVenta(nuevaVenta, cajaAbierta);
               

                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: VentasController/Edit/5
        public ActionResult AgregarDescuentoAVenta(int idVenta)
        {
            Model.Venta ventaAEditar = ElAdministradorDeVentas.BusqueVentasPorId(idVenta);

            TempData["IdDeVentaAModificar"] = idVenta;
            return View();
        }

        // POST: VentasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult AgregarDescuentoAVenta(Model.Venta ventaModificada)
        {
            try
            {

                int idVentaAModificar = int.Parse(TempData["IdDeVentaAModificar"].ToString());

                ElAdministradorDeVentas.AgregueDescuento(idVentaAModificar, ventaModificada);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        public ActionResult TerminarVenta(int idVenta)
        {
            Model.Venta ventaAEditar = ElAdministradorDeVentas.BusqueVentasPorId(idVenta);

            TempData["IdDeVentaAModificar"] = idVenta;
            return View();
        }

        // POST: VentasController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult TerminarVenta(Model.Venta ventaModificada)
        {
            try
            {

                int idVentaAModificar = int.Parse(TempData["IdDeVentaAModificar"].ToString());

                ElAdministradorDeVentas.TermineLaVenta(idVentaAModificar, ventaModificada);


                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

      
    }
}
