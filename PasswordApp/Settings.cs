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
        static IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
        public static ObservableCollection<Password> PasswordsList;
        public static int CurrentIndex{get;set;}
    }
}
