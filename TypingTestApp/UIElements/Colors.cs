using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Media;

namespace TypingTestApp
{
    public class Colors
    {
        public static SolidColorBrush Mistake100 = new SolidColorBrush(Color.FromRgb(252, 210, 101));
        public static SolidColorBrush Mistake200 = new SolidColorBrush(Color.FromRgb(250, 163, 7));
        public static SolidColorBrush Mistake300 = new SolidColorBrush(Color.FromRgb(232, 93, 4));
        public static SolidColorBrush Mistake400 = new SolidColorBrush(Color.FromRgb(208, 0, 0));
        public static SolidColorBrush Mistake500 = new SolidColorBrush(Color.FromRgb(220, 47, 2));
        public static SolidColorBrush LightFont = new SolidColorBrush(Color.FromRgb(130, 144, 148));
        public static SolidColorBrush DefaultLetter = new SolidColorBrush(Color.FromRgb(83, 97, 110));
        public static SolidColorBrush CorrectLetter = new SolidColorBrush(Color.FromRgb(236, 237, 244));
        public static SolidColorBrush IncorrectLetter = new SolidColorBrush(Color.FromRgb(248, 150, 30));
        public static SolidColorBrush MainBg = new SolidColorBrush(Color.FromRgb(38, 46, 54));
        public static SolidColorBrush[] SecondaryBg = { new SolidColorBrush(Color.FromRgb(43, 50, 59)), new SolidColorBrush(Color.FromRgb(34, 41, 49)) };
    }
}
