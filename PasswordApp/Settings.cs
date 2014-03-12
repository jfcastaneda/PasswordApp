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

namespace PasswordApp
{
    public static class Settings
    {
        public static System.Collections.ObjectModel.ObservableCollection<Password> PasswordsList
        {

        }

        public int CurrentIndex
        {

        }

        public IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
    }
}
