using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public interface IAdministradorDeAjustesDeInventarios
    {
        List<Model.Inventario> ObtengaLaLista();
        public List<ModeloAjusteDeInventario> ObtengaListaDeAjustes(int idInventario);
        public List<ModeloAjusteDeInventario> ObtengaListaDeAjustesParaDetalle(int idAjusteInventario);
        public bool AgregueAjuste(ModeloAgregarAjuste nuevoAjuste);
        public Inventario ObtenerInventarioPorId(int id);
        public List<ModeloAjusteDeInventario> ConvertirAjustesDeInventario(List<AjusteDeInventario> ajustes);
        List<Model.Inventario> FiltreLaLista(string nombre);
    }
}
