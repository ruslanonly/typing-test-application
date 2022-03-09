using System;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    class TestState
    {
        public static int wordIndex = 0;
        public static int correctWords = 0;

        public static void Reset()
        {
            wordIndex = 0;
            correctWords = 0;
        }
    }
}
