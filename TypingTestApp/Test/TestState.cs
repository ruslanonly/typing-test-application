using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    class TestState
    {
        public static int WordIndex = 0;
        public static int LetterIndex = 0;
        public static bool RepeatTest = false;

        public static void Reset()
        {
            WordIndex = 0;
            LetterIndex = 0;
        }
    }
}
