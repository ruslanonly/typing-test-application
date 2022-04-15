using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;


namespace TypingTestApp
{
    public class Colors
    {
        public static SolidColorBrush Mistake100 = new SolidColorBrush(Color.FromRgb(252, 210, 101));
        public static SolidColorBrush Mistake200 = new SolidColorBrush(Color.FromRgb(250, 163, 7));
        public static SolidColorBrush Mistake300 = new SolidColorBrush(Color.FromRgb(232, 93, 4));
        public static SolidColorBrush Mistake400 = new SolidColorBrush(Color.FromRgb(208, 0, 0));
        public static SolidColorBrush Mistake500 = new SolidColorBrush(Color.FromRgb(220, 47, 2));
        public static SolidColorBrush Incorrect = new SolidColorBrush(Color.FromRgb(248, 150, 30));
        public static SolidColorBrush LightFont = new SolidColorBrush(Color.FromRgb(130, 144, 148));
    }
    public class Word : StackPanel
    {
        public string Content;
        public bool isCorrect;
        public int Length
        {
            get
            {
                return Children.Count;
            }
        }

        public void Analyse()
        {
            bool res = true;
            for (int i = 0; i < Length; i++)
            {
                Letter letter = Children[i] as Letter;
                if (!letter.isCorrect)
                {
                    res = false;
                }
            }
            if (res)
            {
                isCorrect = true;
            } else
            {
                isCorrect = false;
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
            FontSize = 23;
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
            Foreground = Colors.Incorrect;
        }
    }
    public class TestOptionButton : Button
    {
        public bool isActive;
        public TestOptionButton() { }
        public void Active()
        {
            isActive = true;
            Style = (Style)FindResource("ActiveTestOptionButton");
        }
        public void Inactive()
        {
            isActive = false;
            Style = (Style)FindResource("TestOptionButton");
        }
    }
    public class WordGroupButton : TestOptionButton
    {
        public WordGroup wordGroup;
        public static WordGroupButton ActiveWordGroupButton;
        public WordGroupButton(WordGroup wordGroup) : base()
        {
            this.wordGroup = wordGroup;
            Content = wordGroup.ToString().ToLower();
            Click += onClick;
        }
        public void onClick(object obj, RoutedEventArgs e)
        {
            ActiveWordGroupButton.Inactive();
            ActiveWordGroupButton = this;
            Active();
            Config.wordGroup = wordGroup;
        }
    }
    public class WordAmountButton : TestOptionButton
    {
        public WordAmount wordAmount;
        public static WordAmountButton ActiveWordAmountButton;
        public WordAmountButton(WordAmount wordAmount) : base()
        {
            this.wordAmount = wordAmount;
            Content = (int)wordAmount;
            Click += onClick;
        }
        public void onClick(object obj, RoutedEventArgs e)
        {
            ActiveWordAmountButton.Inactive();
            ActiveWordAmountButton = this;
            Active();
            Config.wordAmount = wordAmount;
        }
    }

    public class KeyBlock : TextBlock
    {
        public string Character;
        private int _misClick = 0;
        private void SetColor()
        {
            if (_misClick == 0)
            {
                Foreground = Colors.LightFont;
                Parent.SetValue(Border.BorderBrushProperty, Colors.LightFont);
            } else
            {
                if (_misClick > 15)
                {
                    Foreground = Colors.Mistake500;
                    Parent.SetValue(Border.BorderBrushProperty, Colors.Mistake500);
                }
                else if (_misClick > 10)
                {
                    Foreground = Colors.Mistake400;
                    Parent.SetValue(Border.BorderBrushProperty, Colors.Mistake400);
                }
                else if (_misClick > 7)
                {
                    Foreground = Colors.Mistake300;
                    Parent.SetValue(Border.BorderBrushProperty, Colors.Mistake300);
                }
                else if (_misClick > 3)
                {
                    Foreground = Colors.Mistake200;
                    Parent.SetValue(Border.BorderBrushProperty, Colors.Mistake200);
                }
                else if (_misClick > 0)
                {
                    Foreground = Colors.Mistake100;
                    Parent.SetValue(Border.BorderBrushProperty, Colors.Mistake100);
                }
            }
        }
        public int IncorrectClicks
        {
            get
            {
                return _misClick;
            }

            set
            {

                _misClick = value;
                SetColor();
            }
        }
        public KeyBlock(string Key)
        {
            Character = Key;
            Text = Key;
            Height = 30;
            Width = 30;
        }
    }
}
