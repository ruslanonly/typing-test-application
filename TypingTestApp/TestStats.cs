using System;
using System.Runtime;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    class TestStats
    {
        public static int CorrectWords = 0;
        public static int CorrectLetters = 0;
        public static int PressedKeys = 0;
        public static double Wpm
        {
            get
            {
                double wps = CorrectWords / TestTimer.Value;
                return Math.Floor(wps * 60);
            }
        }

        public static double Cpm
        {
            get
            {
                double cps = CorrectLetters / TestTimer.Value;
                return Math.Floor(cps * 60);
            }
        }

        public static void Reset()
        {
            CorrectWords = CorrectLetters = PressedKeys = 0;
        }
    }
}
