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
using System.Windows.Data;

/*
 * PasswordApp: This program will allow the user to save
 * their passwords on their phone.
 * 
 * DateConverter.cs: This file holds the DataConverter class.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 3/26/2014
 * Added Convert and Convertback. Convertback is not nessesary for this binding.
 */

namespace PasswordApp
{
    public class DateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            DateTimeOffset thisdate = (DateTimeOffset)value;
            return thisdate.ToString("MM/dd/yy h:mm");
        }
        // ConvertBack is not implemented for a OneWay binding.
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
