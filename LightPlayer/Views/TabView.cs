using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Views
{
    public partial class TabView : ObservableObject
    {
        public string View { get; set; }

        [ObservableProperty]
        private string path;

        public TabView(string view)
        {
            View = view;
            Path = $"pack://application:,,,/Properties/{view}.png";
        }

        public override string ToString() => View;
    }
}
