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
using System.Threading;
using Microsoft.Phone.Shell;
using System.Collections.ObjectModel;

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * MainPage.xaml.cs: This file handles the new/current user signup/login
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 4/20/2014
 * Added code to handle fetching all of the user's data. Also added code to
 * save the new users data right away in order to avoid errors.
 * 
 * Update 4/16/2014 
 * Added code to handle switching views.
 */

namespace PasswordApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        /* 
         * This is the second thread that we use to fetch memory on login.
         * Will pull from isolated storage and restore the user's info.
         */
        static void FetchPass()
        {
            Settings.PasswordsList = (ObservableCollection<Password>)Settings.settings["PasswordList"];
            Settings.PasswordHint = (string)Settings.settings["PasswordHint"];
            Settings.Password = (string)Settings.settings["HashedPassword"];
            Settings.Salt = (byte[])Settings.settings["Salt"];
            Settings.BackupSet = (string)Settings.settings["BackupSet"];
        }

        /* 
         * The main class and constructor where execution begins. 
         */
        public MainPage()
        {          
            InitializeComponent();
            if (Settings.settings.Contains("HashedPassword"))
            {
                //this means we have an existing user, go to login viww
                NewUser.Visibility = Visibility.Collapsed;
                AlreadyUser.Visibility = Visibility.Visible;
                ApplicationBar.IsVisible = true;
            }
            else
            {
                //this means we have a new user go to new user view
                NewUser.Visibility = Visibility.Visible;
                AlreadyUser.Visibility = Visibility.Collapsed;
            }
        }

        /* 
         * Method to handle the click of the ok button on the login or new user page.
         * Will either create and store the new user, or load the current one.
         */
        private void UserOK_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.settings.Contains("HashedPassword") && Settings.IsLoggedIn == false) //normal login
            {
                //get password from passwordbox and hash it
                var pass = EnterPassword.Password;
                var hashPass = Crypto.Hash(pass);

                //then check against password from isolated storage
                if (hashPass == Settings.HashedPassword) //passwords match
                {
                    Settings.Password = pass; //set password property

                    //create second thread and indicate its method to execute is FetchState
                    Thread worker = new Thread(new ThreadStart(FetchPass));
                    worker.Name = "FetchPass";
                    worker.Start();

                    //then set isloggedin to true and navigate to listviewpage
                    Settings.IsLoggedIn = true;
                    this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
                }
                else //if not matched, display messagebox
                {
                    MessageBox.Show("Password entered is incorrect. Please try again.");
                }

            }
            else
            {
                //if hashed password from isolated storage is null, show new user panel
                if (String.IsNullOrWhiteSpace(NewPassword.Password))
                {
                    MessageBox.Show("No password entered");
                }
                else
                {
                    if (NewPassword.Password != NewPasswordAgain.Password)
                    {
                        MessageBox.Show("Passwords do not match!");
                    }
                    else
                    {
                        if (String.IsNullOrWhiteSpace(BackupSet.Text))
                        {
                            MessageBox.Show("No backup set name chosen!");
                        }
                        else//everything is good!
                        {
                            // generate new salt and save it
                            var salt = Crypto.GenerateNewSalt(16);
                            Settings.Salt = salt;

                            //hash hassword with it and set settings.hashedpassword
                            var newpassword=Crypto.Hash(NewPassword.Password);
                            Settings.HashedPassword = newpassword;
                            
                            //also save hint,backup name,and cleartext password and setup password list
                            Settings.BackupSet = BackupSet.Text;
                            Settings.PasswordHint = PasswordHint.Text;
                            Settings.Password = NewPassword.Password;
                            Settings.PasswordsList = new ObservableCollection<Password>();

                            //immediate save to isolated storage ('I'm paranoid, why wait on saving?)
                            Settings.settings["HashedPassword"] = newpassword;
                            Settings.settings["BackupSet"] = BackupSet.Text;
                            Settings.settings["Salt"] = Settings.Salt;
                            Settings.settings["PasswordHint"] = PasswordHint.Text;

                            //then set isloggedin to true and navigate to listviewpage
                            Settings.IsLoggedIn = true;
                            this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));
                        }
                    }
                }
            }
        }

        /* 
         * Method to handle the password hint display.
         * Will display the hint, or the no hint message.
         */
        private void ApplicationBarMenuItem_Click(object sender, EventArgs e)
        {
            if ((string)Settings.settings["PasswordHint"] != null)
            {
                MessageBox.Show((string)Settings.settings["PasswordHint"]);
            }
            else
            {
                MessageBox.Show("No hint! Sorry :(");
            }
           
        }
    }
}