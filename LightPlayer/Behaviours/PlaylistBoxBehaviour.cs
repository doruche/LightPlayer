using Microsoft.Xaml.Behaviors;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace LightPlayer.Behaviours
{
    public class PlaylistBoxBehaviour : Behavior<ListBox>
    {
        public int Index
        {
            get { return (int)GetValue(IndexProperty); }
            set { SetValue(IndexProperty, value); }
        }

        public static readonly DependencyProperty IndexProperty =
            DependencyProperty.Register("Index",
                typeof(int),
                  typeof(PlaylistBoxBehaviour),
                new PropertyMetadata(0, IndexChangedCallBack));


        private static void IndexChangedCallBack(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            var listBox = (d as PlaylistBoxBehaviour)?.AssociatedObject;
            if (listBox != null && (int)e.NewValue != -1 && (int)e.OldValue != -1)
            {
                var previousItem = listBox.ItemContainerGenerator.ContainerFromIndex((int)e.OldValue) as ListBoxItem;
                var currentItem = listBox.ItemContainerGenerator.ContainerFromIndex((int)e.NewValue) as ListBoxItem;
            }
        }
    }
}
