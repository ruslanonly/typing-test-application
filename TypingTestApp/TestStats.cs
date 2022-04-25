using System;
using System.Runtime;
using System.Collections.Generic;
using System.Windows;
using System.Text;

namespace TypingTestApp
{
    public class Stat
    {
        public static int Number = 1;
        public int Wpm;
        public int Cpm;
        public double Accuracy;
        public double Time;
        public Stat(int Wpm, int Cpm, double Accuracy, double Time)
        {
            this.Wpm = Wpm;
            this.Cpm = Cpm;
            this.Accuracy = Accuracy;
            this.Time = Time;
        }
    }
    public class TestStats
    {
        public static List<Stat> StatHistory = new List<Stat>();
        public static List<int> WpmHistory = new List<int>();
        public static int CorrectWords = 0;
        public static int CorrectLetters = 0;
        public static int PressedKeys = 0;
        public static int Wpm
        {
            get
            {
                double wps = CorrectWords / TestTimer.Value; 
                int wpm = (int)Math.Floor(wps * 60);
                return wpm;
            }
        }

        public static double AverageWpm
        {
            get
            {
                double summaryWps = 0;
                for (int i = 0; i < StatHistory.Count; i++)
                {
                    summaryWps += StatHistory[i].Wpm;
                }
                return Math.Floor(summaryWps / StatHistory.Count);
            }
        }

        public static int Cpm
        {
            get
            {
                double cps = CorrectLetters / TestTimer.Value;
                int cpm = (int)Math.Floor(cps * 60);
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
