using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionDeTiendaParte2.App.Interfaces;
using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.App.ViewModels
{
    public partial class InventarioViewModel : ObservableValidator

    {
       
       
        public ObservableCollection<Inventario> Inventarios { get; set; } = new();
        
        private readonly IUserService userService;
        public InventarioViewModel()
        {
            userService = App.Current.Services.GetService<IUserService>();

        }
       
        [RelayCommand]
        public async Task LoadInventarios()
        {
            

            try
            {
                Inventarios.Clear();
                var items = await userService.ObtenerLista();
                foreach (var item in items)
                {
                    Inventarios.Add(item);
                }
            }
            catch (Exception ex)
            {
                // Manejar error
                Console.WriteLine($"Error loading inventory: {ex.Message}");
            }
            
        }
    }
}
