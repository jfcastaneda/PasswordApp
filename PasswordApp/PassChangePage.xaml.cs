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

namespace PasswordApp
{
    public partial class PassChangePage : PhoneApplicationPage
    {
        public PassChangePage()
        {
            InitializeComponent();
        }

        private void UserOK_Click(object sender, RoutedEventArgs e)
        {
            var hashedpass = Crypto.Hash(OldPassword.Password);
            if (Settings.HashedPassword != hashedpass)
            {
                MessageBox.Show("Current Password is incorrect");
            }
            else
            {
                if(String.IsNullOrWhiteSpace(NewPassword.Password))
                {
                    MessageBox.Show("New Password Cannot Be Blank");
                }
                else
                {
                    if (NewPassword.Password != NewPasswordAgain.Password)
                    {
                        MessageBox.Show("New Passwords Do Not Match");
                    }
                    else//all good
                    {
                        //decrypt old passwords
                        List<string> decryptedpasswords=new List<string>();
                        for(int i=0;i<Settings.PasswordsList.Count;i++)
                        {
                            var decryptedpass=Crypto.Decrypt(Settings.PasswordsList[i].EncryptedContent,Settings.Password);
                            decryptedpasswords[i]=decryptedpass;
                        }
                        //create and assign new salt and hash
                        var newsalt=Crypto.GenerateNewSalt(16);
                        var newpasshash=Crypto.Hash(NewPassword.Password);
                        Settings.SaltBytes=newsalt;
                        Settings.HashedPassword=newpasshash;

                        //set actual password to new
                        Settings.PasswordHint=PasswordHint.Text;
                        Settings.Password=NewPassword.Password;

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