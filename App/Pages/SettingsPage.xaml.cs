using System.Windows;
using System.Windows.Controls;
using Presence.Enums;
using Presence.Utils;

namespace Presence.Pages
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        public void LoadSettings()
        {
            if (Config.Current != null)
            {
                AutoStartPresence.IsChecked = Config.Current.AutoStartPresence;
            }
            RunOnStartup.IsChecked = Util.IsOnStartup();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.Minimize();
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.ChangePage(AppPage.Main);
        }

        private void SaveSettingsButton_Click(object sender, RoutedEventArgs e)
        {
            if (Config.Current != null)
            {
                Config.Current.AutoStartPresence = AutoStartPresence.IsChecked ?? false;
                Config.Current.Save();
            }

            bool startup = Util.IsOnStartup();
            if (RunOnStartup.IsChecked ?? false)
            {
                if (!startup)
                {
                    Util.AddToStartup();
                }
            }
            else
            {
                if (startup)
                {
                    Util.RemoveFromStartup();
                }
            }

            // Save button cooldown
            SaveSettings_Button.IsEnabled = false;
            SaveSettings_Button.Opacity = 0.75f;

            Util.WaitAction(() =>
            {
                SaveSettings_Button.IsEnabled = true;
                SaveSettings_Button.Opacity = 1f;
            }, 1f);
        }
    }
}