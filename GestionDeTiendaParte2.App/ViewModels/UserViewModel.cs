using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using GestionDeTiendaParte2.App.Interfaces;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GestionDeTiendaParte2.App.ViewModels
{
    public partial class UserViewModel : ObservableValidator
    {

        public ObservableCollection<string> Errors { get; set; } = new();
        private string usuario;
        private readonly IUserService userService;
        
        public event Action OnLoginSuccess;
        public UserViewModel()
        {
            userService = App.Current.Services.GetService<IUserService>();
        }
        [Required(ErrorMessage = "Usuario es Requerido")]
        
        public string Usuario
        {
            get { return usuario; }
            set { SetProperty(ref usuario, value); }
        }

        private string clave;
        [Required(ErrorMessage = "Clave es Requerida")]
        
        public string Clave
        {
            get { return clave; }
            set { SetProperty(ref clave, value); }
        }

        private string codUser;

        

        [RelayCommand]
       

            private async Task IniciarSesion()
            {
                var elUsuario = await userService.IniciarSesion(Usuario, Clave);

                if (elUsuario != null && elUsuario.Clave != null)
                {
                OnLoginSuccess?.Invoke();
                Shell.Current.Navigation.PushAsync(new Views.Index(), true);

            }
                else
                {
                    // Manejar caso de credenciales incorrectas o usuario externo
                    await Application.Current.MainPage.DisplayAlert("Error", "Usuario o contraseña incorrectos", "OK");
                }
            }



        }
    
}
