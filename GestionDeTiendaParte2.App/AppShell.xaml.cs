using GestionDeTiendaParte2.App.Views;

namespace GestionDeTiendaParte2.App
{
    public partial class AppShell : Shell
    {
        public AppShell()
        {
            Routing.RegisterRoute(nameof(Login), typeof(Login));
            Routing.RegisterRoute(nameof(Views.Index), typeof(Views.Index));
            Routing.RegisterRoute(nameof(Inventarios), typeof(Inventarios));
            InitializeComponent();
        }
    }
}
