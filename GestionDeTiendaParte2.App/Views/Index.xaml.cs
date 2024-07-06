using GestionDeTiendaParte2.App.ViewModels;

namespace GestionDeTiendaParte2.App.Views;

public partial class Index : ContentPage
{
	public Index()
	{
		InitializeComponent();
	}
    private async void OnListarInventariosClicked(object sender, EventArgs e)
    {
        var inventariosPage = new Views.Inventarios();
        await Navigation.PushAsync(inventariosPage);
    }

    public async void OnListarVentasClicked(object sender, EventArgs e)
    {
        var ventasPage = new Views.Ventas();
        await Navigation.PushAsync(ventasPage);
    }

    private void OnCerrarSesionClicked(object sender, EventArgs e)
    {
        Navigation.PopToRootAsync();
    }
}