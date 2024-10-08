using LightPlayer.Services;
using LightPlayer.ViewModels;
using LightPlayer.Views;
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
        private ListBox listBox;

        protected override void OnAttached()
        {
            listBox = AssociatedObject as ListBox;
            listBox.SelectionChanged += SelectionChangedHandeler;
            navigationService = App.Current.Services.GetRequiredService<NavigationService>();
        }
        private void SelectionChangedHandeler(object sender, SelectionChangedEventArgs e)
        {
            if (listBox.SelectedIndex == -1)
                return;
            var view = (TabView)listBox.SelectedItem;
            navigationService.NavigateTo(view);
        }
    }
}
