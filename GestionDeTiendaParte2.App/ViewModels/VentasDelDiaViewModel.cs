using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionDeTiendaParte2.App.Interfaces;
using GestionDeTiendaParte2.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.App.ViewModels
{
    public partial class VentasDelDiaViewModel : ObservableValidator
    {
        private InformacionCaja informacionCaja;

        private readonly IUserService userService;
        public VentasDelDiaViewModel()
        {
            userService = App.Current.Services.GetService<IUserService>();

        }

        public InformacionCaja InformacionCaja 
        {

           get => informacionCaja;
           set => SetProperty(ref informacionCaja, value);
        }

        [RelayCommand]
        private async Task LoadAperturaDeCaja()
        {

            try
            {
                // Cargar datos del objeto Inventario desde el servicio
                InformacionCaja = await userService.ObtenerInformacionCajasDeHoy();
            }
            catch (Exception ex)
            {
                // Manejar error
                Console.WriteLine($"Error loading inventory: {ex.Message}");
            }
            
        }

    }
}
