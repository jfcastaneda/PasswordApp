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
            this.Loaded += new RoutedEventHandler(MainPage_Loaded);
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            //This means we just entered the MainPage of the app
            DateTime exampleDate = new DateTime(2008, 5, 1, 18, 32, 6);

            // Display the date using the current (en-US) culture.
            MessageBox.Show(exampleDate.ToString());

            Settings.CurrentIndex = -1;
            Passwords.SelectedIndex = -1;

            //Check if we need to hide the no passwords message
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
            //Check if we actually want the SelectionChanged event to fire
            if (Passwords.SelectedIndex > -1)
            {
                Settings.CurrentIndex = Passwords.SelectedIndex;   // set current node value
                this.NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
            }
        }

        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //Create new instance of Password, set DateTime and store data,
            Password pass = new Password();
            pass.Modified = DateTimeOffset.Now;
            Settings.PasswordsList.Insert(0, pass);
            Settings.CurrentIndex = 0;
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
        }

        private void MainPage_Loaded(object sender, RoutedEventArgs e)
        {
            Passwords.Items.Clear();
            Passwords.DataContext = Settings.PasswordsList;
            foreach (Password p in Settings.PasswordsList)
                {
                    Passwords.Items.Add(p);
                }
        }
    }
}