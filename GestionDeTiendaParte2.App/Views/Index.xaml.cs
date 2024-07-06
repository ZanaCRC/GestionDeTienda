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
}