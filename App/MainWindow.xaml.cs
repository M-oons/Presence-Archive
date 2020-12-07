using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Presence.Utils;

namespace Presence
{
    public partial class MainWindow : Window
    {
        public SettingsPage SettingsPage { get; }
        public object OriginalContent { get; }

        public MainWindow()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage();
            OriginalContent = Content;
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Application.Current.MainWindow.WindowState = WindowState.Minimized;
        }

        private void StartPresenceButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence();
        }

        private void UpdatePresenceButton_Click(object sender, RoutedEventArgs e)
        {
            UpdatePresence();
        }

        private void StopPresenceButton_Click(object sender, RoutedEventArgs e)
        {
            App app = Util.GetApp();
            app?.Discord?.StopPresence();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            LoadSettings();
            Application.Current.MainWindow.Content = SettingsPage.Content;
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Util.StartProcess(AppInfo.URL);
        }

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Unfocus all inputs when background is clicked
            Keyboard.ClearFocus();
        }

        private void ClientID_TextChanged(object sender, TextChangedEventArgs e)
        {
            ClientID_Placeholder.Content = ClientID.Text.Length > 0 ? "" : "Application Client ID";
        }

        private void Details_TextChanged(object sender, TextChangedEventArgs e)
        {
            Details_Placeholder.Content = Details.Text.Length > 0 ? "" : "Details";
        }

        private void State_TextChanged(object sender, TextChangedEventArgs e)
        {
            State_Placeholder.Content = State.Text.Length > 0 ? "" : "State";
        }

        private void LargeImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            LargeImage_Placeholder.Content = LargeImageKey.Text.Length > 0 ? "" : "Large Image";
        }

        private void SmallImage_TextChanged(object sender, TextChangedEventArgs e)
        {
            SmallImage_Placeholder.Content = SmallImageKey.Text.Length > 0 ? "" : "Small Image";
        }

        private void LargeImageText_TextChanged(object sender, TextChangedEventArgs e)
        {
            LargeImageText_Placeholder.Content = LargeImageText.Text.Length > 0 ? "" : "Large Image Text";
        }

        private void SmallImageText_TextChanged(object sender, TextChangedEventArgs e)
        {
            SmallImageText_Placeholder.Content = SmallImageText.Text.Length > 0 ? "" : "Small Image Text";
        }

        private void UpdatePresence()
        {
            App app = Util.GetApp();

            if (app != null && app.Discord != null)
            {
                Activity newActivity = new Activity
                {
                    ClientID = ClientID.Text,
                    Details = Details.Text,
                    State = State.Text,
                    LargeImageKey = LargeImageKey.Text,
                    LargeImageText = LargeImageText.Text,
                    SmallImageKey = SmallImageKey.Text,
                    SmallImageText = SmallImageText.Text,
                    ShowTimestamp = ShowTimestampCheckbox.IsChecked ?? false,
                    ResetTimestamp = ResetTimestampCheckbox.IsChecked ?? false
                };

                if (newActivity.ResetTimestamp)
                {
                    app.Discord.ShouldResetTimestamp = true;
                }

                // Only allow update if new presence is different
                if (Config.Current != null && (!app.Discord.PresenceActive || app.Discord.ShouldResetTimestamp || !Config.Current.Activity.Equals(newActivity)))
                {
                    Config.Current.Activity = newActivity;
                    Config.Current.Save();

                    app.Discord.UpdatePresence();
                }
            }
        }

        private void LoadSettings()
        {
            if (Config.Current != null)
            {
                SettingsPage.AutoStartPresence.IsChecked = Config.Current.AutoStartPresence;
            }
            SettingsPage.RunOnStartup.IsChecked = Util.IsOnStartup();
        }
    }
}