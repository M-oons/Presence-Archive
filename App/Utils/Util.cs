using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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
                return Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + $"\\{AppInfo.NAME}\\";
            }
        }

        public static string ConfigLocation
        {
            get
            {
                return RootLocation + "Config.json";
            }
        }

        public static string AppLocation
        {
            get
            {
                return Assembly.GetExecutingAssembly().Location;
            }
        }

        public static void CreateDataFolder()
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

        public static void StartProcess(string name)
        {
            Process.Start(name);
        }

        public static void StartProcess(string name, string args)
        {
            Process.Start(name, args);
        }

        public static Process[] GetProcesses(string name)
        {
            return Process.GetProcessesByName(name);
        }

        public static Process GetDiscordProcess()
        {
            Process[] processes = GetProcesses("Discord");
            if (processes.Length == 0) return null;
            return processes.FirstOrDefault(f => !string.IsNullOrEmpty(f.MainWindowTitle)); // main Discord process
        }

        public static async Task WaitForProcess(string name, float interval)
        {
            while (true)
            {
                Process[] processes = GetProcesses(name);
                if (processes.Length > 0)
                {
                    break;
                }
                await Task.Delay((int)(interval * 1000));
            }
        }

        public static void Popup(string title, string message, Action yesAction = null, Action noAction = null)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.YesNo, MessageBoxImage.Information);
            if (result == MessageBoxResult.Yes)
            {
                yesAction?.Invoke();
            }
            else if (result == MessageBoxResult.No)
            {
                noAction?.Invoke();
            }
        }

        public static void Warning(string title, string message, Action okAction = null)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Exclamation);
            if (result == MessageBoxResult.OK)
            {
                okAction?.Invoke();
            }
        }

        public static void Info(string title, string message, Action okAction = null)
        {
            MessageBoxResult result = MessageBox.Show(message, title, MessageBoxButton.OK, MessageBoxImage.Information);
            if (result == MessageBoxResult.OK)
            {
                okAction?.Invoke();
            }
        }

        public static bool AddToStartup()
        {
            try
            {
                RegistryKey key = Registry.CurrentUser.OpenSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Run", true);
                if (key != null)
                {
                    key.SetValue(AppInfo.NAME, AppLocation);
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
                    key.DeleteValue(AppInfo.NAME);
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
                    string path = key.GetValue(AppInfo.NAME)?.ToString();
                    return path != null && path.ToLower() == AppLocation.ToLower();
                }
            }
            catch
            {
            }
            return false;
        }

        public static void WaitAction(Action action, float seconds)
        {
            if (seconds == 0f)
            {
                action?.SafeInvoke();
                return;
            }
            Timer timer = new Timer(seconds * 1000);
            timer.Elapsed += (o, e) =>
            {
                action?.SafeInvoke();
                timer.Stop();
                timer.Dispose();
                timer = null;
            };
            timer.Start();
        }

        public static void SafeInvoke(this Action action)
        {
            if (action == null) return;
            GetApp()?.Dispatcher?.Invoke(action);
        }
    }
}