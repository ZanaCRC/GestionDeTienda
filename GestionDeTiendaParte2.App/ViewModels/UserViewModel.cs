using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
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
        public async Task IniciarSesion()
        {

            Errors.Clear();
            ValidateAllProperties();
            GetErrors(nameof(Usuario)).ToList().ForEach(n => Errors.Add("Usuario: " + n.ErrorMessage));
            GetErrors(nameof(Clave)).ToList().ForEach(n => Errors.Add("Clave: " + n.ErrorMessage));

            if (Errors.Count > 0) return;
            


        }
    }
}
