using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using Presence.Activities;
using Presence.Utils;

namespace Presence.Pages
{
    public partial class ActivitiesPage : Page
    {
        public ActivitiesPage()
        {
            InitializeComponent();
        }

        public void LoadActivities()
        {
            // Populate inputs
            if (Config.Current != null)
            {
                Config.Current.SetupActivities();

                Activity1.Text = Config.Current.Activities[1].Name;
                Activity1_Placeholder.Content = "";
                LoadActivity1_Button.IsEnabled = Config.Current.Activity != 1;
                LoadActivity1_Button.Opacity = Config.Current.Activity == 1 ? 0.75f : 1f;

                Activity2.Text = Config.Current.Activities[2].Name;
                Activity2_Placeholder.Content = "";
                LoadActivity2_Button.IsEnabled = Config.Current.Activity != 2;
                LoadActivity2_Button.Opacity = Config.Current.Activity == 2 ? 0.75f : 1f;

                Activity3.Text = Config.Current.Activities[3].Name;
                Activity3_Placeholder.Content = "";
                LoadActivity3_Button.IsEnabled = Config.Current.Activity != 3;
                LoadActivity3_Button.Opacity = Config.Current.Activity == 3 ? 0.75f : 1f;

                Activity4.Text = Config.Current.Activities[4].Name;
                Activity4_Placeholder.Content = "";
                LoadActivity4_Button.IsEnabled = Config.Current.Activity != 4;
                LoadActivity4_Button.Opacity = Config.Current.Activity == 4 ? 0.75f : 1f;

                Activity5.Text = Config.Current.Activities[5].Name;
                Activity5_Placeholder.Content = "";
                LoadActivity5_Button.IsEnabled = Config.Current.Activity != 5;
                LoadActivity5_Button.Opacity = Config.Current.Activity == 5 ? 0.75f : 1f;
            }
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

        private void Rectangle_MouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            // Unfocus all inputs when background is clicked
            Keyboard.ClearFocus();
        }

        #region Activity 1

        private void Activity1_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activity1_Placeholder.Content = Activity1.Text.Length > 0 ? "" : "Activity 1";
        }

        private void SaveActivity1Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null && !string.IsNullOrEmpty(Activity1.Text))
            {
                SavedActivity activity = new SavedActivity
                {
                    Name = Activity1.Text,
                    Activity = new Activity
                    {
                        ClientID = mainWindow.ClientID.Text,
                        Details = mainWindow.Details.Text,
                        State = mainWindow.State.Text,
                        LargeImageKey = mainWindow.LargeImageKey.Text,
                        LargeImageText = mainWindow.LargeImageText.Text,
                        SmallImageKey = mainWindow.SmallImageKey.Text,
                        SmallImageText = mainWindow.SmallImageText.Text,
                        ShowTimestamp = mainWindow.ShowTimestampCheckbox.IsChecked ?? false,
                        ResetTimestamp = mainWindow.ResetTimestampCheckbox.IsChecked ?? false
                    }
                };
                SaveActivity(1, activity);
            }
        }

        private void ResetActivity1Button_Click(object sender, RoutedEventArgs e)
        {
            ResetActivity(1);
            Activity1.Text = "Activity 1";
        }

        private void LoadActivity1Button_Click(object sender, RoutedEventArgs e)
        {
            SetActivity(1);
        }

        #endregion

        #region Activity 2

        private void Activity2_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activity1_Placeholder.Content = Activity1.Text.Length > 0 ? "" : "Activity 2";
        }

        private void SaveActivity2Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null && !string.IsNullOrEmpty(Activity2.Text))
            {
                SavedActivity activity = new SavedActivity
                {
                    Name = Activity2.Text,
                    Activity = new Activity
                    {
                        ClientID = mainWindow.ClientID.Text,
                        Details = mainWindow.Details.Text,
                        State = mainWindow.State.Text,
                        LargeImageKey = mainWindow.LargeImageKey.Text,
                        LargeImageText = mainWindow.LargeImageText.Text,
                        SmallImageKey = mainWindow.SmallImageKey.Text,
                        SmallImageText = mainWindow.SmallImageText.Text,
                        ShowTimestamp = mainWindow.ShowTimestampCheckbox.IsChecked ?? false,
                        ResetTimestamp = mainWindow.ResetTimestampCheckbox.IsChecked ?? false
                    }
                };
                SaveActivity(2, activity);
            }
        }

        private void ResetActivity2Button_Click(object sender, RoutedEventArgs e)
        {
            ResetActivity(2);
            Activity2.Text = "Activity 2";
        }

        private void LoadActivity2Button_Click(object sender, RoutedEventArgs e)
        {
            SetActivity(2);
        }

        #endregion

        #region Activity 3

        private void Activity3_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activity3_Placeholder.Content = Activity3.Text.Length > 0 ? "" : "Activity 3";
        }

        private void SaveActivity3Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null && !string.IsNullOrEmpty(Activity3.Text))
            {
                SavedActivity activity = new SavedActivity
                {
                    Name = Activity3.Text,
                    Activity = new Activity
                    {
                        ClientID = mainWindow.ClientID.Text,
                        Details = mainWindow.Details.Text,
                        State = mainWindow.State.Text,
                        LargeImageKey = mainWindow.LargeImageKey.Text,
                        LargeImageText = mainWindow.LargeImageText.Text,
                        SmallImageKey = mainWindow.SmallImageKey.Text,
                        SmallImageText = mainWindow.SmallImageText.Text,
                        ShowTimestamp = mainWindow.ShowTimestampCheckbox.IsChecked ?? false,
                        ResetTimestamp = mainWindow.ResetTimestampCheckbox.IsChecked ?? false
                    }
                };
                SaveActivity(3, activity);
            }
        }

        private void ResetActivity3Button_Click(object sender, RoutedEventArgs e)
        {
            ResetActivity(3);
            Activity3.Text = "Activity 3";
        }

        private void LoadActivity3Button_Click(object sender, RoutedEventArgs e)
        {
            SetActivity(3);
        }

        #endregion

        #region Activity 4

        private void Activity4_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activity4_Placeholder.Content = Activity4.Text.Length > 0 ? "" : "Activity 4";
        }

        private void SaveActivity4Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null && !string.IsNullOrEmpty(Activity4.Text))
            {
                SavedActivity activity = new SavedActivity
                {
                    Name = Activity4.Text,
                    Activity = new Activity
                    {
                        ClientID = mainWindow.ClientID.Text,
                        Details = mainWindow.Details.Text,
                        State = mainWindow.State.Text,
                        LargeImageKey = mainWindow.LargeImageKey.Text,
                        LargeImageText = mainWindow.LargeImageText.Text,
                        SmallImageKey = mainWindow.SmallImageKey.Text,
                        SmallImageText = mainWindow.SmallImageText.Text,
                        ShowTimestamp = mainWindow.ShowTimestampCheckbox.IsChecked ?? false,
                        ResetTimestamp = mainWindow.ResetTimestampCheckbox.IsChecked ?? false
                    }
                };
                SaveActivity(4, activity);
            }
        }

        private void ResetActivity4Button_Click(object sender, RoutedEventArgs e)
        {
            ResetActivity(4);
            Activity4.Text = "Activity 4";
        }

        private void LoadActivity4Button_Click(object sender, RoutedEventArgs e)
        {
            SetActivity(4);
        }

        #endregion

        #region Activity 5

        private void Activity5_TextChanged(object sender, TextChangedEventArgs e)
        {
            Activity5_Placeholder.Content = Activity5.Text.Length > 0 ? "" : "Activity 5";
        }

        private void SaveActivity5Button_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = Util.GetMainWindow();
            if (mainWindow != null && !string.IsNullOrEmpty(Activity5.Text))
            {
                SavedActivity activity = new SavedActivity
                {
                    Name = Activity5.Text,
                    Activity = new Activity
                    {
                        ClientID = mainWindow.ClientID.Text,
                        Details = mainWindow.Details.Text,
                        State = mainWindow.State.Text,
                        LargeImageKey = mainWindow.LargeImageKey.Text,
                        LargeImageText = mainWindow.LargeImageText.Text,
                        SmallImageKey = mainWindow.SmallImageKey.Text,
                        SmallImageText = mainWindow.SmallImageText.Text,
                        ShowTimestamp = mainWindow.ShowTimestampCheckbox.IsChecked ?? false,
                        ResetTimestamp = mainWindow.ResetTimestampCheckbox.IsChecked ?? false
                    }
                };
                SaveActivity(5, activity);
            }
        }

        private void ResetActivity5Button_Click(object sender, RoutedEventArgs e)
        {
            ResetActivity(5);
            Activity5.Text = "Activity 5";
        }

        private void LoadActivity5Button_Click(object sender, RoutedEventArgs e)
        {
            SetActivity(5);
        }

        #endregion

        private void SetActivity(int number)
        {
            if (Config.Current != null)
            {
                App app = Util.GetApp();
                if (app != null && app.Discord != null)
                {
                    app.Discord.StopPresence();
                    Config.Current.Activity = number;
                    app.Discord.CurrentActivity = Config.Current.GetActivity()?.Activity;
                    Config.Current.Save();
                    app.ChangePage(AppPage.Main);
                }
            }
        }

        private void SaveActivity(int number, SavedActivity activity)
        {
            if (Config.Current != null)
            {
                Config.Current.SaveActivity(number, activity);
                CooldownButtons();
            }
        }

        private void ResetActivity(int number)
        {
            if (Config.Current != null)
            {
                // Switch to default activity if current activity is reset
                if (Config.Current.Activity == number)
                {
                    Config.Current.Activity = 0;
                }
                Config.Current.SaveActivity(number, new SavedActivity());
                LoadActivities();
                CooldownButtons();
            }
        }

        private void CooldownButtons()
        {
            Keyboard.ClearFocus();

            SaveActivity1_Button.IsEnabled = false;
            SaveActivity1_Button.Opacity = 0.75f;
            ResetActivity1_Button.IsEnabled = false;
            ResetActivity1_Button.Opacity = 0.75f;

            SaveActivity2_Button.IsEnabled = false;
            SaveActivity2_Button.Opacity = 0.75f;
            ResetActivity2_Button.IsEnabled = false;
            ResetActivity2_Button.Opacity = 0.75f;

            SaveActivity3_Button.IsEnabled = false;
            SaveActivity3_Button.Opacity = 0.75f;
            ResetActivity3_Button.IsEnabled = false;
            ResetActivity3_Button.Opacity = 0.75f;

            SaveActivity4_Button.IsEnabled = false;
            SaveActivity4_Button.Opacity = 0.75f;
            ResetActivity4_Button.IsEnabled = false;
            ResetActivity4_Button.Opacity = 0.75f;

            SaveActivity5_Button.IsEnabled = false;
            SaveActivity5_Button.Opacity = 0.75f;
            ResetActivity5_Button.IsEnabled = false;
            ResetActivity5_Button.Opacity = 0.75f;

            Util.WaitAction(() =>
            {
                SaveActivity1_Button.IsEnabled = true;
                SaveActivity1_Button.Opacity = 1f;
                ResetActivity1_Button.IsEnabled = true;
                ResetActivity1_Button.Opacity = 1f;

                SaveActivity2_Button.IsEnabled = true;
                SaveActivity2_Button.Opacity = 1f;
                ResetActivity2_Button.IsEnabled = true;
                ResetActivity2_Button.Opacity = 1f;

                SaveActivity3_Button.IsEnabled = true;
                SaveActivity3_Button.Opacity = 1f;
                ResetActivity3_Button.IsEnabled = true;
                ResetActivity3_Button.Opacity = 1f;

                SaveActivity4_Button.IsEnabled = true;
                SaveActivity4_Button.Opacity = 1f;
                ResetActivity4_Button.IsEnabled = true;
                ResetActivity4_Button.Opacity = 1f;

                SaveActivity5_Button.IsEnabled = true;
                SaveActivity5_Button.Opacity = 1f;
                ResetActivity5_Button.IsEnabled = true;
                ResetActivity5_Button.Opacity = 1f;
            }, 1f);
        }
    }
}