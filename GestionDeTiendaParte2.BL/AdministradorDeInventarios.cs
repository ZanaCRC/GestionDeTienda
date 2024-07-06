using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public class AdministradorDeInventarios: IAdministradorDeInventarios
    {

        private GestionDeTiendaParte2.DA.DBContexto ElContextoBD;
        public AdministradorDeInventarios(DA.DBContexto elContexto)
        {
            ElContextoBD = elContexto;
        }

        public void Agregue(Model.ModeloInventario inventario, string nombreUsuario)
        {
            Model.Historico historico = new Model.Historico();
            inventario.Cantidad = 0;
            Model.Inventario nuevoIventario = new Inventario();
            nuevoIventario.id = inventario.id;

            nuevoIventario.Nombre = inventario.Nombre;
            nuevoIventario.Categoria = inventario.Categoria;
            nuevoIventario.Cantidad = inventario.Cantidad; 
            nuevoIventario.Precio = inventario.Precio;


            ElContextoBD.Inventarios.Add(nuevoIventario);
            ElContextoBD.SaveChanges();

            historico.ElNombre = inventario.Nombre;
            historico.FechaYHora = DateTime.Now;
            historico.ElTipoDeModificacion = Model.TipoModificacion.Creacion;
            historico.IdInventario = nuevoIventario.id;
            historico.NombreUsuario = nombreUsuario;
            historico.ElPrecio = inventario.Precio;
            historico.LaCategoria = inventario.Categoria;

            
            ElContextoBD.Historico.Add(historico);
            ElContextoBD.SaveChanges();
            
        }

        public List<Model.Inventario> ObtengaLaLista()
        {
            var resultado = from c in ElContextoBD.Inventarios
                            select new Model.Inventario
                            {
                                id = c.id,
                                Nombre = c.Nombre,
                                Categoria = c.Categoria,
                                Cantidad = c.Cantidad,
                                Precio = c.Precio
                               
                            };
            return resultado.ToList();
        }

        public Model.Inventario ObtengaElInventario(int id)
        {
            Model.Inventario resultado;
            resultado = ElContextoBD.Inventarios
                            .Where(i => i.id == id)
                            .Select(i => new Model.Inventario
                            {
                                id = i.id,
                                Nombre = i.Nombre,
                                Categoria = i.Categoria,
                                Cantidad = i.Cantidad,
                                Precio = i.Precio
                                // Excluir UserId
                            })
                            .FirstOrDefault();
            return resultado;
        }

        public List<Model.Inventario> FiltreLaLista(string nombre)
        {
            List<Model.Inventario> listaDeLibros = ObtengaLaLista();
            return listaDeLibros.Where(x => x.Nombre.Contains(nombre)).ToList();
        }


        public List<Model.Historico> ObtengaHistorico(int id)
        {

            try
            {
                var resultado = from c in ElContextoBD.Historico
                                where c.IdInventario == id
                                select c  ;
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

        public void Edite(Model.ModeloInventario inventario)
        {
            Model.Inventario inventarioEditado = new Inventario();

            inventarioEditado.id = inventario.id;
            inventarioEditado.Nombre = inventario.Nombre;
            inventarioEditado.Categoria = inventario.Categoria;
            inventarioEditado.Precio = inventario.Precio;
            inventarioEditado.Cantidad = inventario.Cantidad;


            Model.Historico historico = new Model.Historico();
            Model.Inventario PorModificar = ObtengaElInventario(inventario.id);

            PorModificar.Nombre = inventarioEditado.Nombre;
            PorModificar.Categoria = inventarioEditado.Categoria;
            PorModificar.Precio = inventarioEditado.Precio;

            ElContextoBD.Inventarios.Update(PorModificar);
            ElContextoBD.SaveChanges();

            historico.ElNombre = inventario.Nombre;
            historico.NombreUsuario = inventario.UserName;
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
