using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    public class TestTimer
    {
        public static double Value;
        private static System.Timers.Timer timer;
        private static void IntervalHandler(Object source, System.Timers.ElapsedEventArgs e)
        {
            Value += 0.1;
        }

        public static void Start()
        {
            timer = new System.Timers.Timer();
            timer.Interval = 100;
            timer.Elapsed += IntervalHandler;
            timer.Start();
        }

        public static void Stop()
        {
            if (timer != null)
            {
                timer.Stop();
            }
        }

        public static void Reset()
        {
            Value = 0;
        }
    }
}
