using System;
using System.Timers;
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
        private Rectangle _block;
        public Caret(Rectangle block)
        {
            _block = block;
        }
        delegate void CaretPositionSetter(DependencyProperty dp, double point);
        public void MoveTo(Point point)
        {
            CaretPositionSetter positionSetter = (dp, value) =>
            {
                SineEase easingFunction = new SineEase();
                easingFunction.EasingMode = EasingMode.EaseOut;
                DoubleAnimation animation = new DoubleAnimation();
                animation.To = value;
                animation.Duration = TimeSpan.FromMilliseconds(90);
                _block.BeginAnimation(dp, animation);
            };
            positionSetter(Canvas.LeftProperty, point.X);
            positionSetter(Canvas.TopProperty, point.Y);
        }
    }
}
