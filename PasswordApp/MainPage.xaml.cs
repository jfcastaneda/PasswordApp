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

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * MainPage.xaml.cs: This file holds the Details classes and methods.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 3/28/2014
 * Changed handling of DateTimeOffSet, added more comments.
 * Also changed the population of the listbox in the MainPage_Loaded Function
 * Added the clear to make sure that the list will always be accurate and not
 * be overpopulated.
 */

namespace PasswordApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        /* 
         * The main class and constructor where execution begins. 
         */
        public MainPage()
        {
            InitializeComponent();
            this.Loaded += new RoutedEventHandler(MainPage_Loaded); //Add it to the new EventHandler
        }

        /* 
         * The eventhandler for when the user reaches the page. 
         * Here we will make sure that the selection is reset and 
         * the no passwords message shows/doesn't show
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
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

        /*
         * Handles when the user selects a password 
         */
        private void Passwords_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            //Check if we actually want the SelectionChanged event to fire
            if (Passwords.SelectedIndex > -1)
            {
                Settings.CurrentIndex = Passwords.SelectedIndex;   // set current node value
                this.NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
            }
        }
        
        /*
         * Handles the add button click event. 
         * Sends the user to the detail pages
         */
        private void ApplicationBarIconButton_Click(object sender, EventArgs e)
        {
            //Create new instance of Password, set DateTime and store data,
            Password pass = new Password();
            pass.Modified = DateTimeOffset.Now;
            Settings.PasswordsList.Insert(0, pass);
            Settings.CurrentIndex = 0;
            NavigationService.Navigate(new Uri("/DetailsPage.xaml", UriKind.Relative));
        }

        /*
         * The event that launches once the page has loaded.
         * Using it to clear the list and reload it when changes are made.
         */
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