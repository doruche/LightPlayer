using Microsoft.Xaml.Behaviors;
using ModernWpf.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace LightPlayer.Behaviours
{
    public class ScaleButtonBehaviour : Behavior<Button>
    {


        public SplitView Target
        {
            get { return (SplitView)GetValue(TargetProperty); }
            set { SetValue(TargetProperty, value); }
        }

        // Using a DependencyProperty as the backing store for Target.  This enables animation, styling, binding, etc...
        public static readonly DependencyProperty TargetProperty =
            DependencyProperty.Register("Target", typeof(SplitView), typeof(ScaleButtonBehaviour), new PropertyMetadata(null));



        private Button button;

        protected override void OnAttached()
        {
            button = AssociatedObject as Button;
            button.Click += (_, _) =>
            {
                Target.IsPaneOpen = !Target.IsPaneOpen;
            };
        }
    }
}
