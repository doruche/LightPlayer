﻿using LightPlayer.Behaviours;
using LightPlayer.ViewModels;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace LightPlayer.Views
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow(MainWindowViewModel vm)
        {
            InitializeComponent();
            this.DataContext = vm;
        }

        private void PlaylistButtonClick(object sender, RoutedEventArgs e)
        {
            if(playlistBorder.Visibility == Visibility.Visible)
                playlistBorder.Visibility = Visibility.Collapsed;
            else
                playlistBorder.Visibility = Visibility.Visible;
        }
    }
}