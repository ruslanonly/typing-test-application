using System;
using System.IO;
using System.Text.Json;
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
        public async void InitApp()
        {
            InitializeComponent();
            Config.InitConfig();
            await StartTest();
            caret = new Caret(CaretBlock);
            WordsBlock.SetValue(WrapPanel.HeightProperty, getLetter().Height * 3);
        }
        public MainWindow()
        {
            InitApp();
        }

        public enum WordGroup
        {
            Standart,
            Medium,
            Hard
        }

        public UIElementCollection CurrentWordCollection;
        public WordGroup currentWordGroup = WordGroup.Medium;

        public async ValueTask<string[]> LoadWordsGroup(WordGroup wordGroup)
        {
            try
            {
                using (StreamReader sr = new StreamReader("../../../Assets/" + wordGroup.ToString() + ".json"))
                {
                    return await JsonSerializer.DeserializeAsync<string[]>(sr.BaseStream);
                }
            } catch(Exception exp)
            {
                MessageBox.Show(exp.Message);
                using (StreamReader sr = new StreamReader("../../../Assets/Standart.json"))
                {
                    return await JsonSerializer.DeserializeAsync<string[]>(sr.BaseStream);
                }
            }
        }
        public async Task RenderText()
        {
            string[] words = await LoadWordsGroup(currentWordGroup);
            WordsBlock.Children.Clear();
            if (repeatTest)
            {
                for(int i = 0; i < CurrentWordCollection.Count; i++)
                {
                    WordsBlock.Children.Add(CurrentWordCollection[i]);
                }
            } else
            {
                Random randomIndex = new Random();
                for (int i = 0; i < Config.Words; i++)
                {
                    int indexInArray = randomIndex.Next(0, words.Length);
                    string wordContent = words[indexInArray];
                    Word word = new Word();
                    for (int j = 0; j < wordContent.Length; j++)
                    {
                        Letter letter = new Letter(Convert.ToString(wordContent[j]));
                        word.Children.Add(letter);
                    }
                    WordsBlock.Children.Add(word);
                }
                CurrentWordCollection = WordsBlock.Children;
            }
        }

        public bool isWaitingForTest = true;
        public bool repeatTest = false;
        public Caret caret;
        public async Task StartTest()
        {
            await RenderText();
            isWaitingForTest = true;
            caret = new Caret(CaretBlock);
            caret.MoveTo(getLetterPoint(0, 0));
        }

        public void RestartTest()
        {
            StopTest();
            StartTest();
            TestState.Reset();
            TestStats.Reset();
            TestTimer.Reset();
            caret.MoveTo(getLetterPoint(0, 0));
            WordsBlock.SetValue(WrapPanel.HeightProperty, getLetter().Height * 3);

        }

        static public void StopTest()
        {
            TestTimer.Stop();
        }

        public void ResetButtonClickHandler(object sender, RoutedEventArgs e)
        {
            RestartTest();
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
            return new Point(letterVector.X + wordVector.X + addX, wordVector.Y + 30);
        }

        public void moveCaretTo(Point point)
        {
            SineEase easingFunction = new SineEase();
            easingFunction.EasingMode = EasingMode.EaseOut;
            DoubleAnimation LeftAnimation = new DoubleAnimation();
            LeftAnimation.To = point.X;
            LeftAnimation.Duration = TimeSpan.FromMilliseconds(90);
            DoubleAnimation TopAnimation = new DoubleAnimation();
            TopAnimation.To = point.Y;
            TopAnimation.Duration = TimeSpan.FromMilliseconds(90);
            CaretBlock.BeginAnimation(Canvas.LeftProperty, LeftAnimation);
            CaretBlock.BeginAnimation(Canvas.TopProperty, TopAnimation);
        }

        public void SpaceHandler()
        {
            bool isWordBeginning = TestState.LetterIndex == 0;
            if (TestState.LetterIndex == getWord().Length)
            {
                Word writtenWord = getWord();
                writtenWord.Analyse();
                if (writtenWord.isCorrect) TestStats.CorrectWords++;
                TestState.WordIndex++;
                TestState.LetterIndex = 0;
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
            } else if (TestState.LetterIndex < getWord().Length && !isWordBeginning)
            {
                Word skippedWord = getWord();
                for (int i = TestState.LetterIndex; i < skippedWord.Length; i++)
                {
                    Letter letter = skippedWord.Children[i] as Letter;
                    letter.Incorrect();
                }
                skippedWord.isCorrect = false;
                TestState.WordIndex++;
                TestState.LetterIndex = 0;
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
            }
        }

        public void BackSpaceHandler()
        {
            bool isWordBeginning = TestState.LetterIndex == 0;
            if (Keyboard.IsKeyDown(Key.LeftCtrl))
            {
                if (!isWordBeginning)
                {
                    for (int i = TestState.LetterIndex; i >= 0; i--)
                    {
                        Letter letter = getLetter(i);
                        if (letter.isCorrect)
                        {
                            letter.isCorrect = false;
                            TestStats.CorrectLetters--;
                        }
                        letter.Default();
                    }
                    TestState.LetterIndex = 0;
                    caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));

                }
                else
                {
                    TestState.WordIndex--;
                    Word word = getWord();
                    if (word.isCorrect)
                    {
                        word.isCorrect = false;
                        TestStats.CorrectWords--;
                    }
                    for (int i = word.Length - 1; i >= 0; i--)
                    {
                        Letter letter = word.Children[i] as Letter;
                        if (letter.isCorrect)
                        {
                            letter.isCorrect = false;
                            TestStats.CorrectLetters--;
                        }
                        letter.Default();
                    }
                    TestState.LetterIndex = 0;
                    caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
                }
            } else
            {
                if (!isWordBeginning)
                {
                    TestState.LetterIndex--;
                    Letter letter = getLetter();
                    if (letter.isCorrect)
                    {
                        letter.isCorrect = false;
                        TestStats.CorrectLetters--;
                    }
                    letter.Default();
                    caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));

                }
                else
                {
                    TestState.WordIndex--;
                    Word word = getWord();
                    if (word.isCorrect)
                    {
                        word.isCorrect = false;
                        TestStats.CorrectWords--;
                    }
                    TestState.LetterIndex = word.Length;
                    caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex - 1, true));
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
                    TestStats.CorrectLetters++;
                }
                else
                {
                    getLetter().Incorrect();
                }
                TestState.LetterIndex++;
            }
            if (TestState.LetterIndex < getWord().Length)
            {
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
            } else
            {
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex - 1, true));
            }
            TestStats.PressedKeys++;
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
                    if (TestState.WordIndex < Config.Words - 1)
                    {
                        SpaceHandler();
                    }
                }
                else if (e.Key == Key.Back)
                {
                    bool isFirstLetter = TestState.WordIndex == 0 && TestState.LetterIndex == 0;
                    if (!isFirstLetter)
                    {
                        if (!e.IsRepeat)
                        {
                            BackSpaceHandler();
                        }
                    }
                }
                else if (e.Key >= Key.A && e.Key <= Key.Z)
                {
                    if (isWaitingForTest)
                    {
                        TestTimer.Start();
                        isWaitingForTest = false;
                    }
                    RegularKeyHandler(e.Key.ToString().ToLower());
                    bool isLastLetter = TestState.WordIndex == Config.Words - 1 && TestState.LetterIndex == getWord().Length;
                    if (isLastLetter)
                    {
                        RestartTest();
                    }
                }
            }
        }
    }
}
