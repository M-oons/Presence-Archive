using System.Windows;
using System.Windows.Controls;
using Presence.Utils;

namespace Presence
{
    public partial class SettingsPage : Page
    {
        public SettingsPage()
        {
            InitializeComponent();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
            Application.Current.MainWindow.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void BackButton_Click(object sender, RoutedEventArgs e)
        {
            GoBack();
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

        private void GoBack()
        {
            Application.Current.MainWindow.Content = Util.GetMainWindow().OriginalContent;
        }
    }
}