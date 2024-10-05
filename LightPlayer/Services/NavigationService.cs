using LightPlayer.ViewModels;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Services
{
    public class NavigationService
    {
        private ViewModelBase? currentViewModel;

        public ViewModelBase? CurrentViewModel
        {
            get => currentViewModel;
            set
            {
                currentViewModel = value;
                CurrentViewModelChanged?.Invoke();
            }
        }

        public event Action? CurrentViewModelChanged;

        public void NavigateTo<T>() where T : ViewModelBase
             => CurrentViewModel = App.Current.Services.GetRequiredService<T>();
    }
}
