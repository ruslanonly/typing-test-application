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
        public static SolidColorBrush LightFont = new SolidColorBrush(Color.FromRgb(130, 144, 148));
        public static SolidColorBrush DefaultLetter = new SolidColorBrush(Color.FromRgb(71, 83, 94));
        public static SolidColorBrush CorrectLetter = new SolidColorBrush(Color.FromRgb(235, 237, 245));
        public static SolidColorBrush IncorrectLetter = new SolidColorBrush(Color.FromRgb(248, 150, 30));
        public static SolidColorBrush MainBg = new SolidColorBrush(Color.FromRgb(38, 46, 54));
        public static SolidColorBrush[] SecondaryBg = { new SolidColorBrush(Color.FromRgb(43, 50, 59)), new SolidColorBrush(Color.FromRgb(34, 41, 49)) };
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

        public void Default()
        {
            for(int i = 0; i < Children.Count; i++)
            {
                Letter letter = Children[i] as Letter;
                letter.Default();
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
            Foreground = Colors.DefaultLetter;
            FontFamily = MainWindow.MainFontFamily;
        }
        public void Default()
        {
            Opacity = 1;
            Foreground = Colors.DefaultLetter;
        }

        public void Correct()
        {
            isCorrect = true;
            Opacity = 0.5;
            Foreground = Colors.CorrectLetter;

        }

        public void Incorrect()
        {
            isCorrect = false;
            Opacity = 1;
            Foreground = Colors.IncorrectLetter;
        }
    }
    public class TestOptionButton : Button
    {
        public bool isActive;
        public TestOptionButton()
        {
            FontFamily = MainWindow.MainFontFamily;
        }
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
            Click += (object obj, RoutedEventArgs e) => {
                ActiveWordGroupButton.Inactive();
                ActiveWordGroupButton = this;
                Active();
                Config.wordGroup = wordGroup;
            };
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
            Click += (object obj, RoutedEventArgs e) => {
                ActiveWordAmountButton.Inactive();
                ActiveWordAmountButton = this;
                Active();
                Config.wordAmount = wordAmount;
            };
        }
    }

    public class KeyBlock : TextBlock
    {
        public string Character;
        private int _misClick = 0;
        public int IncorrectClicks
        {
            get
            {
                return _misClick;
            }

            set
            {
                _misClick = value;
                OnMisClickChange();
            }
        }
        private void SetColor(SolidColorBrush color)
        {
            Foreground = color;
            Parent.SetValue(Border.BorderBrushProperty, color);
        }
        private void OnMisClickChange()
        {
            if (_misClick == 0)
            {
                SetColor(Colors.LightFont);
            } else
            {
                if (_misClick > 15)
                {
                    SetColor(Colors.Mistake500);
                }
                else if (_misClick > 10)
                {
                    SetColor(Colors.Mistake400);
                }
                else if (_misClick > 7)
                {
                    SetColor(Colors.Mistake300);
                }
                else if (_misClick > 3)
                {
                    SetColor(Colors.Mistake200);
                }
                else if (_misClick > 0)
                {
                    SetColor(Colors.Mistake100);
                }
            }
        }
        public KeyBlock(string Key)
        {
            Character = Key;
            Text = Key;
            Height = 30;
            Width = 30;
            FontFamily = MainWindow.MainFontFamily;
        }

    }
    public class HistoryItem : Grid
    {
        public HistoryItem(Stat stat, int number)
        {
            HorizontalAlignment = HorizontalAlignment.Stretch;
            Margin = new Thickness(0, 0, 0, 5);
            for (int i = 0; i < 5; i++)
            {
                ColumnDefinitions.Add(new ColumnDefinition());

            }
            RowDefinitions.Add(new RowDefinition());
            for (int i = 0; i < 5; i++)
            {
                Children.Add(GetTextBlock(number.ToString(), 0));
                Children.Add(GetTextBlock(stat.Wpm.ToString(), 1));
                Children.Add(GetTextBlock(stat.Cpm.ToString(), 2));
                TextBlock accBlock = GetTextBlock(stat.Accuracy.ToString() + "%", 3);
                if (stat.Accuracy < 50)
                {
                    Opacity = 0.5;
                }
                Children.Add(accBlock);
                Children.Add(GetTextBlock(stat.Time.ToString() + "s", 4));
            }
        }
        public TextBlock GetTextBlock(string content, int column)
        {
            TextBlock block = new TextBlock();
            block.Text = content;
            block.TextAlignment = TextAlignment.Center;
            block.Foreground = Colors.LightFont;
            block.Padding = new Thickness(7);
            Grid.SetColumn(block, column);
            return block;
        }
    }
}
