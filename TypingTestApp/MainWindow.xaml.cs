using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace TypingTestApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            StartTest();
        }

        public string[] words = { 
            "add",
            "move",
            "it",
            "buy",
            "by",
            "word",
            "letter",
            "check",
            "as",
            "rise",
            "thought",

        };

        public void RenderText()
        {
            Random randomIndex = new Random();
            for(int i = 0; i < 25; i++)
            {
                var word = new Word(words[randomIndex.Next(0, words.Length)]);
                WordsBlock.Children.Add(word);
            }
        }

        public void StartTest()
        {
            RenderText();
            InputBlock.Focus();
        }

        public class Word : TextBlock
        {
            public string Content;
            public bool isCorrect;
            public Word(string content)
            {
                Text = content;
                Content = content;
                Padding = new Thickness(5, 5, 5, 5);
            }

            public void Correct()
            {
                isCorrect = true;
                Opacity = 0.5;
                Background = new SolidColorBrush();
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }

            public void Incorrect()
            {
                isCorrect = false;
                Foreground = new SolidColorBrush(Color.FromRgb(255, 0, 0));
            }

            public void Highlight()
            {
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 255));
            }
        }

        public int wordIndex = 0;
        public void SpaceHandler()
        {
            var word = WordsBlock.Children[wordIndex++] as Word;
            if (word.Content == InputBlock.Text.Trim(' '))
            {
                word.Correct();
            } else
            {
                word.Incorrect();
            }
            InputBlock.Clear();
            var nextWord = WordsBlock.Children[wordIndex] as Word;
            nextWord.Highlight();
        }

        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SpaceHandler();
            }
        }
    }
}
