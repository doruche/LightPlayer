using LightPlayer.Services;
using LightPlayer.ViewModels;
using LightPlayer.Views;
using Microsoft.Extensions.DependencyInjection;
using System.ComponentModel.Design;
using System.Configuration;
using System.Data;
using System.Windows;

namespace LightPlayer
{
    public partial class App : Application
    {
        public new static App Current => (App)Application.Current;

        public IServiceProvider Services { get; }
        public App()
        {
            Services = ConfigureServices();
        }

        private static IServiceProvider ConfigureServices()
        {
            var services = new ServiceCollection();

            services.AddSingleton<NavigationService>();
            services.AddSingleton<IMusicDBService, MusicDBService>();
            services.AddTransient<IGetFileService, GetFileService>();
            services.AddSingleton<IPlayMusicService, PlayMusicService>();

            services.AddTransient<MainWindow>();
            services.AddTransient<MainWindowViewModel>();
            services.AddSingleton<Music>();
            services.AddSingleton<MusicViewModel>();
            services.AddSingleton<Album>();
            services.AddSingleton<AlbumViewModel>();
            services.AddSingleton<Musician>();
            services.AddSingleton<MusicianViewModel>();
            services.AddSingleton<Playlist>();
            services.AddSingleton<PlaylistViewModel>();
            services.AddTransient<SearchPage>();
            services.AddSingleton<SearchPageViewModel>();

            return services.BuildServiceProvider();
        }

        private void Application_Startup(object sender, StartupEventArgs e)
        {
            var mainWindow = Services.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }
    }

}
