using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public interface IAdministradorDeInventarios
    {
        public void Agregue(Model.Inventario inventario, string nombreUsuario);
        public Model.Inventario ObtengaElInventario(int id);
        public void Edite(Model.ModeloInventario inventario);
        public Model.Inventario ObtenerPorId(int Id);
        public List<Model.Inventario> ObtengaLaLista();
        public List<Model.Historico> ObtengaHistorico();
        public List<Model.Inventario> FiltreLaLista(string nombre);
    }
}
