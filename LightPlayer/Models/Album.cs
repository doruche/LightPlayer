using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace LightPlayer.Models
{
    public partial class Album : ObservableObject
    {

        [ObservableProperty]
        private string name;

        [ObservableProperty]
        private string? description;

        [ObservableProperty]
        private string? icon;

        [ObservableProperty]
        private ObservableCollection<Song>? songs;

        public Album(string name, string? description = null, string? icon = null)
        {
            Name = name;
            Description= description;
            Icon = icon;
            Songs = new();
        }
    }
}
