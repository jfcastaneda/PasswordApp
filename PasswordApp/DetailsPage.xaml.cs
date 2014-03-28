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
using Microsoft.Phone.Tasks;

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * DetailsPage.xaml.cs: This file holds the Details classes and methods.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 3/28/2014
 * Changed handling of DateTimeOffSet, added more comments.
 */


namespace PasswordApp
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        /* 
         * The main class and constructor where execution begins. 
         */
        public DetailsPage()
        {
            InitializeComponent();
        }

        /*
         * Launches when the user nagivates to the page and loads saved data if needed
         */
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            if(Settings.CurrentIndex!=-1)
            {
                Password curpass = Settings.PasswordsList[Settings.CurrentIndex];
                if(!String.IsNullOrEmpty(curpass.Title)) thetitle.Text = curpass.Title;
                if(!String.IsNullOrEmpty(curpass.Content)) thecontent.Text = curpass.Content;
            }

        }

        /*
         * Launches when leaving the page to remove entry if nessesary.
         */
        protected override void OnNavigatingFrom(NavigatingCancelEventArgs e)
        {
            if (Settings.CurrentIndex == -1) return;//we have an invalid note index(probably already deleted), don't check for change
            if (String.IsNullOrEmpty(thetitle.Text))
            {
                Settings.PasswordsList.RemoveAt(Settings.CurrentIndex);
                Settings.CurrentIndex = -1;//index no longer valid for deleted item
                return;
            }

            //check if anything changed
            Password curpass = Settings.PasswordsList[Settings.CurrentIndex];
                if (curpass.Content != thecontent.Text || curpass.Title != thetitle.Text)
                {
                    if (MessageBox.Show("Save Changes?", "Abandon Notice", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
                    {
                        actualsave();
                        //this part cancels navigation otherwise user will not be able to respond to any errors from save function without losing their stuff :(
                        e.Cancel=true;
                        base.OnNavigatingFrom(e);
                    }
                }
        }

        /*
         * Launches when the page is loaded and if user is creating a password it sets focus
         */
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //arrived to create new password
            if (thetitle.Text.Length == 0)
            {
                thetitle.Focus();
            }
        }

        /*
         * Handles deleting event when button is clicked
         * Removes by index then goes back
         */
        private void ApplicationBarIconButton_delete_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to delete this password?", "Delete Item", MessageBoxButton.OKCancel) == MessageBoxResult.OK)
            {
                Settings.PasswordsList.RemoveAt(Settings.CurrentIndex);
                Settings.CurrentIndex = -1; //index no longer valid for deleted item
                if (this.NavigationService.CanGoBack)
                {
                    this.NavigationService.GoBack(); //this triggers onnavigatefrom which tries to delete item again...
                }
            }
        }



        /*
         * Handles email event when button is clicked
         * Send to email handler
         */
        private void ApplicationBarIconButton_email_Click(object sender, EventArgs e)
        {
            EmailComposeTask eeeemail = new EmailComposeTask();
            eeeemail.Subject = "pass phishing";
            eeeemail.Body = Settings.PasswordsList[Settings.CurrentIndex].Content;
            eeeemail.To = "z147395@students.niu.edu";
            eeeemail.Show();
        }

        /* Handles even of clicking the save button
         * calls the save function
         */
        private void ApplicationBarIconButton_check_Click(object sender, EventArgs e)
        {
            actualsave();
        }

        /*
         * Saves data into Isolated Storage!
         */
        private void actualsave()
        {
            if (String.IsNullOrEmpty(thecontent.Text) || String.IsNullOrEmpty(thetitle.Text))
            {
                MessageBox.Show("Title and Content fields are both required");
                return;
            }
            Password curpass = Settings.PasswordsList[Settings.CurrentIndex];
            //check if anything changed
            if (curpass != null)//make sure something is here(sanity check)
            {
                //content changed!
                if (curpass.Content != thecontent.Text)
                {
                    //same title?
                    if (curpass.Title == thetitle.Text)//title is same so only update content
                    {
                        Settings.PasswordsList[Settings.CurrentIndex].Content = thecontent.Text;
                    }
                    else //title and content changed so update timestamp too
                    {
                        Settings.PasswordsList[Settings.CurrentIndex].Title = thetitle.Text;
                        Settings.PasswordsList[Settings.CurrentIndex].Content = thecontent.Text;
                        Settings.PasswordsList[Settings.CurrentIndex].Modified = DateTimeOffset.Now;
                    }
                }
                MessageBox.Show("Password Saved");
            }
        }
    }
}