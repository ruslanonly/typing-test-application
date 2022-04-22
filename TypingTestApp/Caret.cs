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
        private static Timer BlinkTimer;
        private void BlinkAction(object obj, ElapsedEventArgs e)
        {
            if (_block.Opacity == 1)
            {
                _block.Opacity = 0;
            } else
            {
                _block.Opacity = 1;
            }
        }
        public void StartBlinking()
        {
            BlinkTimer = new Timer(1000);
            BlinkTimer.Elapsed += BlinkAction;
            BlinkTimer.Start();
        }

        public void StopBlinking()
        {
            BlinkTimer.Stop();
        }
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
