using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LightPlayer.Views
{
    public struct TabView
    {
        public readonly string View { get; init; }

        public string Path { get; private set; }

        public TabView(string view)
        {
            View = view;
            Path = $"pack://application:,,,/Properties/{view}.png";
        }

        public override string ToString() => View;
    }
}
