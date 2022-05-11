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
using System.Windows.Input;
using System.Windows.Media;
using System.Threading;

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
            LoadTestOptions();
            LoadKeyMap();
            await StartTest();
            caret = new Caret(CaretBlock);
            Thread.Sleep(250);
            Animate.FadeIn(TestWrapper);
        }
        public MainWindow()
        {
            InitApp();
        }

        public void DisplayTestStats()
        {
            Stat stat = TestStats.GetCurrentStat();
            WPMValue.Text = Convert.ToString(stat.Wpm);
            CPMValue.Text = Convert.ToString(stat.Cpm);
            AccuracyValue.Text = Convert.ToString(stat.Accuracy) + "%";
            TestStats.StatHistory.Add(stat);
            AverageWPMValue.Text = Convert.ToString(TestStats.AverageWpm);

            if (StatsBlock.Opacity != 1)
            {
                Animate.FadeIn(StatsBlock);
            }
        }

        public string[][] keyMap = { 
            new string[] { "Q", "W", "E", "R", "T", "Y", "U", "I", "O", "P" },
            new string[] { "A", "S", "D", "F", "G", "H", "J", "K", "L" },
            new string[] { "Z", "X", "C", "V", "B", "N", "M" },
        };
        Dictionary<string, KeyBlock> keysDictionary = new Dictionary<string, KeyBlock>();
        public void LoadKeyMap()
        {
            for(int i = 0; i < keyMap.Length; i++)
            {
                string[] keyChars = keyMap[i];
                int column = 0;
                foreach (string ch in keyChars)
                {
                    KeyBlock key = new KeyBlock(ch);
                    key.Style = (Style)FindResource("KeyMapItem");
                    keysDictionary.Add(ch, key);
                    Border border = new Border();
                    border.Style = (Style)FindResource("KeyMapItemBorder");
                    border.Child = key;
                    border.SetValue(Grid.RowProperty, i);
                    border.SetValue(Grid.ColumnProperty, column);
                    column++;
                    KeymapBlock.Children.Add(border);
                }
            }
        }

        public void LoadTestOptions()
        {
            Array WordGroupValues = Enum.GetValues(typeof(WordGroup));
            foreach (WordGroup wg in WordGroupValues)
            {
                WordGroupButton button = new WordGroupButton(wg);
                WordGroupOptions.Children.Add(button);
                if (Config.wordGroup == wg)
                {
                    button.Active();
                    WordGroupButton.ActiveWordGroupButton = button;
                } else
                {
                    button.Inactive();
                }
                button.Click += (object obj, RoutedEventArgs e) => {
                    RestartTest();
                };
            }

            Array WordAmountValues = Enum.GetValues(typeof(WordAmount));
            foreach (WordAmount wa in WordAmountValues)
            {
                WordAmountButton button = new WordAmountButton(wa);
                WordAmountOptions.Children.Add(button);
                if (Config.wordAmount == wa)
                {
                    button.Active();
                    WordAmountButton.ActiveWordAmountButton = button;
                } else
                {
                    button.Inactive();
                }
                button.Click += (object obj, RoutedEventArgs e) => {
                    RestartTest();
                };
            }
        }
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
                using (StreamReader sr = new StreamReader("../../../Assets/English.json"))
                {
                    return await JsonSerializer.DeserializeAsync<string[]>(sr.BaseStream);
                }
            }
        }
        public Word[] CurrentWordCollection;
        public Word[] UIElementCollectionToWordArray(UIElementCollection collection)
        {
            Word[] array = new Word[collection.Count];
            for (int i = 0; i < collection.Count; i++)
            {
                Word word = (Word)collection[i];
                word.Default();
                array[i] = word;

            }
            return array;
        }
        public async Task RenderText()
        {
            string[] words = await LoadWordsGroup(Config.wordGroup);
            if (Config.wordGroup == WordGroup.Russian)
            {
                IEnumerable<string> array = from word in words
                              where !(word.Contains("б") || word.Contains("ю") || word.Contains("э") || word.Contains("ж") || word.Contains("х") || word.Contains("ъ"))
                              select word;
                words = array.ToArray();
            }
            WordsBlock.Children.Clear();
            if (TestState.RepeatTest)
            {
                for (int i = 0; i < CurrentWordCollection.Length; i++)
                {
                    Word word = (Word)CurrentWordCollection[i];
                    WordsBlock.Children.Add(word);
                }
                TestState.RepeatTest = false;
            } else
            {
                Random randomIndex = new Random();
                int wordAmount = (int)Config.wordAmount;
                for (int i = 0; i < wordAmount; i++)
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
            }
        }

        public bool isWaitingForTest = true;
        public Caret caret;
        public async Task StartTest()
        {
            await RenderText();
            CurrentWordCollection = UIElementCollectionToWordArray(WordsBlock.Children);
            isWaitingForTest = true;
            caret = new Caret(CaretBlock);
            caret.MoveTo(getLetterPoint(0, 0));
        }

        public async void RestartTest()
        {
            await StartTest();
            TestState.Reset();
            TestStats.Reset();
            TestTimer.Stop();
            TestTimer.Reset();
            caret.MoveTo(getLetterPoint(0, 0));
            WordsBlock.SetValue(WrapPanel.HeightProperty, getLetter().Height * 3);
        }

        public void StopTest()
        {
            DisplayTestStats();
        }

        public void ResetButtonClickHandler(object sender, RoutedEventArgs e)
        {
            TestState.RepeatTest = true;
            RestartTest();
        }

        public void ContinueButtonClickHandler(object sender, RoutedEventArgs e)
        {
            RestartTest();
        }

        public void InfoButtonClickHandler(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Tab to Continue. It renders new text.\nCtrl + R to Repeat test with the same words.", "Information");

        }

        public void ResetKeyMapButtonClickHandler(object sender, RoutedEventArgs e)
        {
            foreach (KeyBlock key in keysDictionary.Values)
            {
                key.IncorrectClicks = 0;
            }
        }

        public void StatHistoryClickHandler(object obj, RoutedEventArgs e)
        {
            StatHistoryWindow statHistoryWindow = new StatHistoryWindow(TestStats.StatHistory);
            statHistoryWindow.Show();
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
            int LetterMargin = 30;
            return new Point(letterVector.X + wordVector.X + addX, wordVector.Y + LetterMargin);
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
                    if (TestState.LetterIndex == getWord().Length) --TestState.LetterIndex;
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

        public Dictionary<string, string> TranslatedKeys = new Dictionary<string, string>
        {
            { "q", "й" }, { "w", "ц" }, { "e", "у" }, { "r", "к" }, { "t", "е" }, { "y", "н" }, { "u", "г" }, { "i", "ш" }, { "o", "щ" }, { "p", "з" },
            { "a", "ф" }, { "s", "ы" }, { "d", "в" }, { "f", "а" }, { "g", "п" }, { "h", "р" }, { "j", "о" }, { "k", "л" }, { "l", "д" },
            { "z", "я" }, { "x", "ч" }, { "c", "с" }, { "v", "м" }, { "b", "и" }, { "n", "т" }, { "m", "ь" }, { ",", "б" },
        };

        public void RegularKeyHandler(string key)
        {
            if (Config.wordGroup == WordGroup.Russian) key = TranslatedKeys[key];
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
                    if (Config.wordGroup == WordGroup.English)
                    keysDictionary[getLetter().Content.ToUpper()].IncorrectClicks++;
                }
                TestState.LetterIndex++;
                TestStats.PressedKeys++;
            }
            if (TestState.LetterIndex < getWord().Length)
            {
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex));
            } else
            {
                caret.MoveTo(getLetterPoint(TestState.WordIndex, TestState.LetterIndex - 1, true));
            }
        }
        private void KeyDownHandler(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.R && (e.KeyboardDevice.IsKeyDown(Key.LeftCtrl) || e.KeyboardDevice.IsKeyDown(Key.RightCtrl)))
            {
                TestState.RepeatTest = true;
                RestartTest();
            } else if (e.Key == Key.Tab)
            {
                RestartTest();
            } else
            {
                if (e.Key == Key.Space)
                {
                    if (TestState.WordIndex < (int)Config.wordAmount - 1)
                    {
                        SpaceHandler();
                    } else
                    {
                        bool isLastLetter = TestState.WordIndex == (int)Config.wordAmount - 1 && TestState.LetterIndex == getWord().Length;
                        if (isLastLetter)
                        {
                            Word word = getWord();
                            word.Analyse();
                            if (word.isCorrect) TestStats.CorrectWords++;
                            StopTest();
                            RestartTest();
                        }
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
                }
            }
        }
    }
}
