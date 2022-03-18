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
            "thought"
        };

        public UIElementCollection CurrentWordCollection;
        public void RenderText()
        {

            Random randomIndex = new Random();
            for (int i = 0; i < 25; i++)
            {
                string wordContent = words[randomIndex.Next(0, words.Length)];
                Word word = new Word();
                for(int j = 0; j < wordContent.Length; j++)
                {
                    Letter letter = new Letter(Convert.ToString(wordContent[j]));
                    word.Children.Add(letter);
                }
                WordsBlock.Children.Add(word);
            }
            CurrentWordCollection = WordsBlock.Children;

        }

        public void StartTest()
        {
            RenderText();
            TestTimer.Start();
        }

        static public void StopTest()
        {
            TestTimer.Stop();
        }

        public class TestTimer
        {
            public static double Value;
            private static System.Timers.Timer timer;
            private static void IntervalStepHandler(Object source, System.Timers.ElapsedEventArgs e)
            {
                Value += 0.1;
            }

            public static void Start()
            {
                timer = new System.Timers.Timer();
                timer.Interval = 100;
                timer.Elapsed += IntervalStepHandler;
                timer.Start();
            }

            public static void Stop()
            {
                timer.Stop();
            }

            public static void Reset()
            {
                Value = 0;
            }
        }

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
                Foreground = new SolidColorBrush(Color.FromRgb(0, 0, 0));
            }

            public void Correct()
            {
                isCorrect = true;
                Opacity = 0.5;
                Foreground = new SolidColorBrush(Color.FromRgb(235, 237, 245));
                TestState.CorrectWords++;
            }

            public void Incorrect()
            {
                isCorrect = false;
                Foreground = new SolidColorBrush(Color.FromRgb(248, 150, 30));
            }
        }

        public Word getWord(int index = -1)
        {
            if (index == -1)
            {
                index = TestState.WordIndex;
            }
            return WordsBlock.Children[index] as Word;
        }

        public Letter getLetter(int index = -1)
        {
            if (index == -1)
            {
                index = TestState.LetterIndex;
            }
            return getWord().Children[index] as Letter;
        }

        public Point getLetterPoint()
        {
            Vector letterVector = VisualTreeHelper.GetOffset(getLetter(TestState.LetterIndex));
            Vector wordVector = VisualTreeHelper.GetOffset(getWord(TestState.WordIndex));
            return new Point(letterVector.X + wordVector.X, wordVector.Y);
        }

        public void SpaceHandler()
        {
            bool isLastWord = TestState.WordIndex + 1 == Config.Words;
            bool isWordBeginning = TestState.LetterIndex == 0;
            if (!isLastWord)
            {
                if (TestState.LetterIndex == getWord().Length)
                {
                    TestState.WordIndex++;
                    TestState.LetterIndex = 0;
                } else if (TestState.LetterIndex < getWord().Length && !isWordBeginning)
                {
                    for (int i = TestState.LetterIndex; i < getWord().Length; i++)
                    {
                        Letter letter = getWord().Children[i] as Letter;
                        letter.Incorrect();
                    }
                    TestState.WordIndex++;
                    TestState.LetterIndex = 0;
                }
            }
        }

        public void RegularKeyHandler(string key)
        {
            if (TestState.LetterIndex < getWord().Length)
            {
                if (key == getLetter().Content)
                {
                    getLetter().Correct();

                }
                else
                {
                    getLetter().Incorrect();
                }
                Caret.MoveTo(getLetterPoint());
                TestState.LetterIndex++;
            }
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Space)
            {
                SpaceHandler();
                
            } else
            {
                RegularKeyHandler(e.Key.ToString().ToLower());
            }
        }
    }
}
