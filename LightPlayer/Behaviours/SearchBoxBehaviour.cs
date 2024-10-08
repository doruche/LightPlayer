using CommunityToolkit.Mvvm.Messaging;
using CommunityToolkit.Mvvm.Messaging.Messages;
using LightPlayer.Models;
using Microsoft.Xaml.Behaviors;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;
using System.Windows.Media;

namespace LightPlayer.Behaviours
{
    public class SearchBoxBehaviour : Behavior<ComboBox>
    {
        public string FilterText
        {
            get { return (string)GetValue(FilterTextProperty); }
            set{ SetValue(FilterTextProperty, value); }
        }
        public static readonly DependencyProperty FilterTextProperty =
            DependencyProperty.Register("FilterText",
                typeof(string),
                typeof(SearchBoxBehaviour),
                new PropertyMetadata(string.Empty, (d, e) =>
                {
                    var b = d as SearchBoxBehaviour;
                    if (b.SongsSource == null) return;
                    b!.AssociatedObject.ItemsSource = b.SongsSource.Where(x => Song.IsRelated(x, (string)e.NewValue));
                    if (((string)e.NewValue).Length < ((string)e.OldValue).Length)
                        b.SelectedIndex = -1;
                }));



        public int SelectedIndex
        {
            get { return (int)GetValue(SelectedIndexProperty); }
            set { SetValue(SelectedIndexProperty, value); }
        }

        public static readonly DependencyProperty SelectedIndexProperty =
            DependencyProperty.Register("SelectedIndex", typeof(int), typeof(SearchBoxBehaviour), new PropertyMetadata(-1));



        public IEnumerable<Song> SongsSource
        {
            get { return (IEnumerable<Song>)GetValue(SongsSourceProperty); }
            set { SetValue(SongsSourceProperty, value); }
        }

        public static readonly DependencyProperty SongsSourceProperty =
            DependencyProperty.Register("SongsSource", typeof(IEnumerable<Song>), typeof(SearchBoxBehaviour), new PropertyMetadata(null));


        protected override void OnAttached()
        {
            AssociatedObject.SelectedIndex = -1;
            AssociatedObject.GotFocus += GotFocus;
            AssociatedObject.LostFocus += LostFocus;
            AssociatedObject.SelectionChanged += AssociatedObject_SelectionChanged;
        }

        private void AssociatedObject_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (SelectedIndex != -1)
            {
                WeakReferenceMessenger.Default.Send(new ValueChangedMessage<Song>((Song)AssociatedObject.SelectedItem));
            }
        }

        private void LostFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.IsDropDownOpen = false;
        }

        private void GotFocus(object sender, RoutedEventArgs e)
        {
            AssociatedObject.IsDropDownOpen = true;
        }
    }
}
