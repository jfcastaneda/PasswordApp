using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using System.Windows.Navigation;
using System.Windows.Data;

namespace PasswordApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Constructor
        public MainPage()
        {
            InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            this.DataContext = null;
            this.DataContext = Settings.PasswordsList;
            Settings.CurrentIndex = -1;
            Passwords.SelectedIndex = -1;
            if (Settings.PasswordsList.Count == 0)
            {
                NoPassLabel.Visibility = Visibility.Visible;
            }
            else
            {
                NoPassLabel.Visibility = Visibility.Collapsed;
            }
        }

        private void Passwords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (Passwords.SelectedIndex > -1)
            {
                Settings.CurrentIndex = Passwords.SelectedIndex;   // set current node value
                this.NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            Password pass = new Password();
            pass.Modified = DateTimeOffset.Now;
            Settings.PasswordsList.Insert(0, pass);
            Settings.CurrentIndex = 0;
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
        }

        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {

        }
    }
}