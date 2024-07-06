using GestionDeTiendaParte2.App.ViewModels;

namespace GestionDeTiendaParte2.App.Views;

public partial class Login : ContentPage
{
	public Login()
	{
        BindingContext = App.Current.Services.GetRequiredService<UserViewModel>();
        InitializeComponent();
	}

    protected override void OnAppearing()
    {
        base.OnAppearing();
        if (BindingContext is UserViewModel viewModel)
        {
            viewModel.OnLoginSuccess += HideKeyboard;
        }
    }

    private void HideKeyboard()
    {
        UsuarioEntry.Unfocus();
        ClaveEntry.Unfocus();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        if (BindingContext is UserViewModel viewModel)
        {
            viewModel.OnLoginSuccess -= HideKeyboard;
        }
    }
}