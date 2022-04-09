using System;
using System.Runtime;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    public class TestStats
    {
        public static List<double> WpmHistory = new List<double>();
        public static int CorrectWords = 0;
        public static int CorrectLetters = 0;
        public static int PressedKeys = 0;
        public static int Wpm
        {
            get
            {
                double wps = CorrectWords / TestTimer.Value; 
                int wpm = (int)Math.Floor(wps * 60);
                WpmHistory.Add(wpm);
                return wpm;
            }
        }

        public static double AverageWpm
        {
            get
            {
                double summaryWps = 0;
                for (int i = 0; i < WpmHistory.Count; i++)
                {
                    summaryWps += WpmHistory[i];
                }
                if (WpmHistory.Count == 0)
                {
                    return Wpm;
                } else
                {
                    return Math.Floor(summaryWps / WpmHistory.Count);
                }
            }
        }

        public static int Cpm
        {
            get
            {
                double cps = CorrectLetters / TestTimer.Value;
                int cpm = (int)Math.Floor(cps * 60);
                WpmHistory.Add(cpm);
                return cpm;
            }
        }

        public static double Accuracy
        {
            get
            {
                double accFloating = (double)CorrectLetters / (double)PressedKeys;
                double acc = Math.Floor(accFloating * 100);
                return acc;
            }
        }

        public static double Time
        {
            get
            {
                return Math.Round(TestTimer.Value, 1);
            }
        }

        public static void Reset()
        {
            CorrectWords = CorrectLetters = PressedKeys = 0;
        }
    }
}
