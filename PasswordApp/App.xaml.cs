﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Microsoft.Phone.Controls;
using Microsoft.Phone.Shell;
using System.IO.IsolatedStorage;
using System.Collections.ObjectModel;

namespace PasswordApp
{
    public partial class App : Application
    {
        /// <summary>
        /// Provides easy access to the root frame of the Phone Application.
        /// </summary>
        /// <returns>The root frame of the Phone Application.</returns>
        public PhoneApplicationFrame RootFrame { get; private set; }

        /// <summary>
        /// Constructor for the Application object.
        /// </summary>
        public App()
        {
            // Global handler for uncaught exceptions. 
            UnhandledException += Application_UnhandledException;

            // Standard Silverlight initialization
            InitializeComponent();

            // Phone-specific initialization
            InitializePhoneApplication();

            // Show graphics profiling information while debugging.
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // Display the current frame rate counters.
                Application.Current.Host.Settings.EnableFrameRateCounter = true;

                // Show the areas of the app that are being redrawn in each frame.
                //Application.Current.Host.Settings.EnableRedrawRegions = true;

                // Enable non-production analysis visualization mode, 
                // which shows areas of a page that are handed off to GPU with a colored overlay.
                //Application.Current.Host.Settings.EnableCacheVisualization = true;

                // Disable the application idle detection by setting the UserIdleDetectionMode property of the
                // application's PhoneApplicationService object to Disabled.
                // Caution:- Use this under debug mode only. Application that disables user idle detection will continue to run
                // and consume battery power when the user is not using the phone.
                PhoneApplicationService.Current.UserIdleDetectionMode = IdleDetectionMode.Disabled;
            }

        }

        // Code to execute when the application is launching (eg, from Start)
        // This code will not execute when the application is reactivated
        private void Application_Launching(object sender, LaunchingEventArgs e)
        {
            if (Settings.settings.Contains("HashedPassword"))
            {
                // This means we have an existing user, load
                Settings.HashedPassword = (string)Settings.settings["HashedPassword"];
                Settings.Salt = (byte[])Settings.settings["Salt"];
            }
        }

        // Code to execute when the application is activated (brought to foreground)
        // This code will not execute when the application is first launched
        private void Application_Activated(object sender, ActivatedEventArgs e)
        {
            if (!e.IsApplicationInstancePreserved) //need to recreate state
            {

                Settings.PasswordHint = (string)PhoneApplicationService.Current.State["PasswordHint"];
                Settings.Password = (string)PhoneApplicationService.Current.State["Password"];
                Settings.Salt=(byte[])Settings.settings["Salt"];// need to fix this depending on format of byte array
                Settings.BackupSet = (string)PhoneApplicationService.Current.State["BackupName"];// gets/sets string backupName.

                if (PhoneApplicationService.Current.State.ContainsKey("PasswordList"))
                {
                    Settings.PasswordsList = PhoneApplicationService.Current.State["PasswordList"] as ObservableCollection<Password>;
                }
                else
                {
                    //Create new one... but this shouldn't happen. I'm paranoid.
                    Settings.PasswordsList = new ObservableCollection<Password>();
                }
            }
        }

        // Code to execute when the application is deactivated (sent to background)
        // This code will not execute when the application is closing
        private void Application_Deactivated(object sender, DeactivatedEventArgs e)
        {
            // save hashed password, password hint, backup name, salt value, and the PasswordsList to both State and isolated storage.
            PhoneApplicationService.Current.State["PasswordHint"] = Settings.PasswordHint;
            PhoneApplicationService.Current.State["Password"] = Settings.Password ;
            PhoneApplicationService.Current.State["Salt"]= Settings.Salt;
            PhoneApplicationService.Current.State["BackupName"]= Settings.BackupSet;// gets/sets string backupName.
            PhoneApplicationService.Current.State["PasswordList"] = Settings.PasswordsList;
            SaveToIsolatedStorage();
        }

        // Code to execute when the application is closing (eg, user hit Back)
        // This code will not execute when the application is deactivated
        private void Application_Closing(object sender, ClosingEventArgs e)
        {
            SaveToIsolatedStorage();
        }

        // Code to save to isolated storage
        // This code can be called from any state in app.xaml.cs and will take from settings and save to ISO
        private void SaveToIsolatedStorage()
        {
            IsolatedStorageSettings settings = IsolatedStorageSettings.ApplicationSettings;
            settings["PasswordHint"] = Settings.PasswordHint;
            settings["Salt"] = Settings.Salt;
            settings["PasswordList"] = Settings.PasswordsList;
            settings["HashedPassword"] = Settings.HashedPassword;
            settings.Save();
        }
        // Code to execute if a navigation fails
        private void RootFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // A navigation has failed; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        // Code to execute on Unhandled Exceptions
        private void Application_UnhandledException(object sender, ApplicationUnhandledExceptionEventArgs e)
        {
            if (System.Diagnostics.Debugger.IsAttached)
            {
                // An unhandled exception has occurred; break into the debugger
                System.Diagnostics.Debugger.Break();
            }
        }

        #region Phone application initialization

        // Avoid double-initialization
        private bool phoneApplicationInitialized = false;

        // Do not add any additional code to this method
        private void InitializePhoneApplication()
        {
            if (phoneApplicationInitialized)
                return;

            // Create the frame but don't set it as RootVisual yet; this allows the splash
            // screen to remain active until the application is ready to render.
            RootFrame = new PhoneApplicationFrame();
            RootFrame.Navigated += CompleteInitializePhoneApplication;

            // Handle navigation failures
            RootFrame.NavigationFailed += RootFrame_NavigationFailed;

            // Ensure we don't initialize again
            phoneApplicationInitialized = true;
        }

        // Do not add any additional code to this method
        private void CompleteInitializePhoneApplication(object sender, NavigationEventArgs e)
        {
            // Set the root visual to allow the application to render
            if (RootVisual != RootFrame)
                RootVisual = RootFrame;

            // Remove this handler since it is no longer needed
            RootFrame.Navigated -= CompleteInitializePhoneApplication;
        }

        #endregion
    }
}