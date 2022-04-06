using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Animation;

namespace TypingTestApp
{
    public class Caret
    {
        Rectangle _block;
        public Caret(Rectangle block)
        {
            _block = block;
        }
        public void MoveTo(Point point)
        {
            SineEase easingFunction = new SineEase();
            easingFunction.EasingMode = EasingMode.EaseOut;
            DoubleAnimation LeftAnimation = new DoubleAnimation();
            LeftAnimation.To = point.X;
            LeftAnimation.Duration = TimeSpan.FromMilliseconds(90);
            DoubleAnimation TopAnimation = new DoubleAnimation();
            TopAnimation.To = point.Y;
            TopAnimation.Duration = TimeSpan.FromMilliseconds(90);
            _block.BeginAnimation(Canvas.LeftProperty, LeftAnimation);
            _block.BeginAnimation(Canvas.TopProperty, TopAnimation);
        }
    }
}
