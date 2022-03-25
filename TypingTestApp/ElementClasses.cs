﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace TypingTestApp
{
    public class Word : StackPanel
    {
        public string Content;
        public bool isCorrect;
        public int Length
        {
            get
            {
                return this.Children.Count;
            }
        }

        public Word()
        {
            Margin = new Thickness(0, 0, 10, 0);
            Orientation = Orientation.Horizontal;
        }

    }

    public class Letter : TextBlock
    {
        public string Content;
        public bool isCorrect;
        public Letter(string content)
        {
            Text = content;
            Content = content;
            FontSize = 20;
            Foreground = new SolidColorBrush(Color.FromRgb(71, 83, 94));
        }
        public void Default()
        {
            Opacity = 1;
            Foreground = new SolidColorBrush(Color.FromRgb(71, 83, 94));
        }

        public void Correct()
        {
            isCorrect = true;
            Opacity = 0.5;
            Foreground = new SolidColorBrush(Color.FromRgb(235, 237, 245));

        }

        public void Incorrect()
        {
            isCorrect = false;
            Opacity = 1;
            Foreground = new SolidColorBrush(Color.FromRgb(248, 150, 30));
        }
    }
}