using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
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
        static public int Words;
        
        static public void InitConfig()
        {
            Words = 15;
        }
    }
}
