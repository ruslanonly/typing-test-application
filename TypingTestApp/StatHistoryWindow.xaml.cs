using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace TypingTestApp
{
    /// <summary>
    /// Логика взаимодействия для StatHistoryWindow.xaml
    /// </summary>
    public partial class StatHistoryWindow : Window
    {
        public StatHistoryWindow(List<Stat> StatHistory)
        {
            InitializeComponent();
            LoadStatHistory(StatHistory);
        }

        public class HistoryItem : Grid
        {
            public static int Count = 1;
            public HistoryItem(Stat stat)
            {
                HorizontalAlignment = HorizontalAlignment.Stretch;
                Margin = new Thickness(0, 0, 0, 10);
                for (int i = 0; i < 5; i++)
                {
                    ColumnDefinitions.Add(new ColumnDefinition());

                }
                RowDefinitions.Add(new RowDefinition());
                for (int i = 0; i < 5; i++)
                {
                    Children.Add(GetTextBlock(Count.ToString(), 0));
                    Children.Add(GetTextBlock(stat.Wpm.ToString(), 1));
                    Children.Add(GetTextBlock(stat.Cpm.ToString(), 2));
                    TextBlock accBlock = GetTextBlock(stat.Accuracy.ToString() + "%", 3);
                    if(stat.Accuracy < 50)
                    {
                        accBlock.Foreground = Colors.Mistake300;
                        Background = Colors.Mistake100;
                        Opacity = 0.7;
                    }
                    Children.Add(accBlock);
                    Children.Add(GetTextBlock(stat.Time.ToString() + "s", 4));
                }
                Count++;
            }
            public TextBlock GetTextBlock(string content, int column)
            {
                TextBlock block = new TextBlock();
                block.Text = content;
                block.TextAlignment = TextAlignment.Center;
                block.Foreground = Colors.LightFont;
                Grid.SetColumn(block, column);
                return block;
            }
        }

        public void LoadStatHistory(List<Stat> StatHistory)
        {
            foreach (Stat stat in StatHistory)
            {
                HistoryItem historyItem = new HistoryItem(stat);
                StatHistoryBlock.Children.Add(historyItem);
            }
        }
    }
}
