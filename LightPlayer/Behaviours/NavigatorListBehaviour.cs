using LightPlayer.Services;
using LightPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Xaml.Behaviors;
using System;
using System.CodeDom;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LightPlayer.Behaviours
{
    internal class NavigatorListBehaviour : Behavior<ListBox>
    {
        private NavigationService navigationService;
        private string? currentView;
        private ListBox listBox;

        protected override void OnAttached()
        {
            listBox = AssociatedObject as ListBox;
            listBox.SelectionChanged += SelectionChangedHandeler;
            navigationService = App.Current.Services.GetRequiredService<NavigationService>();
        }
        private void SelectionChangedHandeler(object sender, SelectionChangedEventArgs e)
        {
            var view = listBox.SelectedItem.ToString();
            if (listBox.SelectedItem.ToString() != currentView)
            {
                if (view == "Music")
                    navigationService.NavigateTo<MusicViewModel>();
                if (view == "Album")
                    navigationService.NavigateTo<AlbumViewModel>();
                if (view == "Musician")
                    navigationService.NavigateTo<MusicianViewModel>();
                if (view == "Playlist")
                    navigationService.NavigateTo<PlaylistViewModel>();

            }
            currentView = view!;
        }
    }
}
