using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    public enum WordAmount
    {
        Ten = 10,
        Twenty = 20,
        Thirty = 30,
        Fifty = 50,
        SeventyFive = 75
    }
    public enum WordGroup
    {
        Simple,
        Medium,
        Hard,
    }

    class Config
    {
        static public int Words;
        static public WordGroup wordGroup;
        static public WordAmount wordAmount;
        
        static public void InitConfig()
        {
            Words = 10;
            wordGroup = WordGroup.Simple;
            wordAmount = WordAmount.Thirty;
        }
    }
}
