using GestionDeTiendaParte2.App.ViewModels;

namespace GestionDeTiendaParte2.App.Views;

public partial class Inventarios : ContentPage
{
	public Inventarios()
	{
        BindingContext = App.Current.Services.GetRequiredService<InventarioViewModel>();
        InitializeComponent();
	}
}