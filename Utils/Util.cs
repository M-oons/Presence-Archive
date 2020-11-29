using System;
using System.IO;
using System.Reflection;
using System.Timers;
using System.Windows;
using Microsoft.Win32;

namespace Presence.Utils
{
    public static class Util
    {
        public static string RootLocation
        {
            get
            {
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{Constants.APP_NAME}\\";
            }
        }

        public static string ConfigLocation
        {
            get
            {
                return RootLocation + "Config.json";
            }
        }

        public static void CreateRootFolder()
        {
            Directory.CreateDirectory(RootLocation);
        }

        public static App GetApp()
        {
            return (App)Application.Current;
        }

        public static MainWindow GetMainWindow()
        {
            return (MainWindow)Application.Current.MainWindow;
        }

        public static bool AddToStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    key.SetValue(Constants.APP_NAME, Assembly.GetExecutingAssembly().Location);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool RemoveFromStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    key.DeleteValue(Constants.APP_NAME);
                    return true;
                }
            }
            catch
            {
            }
            return false;
        }

        public static bool IsOnStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    string path = key.GetValue(Constants.APP_NAME)?.ToString();
                    return path != null && path.ToLower() == Assembly.GetExecutingAssembly().Location.ToLower();
                }
            }
            catch
            {
            }
            return false;
        }

        public static void WaitAction(Action action, float seconds)
        {
            Timer timer = new Timer(seconds * 1000);
            timer.Elapsed += (o, e) =>
            {
                GetApp()?.Dispatcher?.Invoke(action);
                timer.Stop();
                timer.Dispose();
                timer = null;
            };
            timer.Start();
        }
    }
}