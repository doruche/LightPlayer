using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LightPlayer.ViewModels;
using LightPlayer.Views;
using Microsoft.Extensions.DependencyInjection;

namespace LightPlayer.Services
{
    public class NavigationService
    {
        private ViewModelBase? currentViewModel;

        private TabView currentTabView;

        public ViewModelBase? CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                currentTabView = new(
                    value!.GetType().Name switch
                    {
                        "MusicViewModel" => "Music",
                        "MusicianViewModel" => "Musician",
                        "AlbumViewModel" => "Album",
                        "PlaylistViewModel" => "Playlist",
                        "SearchPageViewModel" => "SearchPage",
                        _ => throw new Exception(nameof(value) + "not supported.")
                    });
                CurrentViewModelChanged?.Invoke();
            }
        }

        public TabView CurrentTabView => currentTabView;

        public event Action? CurrentViewModelChanged;

        public void NavigateTo<T>() where T : ViewModelBase
             => CurrentViewModel = App.Current.Services.GetRequiredService<T>();

        public void NavigateTo(TabView tabView)
        {
            if (tabView.View != currentTabView.View)
            {
                var view = tabView.View;
                if (view == "Music")
                    NavigateTo<MusicViewModel>();
                if (view == "Album")
                    NavigateTo<AlbumViewModel>();
                if (view == "Musician")
                    NavigateTo<MusicianViewModel>();
                if (view == "Playlist")
                    NavigateTo<PlaylistViewModel>();
            }
        }

        public void GoToSearchPage(string? filterText)
        {
            if (string.IsNullOrEmpty(filterText) || string.IsNullOrWhiteSpace(filterText))
                return;
            CurrentViewModel = App.Current.Services.GetRequiredService<SearchPageViewModel>();
            WeakReferenceMessenger.Default.Send(new ValueChangedMessage<string>(filterText));
        }
    }
}
