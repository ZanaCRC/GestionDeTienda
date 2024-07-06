using GestionDeTiendaParte2.App.ViewModels;

namespace GestionDeTiendaParte2.App.Views;

public partial class Login : ContentPage
{
	public Login()
	{
        BindingContext = App.Current.Services.GetRequiredService<UserViewModel>();
        InitializeComponent();
	}
}