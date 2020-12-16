using System;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Presence.Pages;
using Presence.Utils;

using Application = System.Windows.Application;

namespace Presence
{
    public partial class App : Application
    {
        public NotifyIcon Tray { get; private set; }
        public Discord Discord { get; private set; }

        private bool _quitting;

        public App()
        {
            // Load embedded resources
            AppDomain.CurrentDomain.AssemblyResolve += (sender, e) =>
            {
                string dll = new AssemblyName(e.Name).Name + ".dll";
                string resource = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(r => r.EndsWith(dll));

                using (Stream stream = Assembly.GetExecutingAssembly().GetManifestResourceStream(resource))
                {
                    byte[] assemblyData = new byte[stream.Length];
                    stream.Read(assemblyData, 0, assemblyData.Length);
                    return Assembly.Load(assemblyData);
                }
            };
        }

        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            AppInfo.CheckInstance();
            AppInfo.CheckForUpdate();

            Tray = new NotifyIcon
            {
                Text = AppInfo.NAME,
                Icon = Presence.Properties.Resources.WumpusOffIcon,
                Visible = true
            };
            Tray.MouseClick += Tray_Click;

            MainWindow = new MainWindow();
            MainWindow.Closing += MainWindow_Closing;

            Config.Load();

            CreateTrayMenu();

            Discord = new Discord();
            Discord.Start();
        }

        public void ChangePage(AppPage page)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null)
            {
                switch (page)
                {
                    default:
                    case AppPage.Main:
                        Current.MainWindow.Content = mainWindow.OriginalContent;
                        break;

                    case AppPage.Activities:
                        ActivitiesPage activitiesPage = mainWindow.ActivitiesPage;
                        if (activitiesPage != null)
                        {
                            activitiesPage.LoadActivities();
                            Current.MainWindow.Content = activitiesPage.Content;
                        }
                        break;

                    case AppPage.Settings:
                        SettingsPage settingsPage = mainWindow.SettingsPage;
                        if (settingsPage != null)
                        {
                            settingsPage.LoadSettings();
                            Current.MainWindow.Content = settingsPage.Content;
                        }
                        break;
                }
            }
        }

        public void Minimize()
        {
            Current.MainWindow.WindowState = WindowState.Minimized;
        }

        public void Close()
        {
            ChangePage(AppPage.Main);
            Minimize();
            Util.WaitAction(Current.MainWindow.Close, 0.5f);
        }

        public void Quit()
        {
            _quitting = true;
            MainWindow?.Close();
            Tray?.Dispose();
            Tray = null;
            Discord?.Quit();
            Current?.Shutdown();
        }

        private void CreateTrayMenu()
        {
            Tray.ContextMenu = new ContextMenu();

            var title = Tray.ContextMenu.MenuItems.Add(AppInfo.NAME);
            title.Enabled = false;
            title.DefaultItem = true;

            Tray.ContextMenu.MenuItems.Add("-"); // Separator

            var location = Tray.ContextMenu.MenuItems.Add("Open App Location");
            location.Click += TrayLocation_Click;

            var data = Tray.ContextMenu.MenuItems.Add("Open Data Folder");
            data.Click += TrayData_Click;

            Tray.ContextMenu.MenuItems.Add("-"); // Separator

            var update = Tray.ContextMenu.MenuItems.Add("Check For Update");
            update.Click += TrayUpdate_Click;

            Tray.ContextMenu.MenuItems.Add("-"); // Separator

            var quit = Tray.ContextMenu.MenuItems.Add("Quit");
            quit.Click += TrayQuit_Click;
        }

        private void ShowMainWindow()
        {
            if (MainWindow.IsVisible)
            {
                if (MainWindow.WindowState == WindowState.Minimized)
                {
                    MainWindow.WindowState = WindowState.Normal;
                }
            }
            else
            {
                MainWindow.Show();
            }
            MainWindow.Activate();
            Util.GetMainWindow()?.SetupInputs(Config.Current?.GetActivity()?.Activity);
        }

        private void MainWindow_Closing(object sender, CancelEventArgs e)
        {
            if (!_quitting)
            {
                e.Cancel = true;
                // Minimize app to tray
                MainWindow.Hide();
            }
        }

        private void Tray_Click(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                ShowMainWindow();
            }
        }

        private void TrayLocation_Click(object sender, EventArgs e)
        {
            Util.StartProcess("explorer.exe", $"/select, \"{Util.AppLocation}\"");
        }

        private void TrayData_Click(object sender, EventArgs e)
        {
            Util.StartProcess(Util.RootLocation);
        }

        private void TrayUpdate_Click(object sender, EventArgs e)
        {
            AppInfo.CheckForUpdate(true);
        }

        private void TrayQuit_Click(object sender, EventArgs e)
        {
            Quit();
        }
    }
}