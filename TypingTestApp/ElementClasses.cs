using System;
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

        public void Analyse()
        {
            bool res = true;
            for (int i = 0; i < this.Length; i++)
            {
                Letter letter = this.Children[i] as Letter;
                if (!letter.isCorrect)
                {
                    res = false;
                }
            }
            if (res) this.isCorrect = true;
            else this.isCorrect = false;
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
            Foreground = new SolidColorBrush(Color.FromRgb(248, 150, 30));
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
            if (ActiveWordGroupButton != null) ActiveWordGroupButton.Inactive();
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
            if (ActiveWordAmountButton != null) ActiveWordAmountButton.Inactive();
            ActiveWordAmountButton = this;
            Active();
            Config.wordAmount = wordAmount;
        }
    }
}
