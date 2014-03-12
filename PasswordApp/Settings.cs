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

/*
 * Password Keeper V1: This program will allow the user to store their passwords
 * in one convenient application on their Windows Phone
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 3/11/2014
 * Added documentation to all cs files, and did initial setup of project.
 */

namespace PasswordApp
{
    public static class Settings
    {
        public IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;

        public static System.Collections.ObjectModel.ObservableCollection<Password> PasswordsList
        {

        }

        public int CurrentIndex
        {

        }
    }
}
