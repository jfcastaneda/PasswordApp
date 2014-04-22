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

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * PassChangePage.xaml.cs: This file holds the methods for the password change page.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Update 4/20/2014 
 * Added UserOK_Click to handle creating a new password
 * will not allow user to make a new password if old one is unknown.
 */

namespace PasswordApp
{
    public partial class PassChangePage : PhoneApplicationPage
    {
        /* 
         * The main class and constructor where execution begins. 
         */
        public PassChangePage()
        {
            InitializeComponent();
        }

        /* 
         * This method handles the OK click from the user and checks that all is ok
         * if all is ok, then it will re-create the password and re-encrypt the list.
         */
        private void UserOK_Click(object sender, RoutedEventArgs e)
        {
            var hashedpass = Crypto.Hash(OldPassword.Password);
            if (Settings.HashedPassword != hashedpass)
            {
                MessageBox.Show("Current Password is incorrect");
                return;
            }
            else
            {
                if(String.IsNullOrWhiteSpace(NewPassword.Password))
                {
                    MessageBox.Show("New Password Cannot Be Blank");
                    return;
                }
                else
                {
                    if (NewPassword.Password != NewPasswordAgain.Password)
                    {
                        MessageBox.Show("New Passwords Do Not Match");
                        return;
                    }
                    else //all good
                    {
                        //decrypt old passwords
                        List<string> decryptedpasswords = new List<string>();
                        for(int i=0; i < Settings.PasswordsList.Count; i++)
                        {
                            var decryptedpass = Crypto.Decrypt(Settings.PasswordsList[i].EncryptedContent,Settings.Password);
                            decryptedpasswords.Add(decryptedpass);
                        }

                        //create and assign new salt and hash
                        var newsalt = Crypto.GenerateNewSalt(16);
                        Settings.Salt = newsalt;
                        var newpasshash = Crypto.Hash(NewPassword.Password);
                        Settings.HashedPassword = newpasshash;

                        //set actual password to new
                        Settings.PasswordHint = PasswordHint.Text;
                        Settings.Password = NewPassword.Password;

                        //reencrypt passwordlist
                        for(int i=0;i<Settings.PasswordsList.Count;i++)
                        {
                            Settings.PasswordsList[i].EncryptedContent=Crypto.Encrypt(decryptedpasswords[i],Settings.Password);
                        }

                        MessageBox.Show("Password is successfully changed");
                        NavigationService.GoBack();
                    }
                }
            }
        }
    }
}