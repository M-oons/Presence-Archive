using System;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows;
using System.Windows.Forms;
using Presence.Utils;

using Application = System.Windows.Application;

namespace Presence
{
    public partial class App : Application
    {
        public Discord Discord { get; private set; }
        public NotifyIcon Tray { get; private set; }

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

            Tray = new NotifyIcon
            {
                Text = Constants.APP_NAME,
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

        private void CreateTrayMenu()
        {
            Tray.ContextMenu = new ContextMenu();

            var title = Tray.ContextMenu.MenuItems.Add(Constants.APP_NAME);
            title.Enabled = false;
            title.DefaultItem = true;

            Tray.ContextMenu.MenuItems.Add("-"); // Separator

            var folder = Tray.ContextMenu.MenuItems.Add("Open Folder");
            folder.Click += TrayFolder_Click;

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
                MainWindow.Activate();
            }
            else
            {
                MainWindow.Show();
                SetupInputs();
            }
        }

        private void Quit()
        {
            _quitting = true;
            MainWindow.Close();
            Tray.Dispose();
            Tray = null;
            Discord?.Quit();
        }

        public void SetupInputs()
        {
            // Populate inputs with config values
            Activity activity = Config.Current.Activity;
            Util.GetMainWindow().ClientID.Text = activity.ClientID;
            Util.GetMainWindow().Details.Text = activity.Details;
            Util.GetMainWindow().State.Text = activity.State;
            Util.GetMainWindow().LargeImageKey.Text = activity.LargeImageKey;
            Util.GetMainWindow().LargeImageText.Text = activity.LargeImageText;
            Util.GetMainWindow().SmallImageKey.Text = activity.SmallImageKey;
            Util.GetMainWindow().SmallImageText.Text = activity.SmallImageText;
            Util.GetMainWindow().ShowTimestampCheckbox.IsChecked = activity.ShowTimestamp;
            Util.GetMainWindow().ResetTimestampCheckbox.IsChecked = activity.ResetTimestamp;
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

        private void TrayFolder_Click(object sender, EventArgs e)
        {
            Process.Start(Util.RootLocation);
        }

        private void TrayQuit_Click(object sender, EventArgs e)
        {
            Quit();
        }
    }
}