using GestionDeTiendaParte1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public interface IAdministradorDeInventarios
    {
        public void Agregue(Model.Inventario inventario, string nombreUsuario);
        public Model.Inventario ObtengaElInventario(int id);
        public void Edite(Model.Inventario inventario, string elNombreDelUsuario);
        public Model.Inventario ObtenerPorId(int Id);
        public List<Model.Inventario> ObtengaLaLista();
        public List<Model.Historico> ObtengaHistorico();
    }
}
