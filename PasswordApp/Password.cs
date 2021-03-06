﻿using System;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Ink;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Shapes;

/*
 * PasswordApp: This program will allow the user to store their passwords
 * in one convenient application on their Windows Phone
 * 
 * Password.cs: This file holds the password class that is used to store the passwords.
 * 
 * Programmers: Jose Castaneda z1701983 and Mark Gunlogson Z147395
 * 
 * Last Update 4/19/2014
 * Removed Modified, and changed content to EncryptedContent
 * 
 * Update 3/11/2014
 * Added Password class, added documentation to Password.cs
 */

namespace PasswordApp
{
    public class Password
    {
        public string Title { get; set; } // Title for the password entry  
        public string EncryptedContent { get; set; } // Content for the password entry
    }
}
