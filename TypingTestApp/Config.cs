using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    enum TestMode
    {
        Words,
        Time,
        Quote
    }
    
    public enum TimeOption
    {
        Fifteen,
        Thirty,
        Minute,
        TwoMinutes
    }

    public enum Words
    {
        Ten,
        Thirty,
        Fifty,
        SeventyFive,
        Hundred
    }
    class Config
    {
        static public TestMode TestMode { get; set; }
        static public int Time;
        static public int Words;
        
        static public void InitConfig()
        {
            TestMode = TestMode.Words;
            Time = 60;
            Words = 10;
        }
    }
}
