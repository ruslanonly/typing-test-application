using System;
using System.Collections.Generic;
using System.Text;
using System.Windows;
using System.Linq;
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
            LoadSortingCriteriaButtons();
        }

        public enum SortingCriteria
        {
            Wpm,
            Accuracy,
            Time
        }

        public delegate void SCClickHandler();
        public class SortingCriteriaButton : Button
        {
            public static SortingCriteriaButton Current;
            public SortingCriteria sortingCriteria;
            public void Active()
            {
                Style = (Style)FindResource("ActiveSortingCriteriaButton");
            }

            public void Default()
            {
                Style = (Style)FindResource("SortingCriteriaButton");
            }

            public SortingCriteriaButton(SortingCriteria sortingCriteria, SCClickHandler clickHandler)
            {
                Content = sortingCriteria.ToString();
                this.sortingCriteria = sortingCriteria;
                Click += (object obj, RoutedEventArgs e) =>
                {
                    if (Current != null) Current.Default();
                    Active();
                    Current = this;
                    CurrentSortingCriteria = this.sortingCriteria;
                    DoStatHistorySorting = true;
                    clickHandler();
                };
            }
        }

        public void LoadSortingCriteriaButtons()
        {
            Array SortingCriterias = Enum.GetValues(typeof(SortingCriteria));
            int columnCounter = 1;
            foreach (SortingCriteria sc in SortingCriterias)
            {
                SCClickHandler clickHandler = () =>
                {
                    StatHistoryBlock.Children.Clear();
                    LoadStatHistory(CurrentStatHistory);
                };
                SortingCriteriaButton button = new SortingCriteriaButton(sc, clickHandler);
                Grid.SetColumn(button, columnCounter++);
                SortingsCriteriaButtons.Children.Add(button);
                button.Default();
            }
        }

        public List<Stat> HandleStatHistory(List<Stat> StatHistory, SortingCriteria sortingCriteria)
        {
            IEnumerable<Stat> statEnumerable = StatHistory;
            switch (sortingCriteria)
            {
                case SortingCriteria.Wpm:
                    {
                        statEnumerable = from stat in statEnumerable
                                         orderby stat.Wpm descending
                                         select stat;
                        break;
                    }
                case SortingCriteria.Accuracy:
                    {
                        statEnumerable = from stat in statEnumerable
                                         orderby stat.Accuracy descending
                                         select stat;
                        break;
                    }
                case SortingCriteria.Time:
                    {
                        statEnumerable = from stat in statEnumerable
                                         orderby stat.Time
                                         select stat;
                        break;
                    }
            }
            return statEnumerable.ToList();
        }

        public List<Stat> CurrentStatHistory;
        static public SortingCriteria CurrentSortingCriteria = SortingCriteria.Wpm;
        static public bool DoStatHistorySorting = false;
        public void LoadStatHistory(List<Stat> StatHistory)
        {
            CurrentStatHistory = StatHistory;
            List<Stat> HandledStatHistory;
            if (DoStatHistorySorting)
            {
                HandledStatHistory = HandleStatHistory(StatHistory, CurrentSortingCriteria);
            } else
            {
                HandledStatHistory = StatHistory;
            }

            int Count = 1;
            foreach (Stat stat in HandledStatHistory)
            {
                HistoryItem historyItem = new HistoryItem(stat, Count++);
                historyItem.Background = Colors.SecondaryBg[Count % 2];
                StatHistoryBlock.Children.Add(historyItem);
            }
        }
    }
}
