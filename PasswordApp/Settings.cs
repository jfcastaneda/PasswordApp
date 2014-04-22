using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

/*
 * Password Keeper V1: This program will allow the user to store their passwords
 * in one convenient application on their Windows Phone
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 3/11/2014
 * Added Settings class, and pupulated it with the three required items. Also added
 * documentation
 */

namespace PasswordApp
{
    public static class Settings
    {
        public static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings; // Holds isolated storage for the app
        public static ObservableCollection<Password> PasswordsList; // List of password class that will hold the users passwords
        public static int CurrentIndex { get; set; } // Holds the index so we can see what password we are working with
        public static string HashedPassword { get; set; } // gets/sets string HashedPassword
        public static string PasswordHint { get; set; } // gets/sets string PasswordHint.
        public static string Password { get; set; } // gets/sets string password (this is clear text password - kept in memory only during life of app)
        public static bool IsLoggedIn { get; set; } // gets/sets bool isLoggedIn.
        public static byte[] Salt { get; set; } // gets/sets byte[] salt. this salt is used for password hashing
        public static string BackupSet { get; set; } // gets/sets string BackupSet.


    }
}
