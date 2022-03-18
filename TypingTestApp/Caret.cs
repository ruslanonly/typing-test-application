using System;
using System.ComponentModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Collections.Generic;
using System.Text;

namespace TypingTestApp
{
    
    public class Caret
    {
        public static double _left = 0;
        public static double _top = 0;
        public event PropertyChangedEventHandler PropertyChanged
        public static void MoveTo(Point target)
        {
            
            _left = target.X;
            _top = target.Y;
        }
    }
}
