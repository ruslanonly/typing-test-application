using System;
using System.Windows;
using System.Windows.Media.Animation;
using System.Threading.Tasks;
using System.Windows.Media;

namespace TypingTestApp
{
    public class Animate
    {
        public static void FadeIn(UIElement block)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(250);
            animation.To = 1;
            block.BeginAnimation(UIElement.OpacityProperty, animation);
        }

        public static void FadeOut(UIElement block)
        {
            DoubleAnimation animation = new DoubleAnimation();
            animation.Duration = TimeSpan.FromMilliseconds(250);
            animation.To = 0;
            block.BeginAnimation(UIElement.OpacityProperty, animation);
        }
    }
}
