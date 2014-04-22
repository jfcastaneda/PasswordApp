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

namespace PasswordApp
{
    public partial class MainPage : PhoneApplicationPage
    {
        // Where second thread starts
        static void FetchState()
        {
            if (PhoneApplicationService.Current.State.ContainsKey("PasswordList"))
            {
                Settings.PasswordsList = PhoneApplicationService.Current.State["PasswordList"] as ObservableCollection<Password>;
            }
            else
            {
                //create new one??? this shouldn't happen??
                Settings.PasswordsList = new ObservableCollection<Password>();
            }
            if (Settings.settings.Contains("Password"))
            {
                Settings.PasswordHint = (string)PhoneApplicationService.Current.State["PasswordHint"];
                Settings.Password = (string)PhoneApplicationService.Current.State["Password"];
                Settings.Salt = (byte[])Settings.settings["Salt"];// need to fix this depending on format of byte array
                Settings.BackupName = (string)PhoneApplicationService.Current.State["BackupName"];// gets/sets string backupName.
            }
        }
        public MainPage()
        {
            //run thread
            // create second thread and indicate its method to execute is StartMethod
            Thread worker = new Thread(new ThreadStart(FetchState));
            worker.Name = "Worker";                              // give it a name as well
            // worker.Priority = ThreadPriority.AboveNormal;
            worker.Start(); 
            
            // start it
            InitializeComponent();
            if (Settings.settings.Contains("HashedPassword"))
            {
                // This means we have an existing user, go to login page
                NewUser.Visibility = Visibility.Collapsed;
                AlreadyUser.Visibility = Visibility.Visible;
                //make sure menubar is visible
            }
            else
            {
                //new user
                NewUser.Visibility = Visibility.Visible;
                AlreadyUser.Visibility = Visibility.Collapsed;
            }
        }

        // Method to handle the click of the ok button on the login or new user page
        private void UserOK_Click(object sender, RoutedEventArgs e)
        {
            if (Settings.settings.Contains("HashedPassword") && Settings.IsLoggedIn == false)//normal login
            {
                //get password from passwordbox and hash it
                var pass=EnterPassword.Password;
                Crypto.Hash(pass);
                //then check against password from isolated storage
                if (pass == Settings.HashedPassword)//passwords match
                {
                    Settings.Password = pass;//set password property
                    Settings.IsLoggedIn = true;
                }
                //if not matched, display messagebox
                else
                {
                    MessageBox.Show("password entered does not match stored password :( . Try again");
                }

            }
            else
            {
                //if hashed pw from isolated storage is null, show new user panel
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
                            var salt=Crypto.GenerateNewSalt(16);
                            Settings.Salt = salt;
                            //hash hassword with it and set settings.hashedpassword
                            var newpassword=Crypto.Hash(NewPassword.Password);
                            Settings.HashedPassword = newpassword;
                            //also save hint,backup name,and cleartext password
                            Settings.BackupName = BackupSet.Text;
                            Settings.PasswordHint = PasswordHint.Text;
                            Settings.Password = NewPassword.Password;
                            //then set isloggedin to true and navigate to listviewpage
                            Settings.IsLoggedIn = true;
                            this.NavigationService.Navigate(new Uri("/ListPage.xaml", UriKind.Relative));

                        }

                    }

                }

            }
        }
    }
}