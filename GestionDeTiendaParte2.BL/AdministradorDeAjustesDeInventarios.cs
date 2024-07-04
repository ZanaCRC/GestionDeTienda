using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.BL
{
    public class AdministradorDeAjustesDeInventarios : IAdministradorDeAjustesDeInventarios
    {
        private readonly DA.DBContexto ElContexto;

        public AdministradorDeAjustesDeInventarios(DA.DBContexto dbContext)
        {
            ElContexto = dbContext;
        }

        public List<Inventario> ObtengaLaLista()
        {
            try
            {
                return ElContexto.Inventarios.ToList();
            }
            catch (Exception ex)
            {
               
                throw new Exception("Error al obtener la lista de inventarios.", ex);
            }
        }

        public List<AjusteDeInventario> ObtengaListaDeAjustes(int idInventario)
        {
            try
            {
                var ajustes = ElContexto.AjusteDeInventarios
                                        .Where(c => c.Id_Inventario == idInventario)
                                        .ToList();

                CargarNombresUsuarios(ajustes);

                return ajustes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los ajustes para el inventario con ID {idInventario}.", ex);
            }
        }

        public List<AjusteDeInventario> ObtengaListaDeAjustesParaDetalle(int idAjusteInventario)
        {
            try
            {
                var ajustes = ElContexto.AjusteDeInventarios
                                        .Where(c => c.Id == idAjusteInventario)
                                        .ToList();

                CargarNombresUsuarios(ajustes);

                return ajustes;
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener los ajustes con ID {idAjusteInventario}.", ex);
            }
        }

        public bool AgregueAjuste(AjusteDeInventario nuevoAjuste)
        {
            using (var transaction = ElContexto.Database.BeginTransaction())
            {
                try
                {
                    var inventario = ElContexto.Inventarios.Find(nuevoAjuste.Id_Inventario);

                    if (inventario == null)
                    {
                        throw new Exception($"No se encontró el inventario con ID {nuevoAjuste.Id_Inventario}.");
                    }

                    if (nuevoAjuste.Tipo == TipoInventario.Aumento)
                    {
                        inventario.Cantidad += nuevoAjuste.Ajuste;
                    }
                    else if (nuevoAjuste.Tipo == TipoInventario.Disminucion)
                    {
                        if (inventario.Cantidad < nuevoAjuste.Ajuste)
                        {
                            return false;
                        }
                        else
                        {
                            inventario.Cantidad -= nuevoAjuste.Ajuste;
                        }
                    }
                    else
                    {
                        throw new Exception("Tipo de ajuste no válido.");
                    }

                    nuevoAjuste.CantidadActual = inventario.Cantidad;
                    nuevoAjuste.Fecha = DateTime.Now;

                    ElContexto.Inventarios.Update(inventario);
                    ElContexto.AjusteDeInventarios.Add(nuevoAjuste);
                    ElContexto.SaveChanges();

                    transaction.Commit();
                    return true;
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    throw new Exception("Error al agregar el ajuste de inventario.", ex);
                }
            }
        }


        private void CargarNombresUsuarios(List<AjusteDeInventario> ajustes)
        {
            foreach (var ajuste in ajustes)
            {
                var usuario = ElContexto.Usuarios.Find(ajuste.UserId);
                if (usuario != null)
                {
                    ajuste.Usuario.Nombre = usuario.Nombre;
                }
            }
        }
        public Inventario ObtenerInventarioPorId(int Id)
        {
            try
            {
                return ElContexto.Inventarios.FirstOrDefault(i => i.id == Id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener el inventario con ID {Id}.", ex);
            }
        }
    }

}
