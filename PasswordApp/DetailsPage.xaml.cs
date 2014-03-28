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
 * Last Update 3/26/2014
 * Added setup email method. Added documentation
 */


namespace PasswordApp
{
    public partial class DetailsPage : PhoneApplicationPage
    {
        public DetailsPage()
        {
            InitializeComponent();
        }
        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            // Make initaltex! Check if the user didnt want to check anything.
            // So it wont bind an emty item
            if(Settings.CurrentIndex!=-1)
            {
                Password curpass = Settings.PasswordsList[Settings.CurrentIndex];
                if(!String.IsNullOrEmpty(curpass.Title)) thetitle.Text = curpass.Title;
                if(!String.IsNullOrEmpty(curpass.Content)) thecontent.Text = curpass.Content;
            }

        }
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
        private void PhoneApplicationPage_Loaded(object sender, RoutedEventArgs e)
        {
            //arrived to create new password
            if (thetitle.Text.Length == 0)
            {
                thetitle.Focus();
            }
        }

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

        private void ApplicationBarIconButton_email_Click(object sender, EventArgs e)
        {
            EmailComposeTask eeeemail = new EmailComposeTask();
            eeeemail.Subject = "pass phishing";
            eeeemail.Body = Settings.PasswordsList[Settings.CurrentIndex].Content;
            eeeemail.To = "z147395@students.niu.edu";
            eeeemail.Show();
        }
        //save password
        private void ApplicationBarIconButton_check_Click(object sender, EventArgs e)
        {
            actualsave();
        }
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
                        //Settings.PasswordsList[Settings.CurrentIndex].Modified = DateTimeOffset.Now;
                    }
                }
                MessageBox.Show("Password Saved");
                MessageBox.Show(Settings.PasswordsList[Settings.CurrentIndex].Title);
                MessageBox.Show(Settings.PasswordsList[Settings.CurrentIndex].Content);
                //MessageBox.Show(Settings.PasswordsList[Settings.CurrentIndex].Modified);
            }
        }
    }
}