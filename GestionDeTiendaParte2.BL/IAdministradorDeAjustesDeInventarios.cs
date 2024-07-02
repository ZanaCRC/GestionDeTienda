using GestionDeTiendaParte1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public interface IAdministradorDeAjustesDeInventarios
    {
        List<Model.Inventario> ObtengaLaLista();
        List<Model.AjusteDeInventario> ObtengaListaDeAjustes(int idInventario);
        List<Model.AjusteDeInventario> ObtengaListaDeAjustesParaDetalle(int idAjusteInventario);
        bool AgregueAjuste(Model.AjusteDeInventario nuevoAjuste);
        public Inventario ObtenerInventarioPorId(int id);
    }
}
