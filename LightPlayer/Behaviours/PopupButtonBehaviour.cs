using Microsoft.Xaml.Behaviors;
using System;
using ModernWpf;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Forms.Design.Behavior;
using ModernWpf.Controls.Primitives;

namespace LightPlayer.Behaviours
{
    public class PopupButtonBehaviour : Behavior<PopupEx>
    {
        public Button TargetButton
        {
            get { return (Button)GetValue(TargetButtonProperty); }
            set { SetValue(TargetButtonProperty, value); }
        }

        public static readonly DependencyProperty TargetButtonProperty =
            DependencyProperty.Register("TargetButton",
                typeof(Button),
                typeof(PopupButtonBehaviour),
                new PropertyMetadata(null));



        protected override void OnAttached()
        {
            TargetButton.MouseEnter += TargetButton_MouseEnter;
            TargetButton.MouseLeave += TargetButton_MouseLeave;
            AssociatedObject.MouseLeave += AssociatedObject_MouseLeave;
        }

        private void AssociatedObject_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.IsOpen = false;
        }

        private void TargetButton_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {
            if(AssociatedObject.IsMouseOver == false)
                AssociatedObject.IsOpen = false;
        }

        private void TargetButton_MouseEnter(object sender, System.Windows.Input.MouseEventArgs e)
        {
            AssociatedObject.IsOpen = true;
        }
    }
}
