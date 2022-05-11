using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    public enum WordAmount
    {
        One = 3,
        Ten = 10,
        Twenty = 20,
        Thirty = 30,
        Fifty = 50,
        SeventyFive = 75
    }
    public enum WordGroup
    {
        English,
        Russian
    }

    class Config
    {
        static public int Words;
        static public WordGroup wordGroup;
        static public WordAmount wordAmount;
        
        static public void InitConfig()
        {
            Words = 10;
            wordGroup = WordGroup.English;
            wordAmount = WordAmount.Thirty;
        }
    }
}
