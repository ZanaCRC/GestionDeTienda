﻿using GestionDeTiendaParte2.App.Interfaces;
using GestionDeTiendaParte2.App.Services;
using GestionDeTiendaParte2.App.ViewModels;
using GestionDeTiendaParte2.App.Views;

namespace GestionDeTiendaParte2.App
{
    public partial class App : Application
    {
        public IServiceProvider Services { get; }
        public new static App Current => (App)Application.Current;
        public App()
        {
            var services = new ServiceCollection();
            Services = ConfigureServices(services);

            InitializeComponent();

            MainPage = new AppShell();
        }

        private static IServiceProvider ConfigureServices(ServiceCollection services)
        {
            services.AddSingleton<IUserService, UserService>();

            services.AddTransient<UserViewModel>();
            services.AddTransient<InventarioViewModel>();
            services.AddTransient<VentasDelDiaViewModel>();

            services.AddSingleton<Login>();
            services.AddSingleton<Views.Index>();
            services.AddSingleton<Inventarios>();
            services.AddSingleton<Ventas>();

            return services.BuildServiceProvider();
        }
    }
}

