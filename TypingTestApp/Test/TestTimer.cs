using System.Timers;

namespace TypingTestApp
{
    public class TestTimer
    {
        public static double Value = 0;
        private static Timer timer;

        public static void Start()
        {
            timer = new Timer();
            timer.Interval = 100;
            timer.Elapsed += (object source, ElapsedEventArgs e) => {
                Value += 0.1;
            };
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
