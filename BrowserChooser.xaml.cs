using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

using Microsoft.Win32;

namespace Browser_Chooser
{
    /// <summary>
    /// Interaction logic for Settings.xaml
    /// </summary>
    public partial class BrowserChooser : Window
    {
        ObservableCollection<BrowserRegex> browserRegexen;

        BrowserRegexSettings settings = new BrowserRegexSettings();

        public BrowserChooser()
        {
            if (settings.BrowserRegexen == null || settings.BrowserRegexen.Count == 0)
            {
                browserRegexen = new ObservableCollection<BrowserRegex>();
                browserRegexen.Add(new BrowserRegex { Regex = ".*", Browser = Browser.Default });
            }
            else
            {
                browserRegexen = new ObservableCollection<BrowserRegex>(settings.BrowserRegexen);
            }

            InitializeComponent();
        }

        public ObservableCollection<BrowserRegex> BrowserRegexen
        {
            get { return browserRegexen; }
        }

        public ObservableCollection<string> Browsers
        {
            get { return Browser.Browsers; }
        }

        private void add_Click(object sender, RoutedEventArgs e)
        {
            browserRegexen.Add(new BrowserRegex { Regex = ".*", Browser = Browser.Default });
            listview.SelectedItem = browserRegexen[browserRegexen.Count - 1];
        }

        private void delete_Click(object sender, RoutedEventArgs e)
        {
            browserRegexen.Remove((BrowserRegex)listview.SelectedItem);
        }

        private void setAsDefault_Click(object sender, RoutedEventArgs e)
        {
            Browser.SetAsDefault();
		}

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            settings.BrowserRegexen = new List<BrowserRegex>(browserRegexen);
            settings.Save();
        }

        private void listview_MouseDown(object sender, MouseButtonEventArgs e)
        {
            (sender as ListView).SelectedItem = null;
        }

        private void moveUp_Click(object sender, RoutedEventArgs e)
        {
            int index = listview.SelectedIndex;

            if (index != 0)
            {
                var browserRegex = (BrowserRegex)listview.SelectedItem;
                browserRegexen.RemoveAt(index);
                browserRegexen.Insert(index - 1, browserRegex);
                listview.SelectedIndex = index - 1;
            }
        }

        private void moveDown_Click(object sender, RoutedEventArgs e)
        {
            int index = listview.SelectedIndex;

            if (index + 1 != browserRegexen.Count)
            {
                var browserRegex = (BrowserRegex)listview.SelectedItem;
                browserRegexen.RemoveAt(index);
                browserRegexen.Insert(index + 1, browserRegex);
                listview.SelectedIndex = index + 1;
            }
        }

        private void listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            moveUp.IsEnabled = true;
            moveDown.IsEnabled = true;

            if (listview.SelectedItem != null)
            {
                moveUp.IsEnabled = true;
                moveDown.IsEnabled = true;

                int index = listview.SelectedIndex;

                if (index == 0)
                {
                    moveUp.IsEnabled = false;
                }
                if (index + 1 == browserRegexen.Count)
                {
                    moveDown.IsEnabled = false;
                }
            }
            else
            {
                moveUp.IsEnabled = false;
                moveDown.IsEnabled = false;
            }
        }
	}
}
