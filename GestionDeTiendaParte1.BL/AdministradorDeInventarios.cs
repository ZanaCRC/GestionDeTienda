using GestionDeTiendaParte1.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte1.BL
{
    public class AdministradorDeInventarios: IAdministradorDeInventarios
    {

        private GestionDeTiendaParte1.DA.DBContext ElContextoBD;
        public AdministradorDeInventarios(DA.DBContext elContexto)
        {
            ElContextoBD = elContexto;
        }

        public void Agregue(Model.Inventario inventario, string nombreUsuario)
        {
            Model.Historico historico = new Model.Historico();
            

            inventario.Cantidad = 0;
            ElContextoBD.Inventarios.Add(inventario);
            ElContextoBD.SaveChanges();

            historico.ElNombre = inventario.Nombre;
            historico.FechaYHora = DateTime.Now;
            historico.ElTipoDeModificacion = Model.TipoModificacion.Creacion;
            historico.IdInventario = inventario.id;
            historico.NombreUsuario =   nombreUsuario;
            historico.ElPrecio = inventario.Precio;
            historico.LaCategoria = inventario.Categoria;

            
            ElContextoBD.Historico.Add(historico);
            ElContextoBD.SaveChanges();
            

        }

        public List<Model.Inventario> ObtengaLaLista()
        {
            var resultado = from c in ElContextoBD.Inventarios
                            select c;
            return resultado.ToList();
        }
        public Model.Inventario ObtengaElInventario(int id)
        {
            Model.Inventario resultado;
            resultado = ElContextoBD.Inventarios.Find(id);
            return resultado;

        }


        public List<Model.Historico> ObtengaHistorico()
        {
            try
            {
                var resultado = from c in ElContextoBD.Historico
                                select c;
                return resultado.ToList();
            }
            catch (Exception ex)
            {

                throw new Exception(ex.Message);
            }
        }
        public Model.Inventario ObtenerPorId(int Id)
        {
            Model.Inventario resultado;
            resultado = ElContextoBD.Inventarios.Find(Id);
            return resultado;
        }

        public void Edite(Model.Inventario inventario, string elNombreDelUsuario)
        {
            Model.Historico historico = new Model.Historico();
            Model.Inventario PorModificar = ObtenerPorId(inventario.id);

            PorModificar.Nombre = inventario.Nombre;
            PorModificar.Categoria = inventario.Categoria;
            PorModificar.Precio = inventario.Precio;

            ElContextoBD.Inventarios.Update(PorModificar);
            ElContextoBD.SaveChanges();

            historico.ElNombre = inventario.Nombre;
            historico.NombreUsuario = elNombreDelUsuario;
            historico.FechaYHora = DateTime.Now;
            historico.ElTipoDeModificacion = Model.TipoModificacion.Edicion;
            historico.IdInventario = inventario.id;
            historico.LaCategoria = inventario.Categoria;
            historico.ElPrecio = inventario.Precio;

            ElContextoBD.Historico.Add(historico);
            ElContextoBD.SaveChanges();

        }


    }
}
