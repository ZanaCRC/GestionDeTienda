using GestionDeTiendaParte2.App.ViewModels;

namespace GestionDeTiendaParte2.App.Views;

public partial class Ventas : ContentPage
{
	public Ventas()
	{
        BindingContext = App.Current.Services.GetRequiredService<VentasDelDiaViewModel>();
        InitializeComponent();
	}
}