using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Animation;
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
            Config.InitConfig();
            StartTest();
        }

        public void moveCaretTo(Point point)
        {
            SineEase easingFunction = new SineEase();
            easingFunction.EasingMode = EasingMode.EaseOut;
            DoubleAnimation LeftAnimation = new DoubleAnimation();
            LeftAnimation.To = point.X;
            LeftAnimation.Duration = TimeSpan.FromMilliseconds(120);
            DoubleAnimation TopAnimation = new DoubleAnimation();
            TopAnimation.To = point.Y;
            TopAnimation.Duration = TimeSpan.FromMilliseconds(120);
            CaretBlock.BeginAnimation(Canvas.LeftProperty, LeftAnimation);
            CaretBlock.BeginAnimation(Canvas.TopProperty, TopAnimation);
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
            WordsBlock.Children.Clear();
            Random randomIndex = new Random();
            for (int i = 0; i < 10; i++)
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

        public void RestartTest()
        {
            TestState.Reset();
            StopTest();
            moveCaretTo(getLetterPoint(0, 0));
            StartTest();
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

            public void AnimateColor(SolidColorBrush color)
            {
                ColorAnimation animation;
                animation = new ColorAnimation();
                animation.To = color.Color;
                animation.Duration = new Duration(TimeSpan.FromMilliseconds(10));
                Foreground.BeginAnimation(SolidColorBrush.ColorProperty, animation);
            }
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
                AnimateColor(new SolidColorBrush(Color.FromRgb(71, 83, 94)));
            }

            public void Correct()
            {
                isCorrect = true;
                Opacity = 0.5;
                AnimateColor(new SolidColorBrush(Color.FromRgb(235, 237, 245)));
            }

            public void Incorrect()
            {
                isCorrect = false;
                Opacity = 1;
                AnimateColor(new SolidColorBrush(Color.FromRgb(248, 150, 30)));
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

        public Point getLetterPoint(int wordI, int letterI, bool addLetterWidth = false)
        {
            Vector letterVector = VisualTreeHelper.GetOffset(getLetter(letterI));
            Vector wordVector = VisualTreeHelper.GetOffset(getWord(wordI));
            double addX = addLetterWidth ? getLetter(letterI).ActualWidth : 0;
            return new Point(letterVector.X + wordVector.X + addX, wordVector.Y);
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
                    moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
                } else if (TestState.LetterIndex < getWord().Length && !isWordBeginning)
                {
                    for (int i = TestState.LetterIndex; i < getWord().Length; i++)
                    {
                        Letter letter = getWord().Children[i] as Letter;
                        letter.Incorrect();
                    }
                    TestState.WordIndex++;
                    TestState.LetterIndex = 0;
                    moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
                }
            }
        }

        public void BackSpaceHandler()
        {
            bool isWordBeginning = TestState.LetterIndex == 0;
            if (!isWordBeginning)
            {
                TestState.LetterIndex--;
                if (!getLetter().isCorrect)
                {
                    TestState.CorrectLetters--;
                }

                int index = TestState.LetterIndex;
                getLetter(index).Default();
                if (getLetter(index).isCorrect)
                {
                    getLetter(index).isCorrect = false;
                }
                moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));

            } else
            {
                TestState.WordIndex--;
                TestState.LetterIndex = getWord().Length - 1;
                moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex, true));
            }
        }

        public void RegularKeyHandler(string key)
        {
            if (TestState.LetterIndex < getWord().Length)
            {
                if (key == getLetter().Content)
                {
                    getLetter().Correct();
                    TestState.CorrectLetters++;
                }
                else
                {
                    getLetter().Incorrect();
                }
                TestState.LetterIndex++;
            }
            if (TestState.LetterIndex < getWord().Length)
            {
                moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
            } else
            {
                moveCaretTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex - 1, true));
            }
            TestState.PressedKeys++;
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R && (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))
            {
                RestartTest();
            } else
            {
                if (e.Key == Key.Space)
                {
                    SpaceHandler();
                }
                else if (e.Key == Key.Back)
                {
                    bool isFirstLetter = TestState.WordIndex == 0 && TestState.LetterIndex == 0;
                    if (!isFirstLetter)
                    {
                        BackSpaceHandler();
                    }
                }
                else if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    RegularKeyHandler(e.Key.ToString().ToLower());
                    bool isLastLetter = TestState.WordIndex == Config.Words - 1 && TestState.LetterIndex == getWord().Length;
                    if (isLastLetter) RestartTest();
                }
            }
        }
    }
}
