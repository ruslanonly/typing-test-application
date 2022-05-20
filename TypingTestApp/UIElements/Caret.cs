using System;
using System.Timers;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using System.Threading;
using System.Windows.Threading;

namespace TypingTestApp
{
    public class Caret
    {
        private Rectangle _block;
        public Caret(Rectangle block)
        {
            _block = block;
        }

        public System.Timers.Timer BlinkInterval;
        public void StartBlinking()
        {
            BlinkInterval = new System.Timers.Timer();
            BlinkInterval.Interval = 1000;
            BlinkInterval.Elapsed += (object obj, ElapsedEventArgs e) => {
                _block.Opacity = _block.Opacity == 1 ? 0 : 1;
            };
            BlinkInterval.Start();
        }

        public void stopBlinking()
        {
            BlinkInterval.Stop();
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
