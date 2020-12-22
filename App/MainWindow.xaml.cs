using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Presence.Activities;
using Presence.Enums;
using Presence.Pages;
using Presence.Utils;

namespace Presence
{
    public partial class MainWindow : Window
    {
        public SettingsPage SettingsPage { get; }
        public ActivitiesPage ActivitiesPage { get; }
        public object OriginalContent { get; }

        public MainWindow()
        {
            InitializeComponent();
            SettingsPage = new SettingsPage();
            ActivitiesPage = new ActivitiesPage();
            OriginalContent = Content;
        }

        public void SetupInputs(Activity activity)
        {
            // Populate inputs
            if (activity != null)
            {
                ClientID.Text = activity.ClientID;
                Details.Text = activity.Details;
                State.Text = activity.State;
                LargeImageKey.Text = activity.LargeImageKey;
                LargeImageText.Text = activity.LargeImageText;
                SmallImageKey.Text = activity.SmallImageKey;
                SmallImageText.Text = activity.SmallImageText;
                ShowTimestampCheckbox.IsChecked = activity.ShowTimestamp;
                ResetTimestampCheckbox.IsChecked = activity.ResetTimestamp;
            }

            // Unfocus all inputs
            Keyboard.ClearFocus();
        }

        private void CloseButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.Close();
        }

        private void MinimizeButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.Minimize();
        }

        private void SettingsButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.ChangePage(AppPage.Settings);
        }

        private void ActivitiesButton_Click(object sender, RoutedEventArgs e)
        {
            Util.GetApp()?.ChangePage(AppPage.Activities);
        }

        private void HelpButton_Click(object sender, RoutedEventArgs e)
        {
            Util.StartProcess(AppInfo.URL);
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
            Util.GetApp()?.Discord?.StopPresence();
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

            if (app != null && app.Discord != null && Config.Current != null)
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

                bool usingSavedActivity = Config.Current.UsingSavedActivity();

                if (usingSavedActivity && !newActivity.Equals(Config.Current.GetActivity()?.Activity))
                {
                    usingSavedActivity = false;
                }

                if (newActivity.ResetTimestamp)
                {
                    app.Discord.ShouldResetTimestamp = true;
                }

                // Only allow update if new presence is different
                if (newActivity.IsValid() && (!app.Discord.PresenceActive || app.Discord.ShouldResetTimestamp || !newActivity.Equals(app.Discord.CurrentActivity)))
                {
                    // Change to default activity if the inputs have been changed
                    if (!usingSavedActivity)
                    {
                        Config.Current.Activity = 0;
                    }

                    // Save new activity as default
                    Config.Current.SaveActivity(0, new SavedActivity
                    {
                        Name = "Default",
                        Activity = newActivity
                    });

                    app.Discord.UpdatePresence(newActivity);
                }
            }
        }
    }
}