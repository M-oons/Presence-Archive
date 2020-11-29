using System;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Message;
using Presence.Utils;

namespace Presence
{
    public class Discord
    {
        public DiscordRpcClient Client { get; private set; }
        public bool PresenceActive { get; private set; }

        private string _lastClientID;
        private DateTime _timestamp;

        public Discord()
        {
            ResetTimestamp();
        }

        public void Start()
        {
            if (Config.Current != null && Config.Current.Activity != null)
            {
                Util.GetApp().SetupInputs();
                if (Config.Current.AutoStartPresence) UpdatePresence();
            }
        }

        public void Quit()
        {
            Client?.Dispose();
            Client = null;
        }

        public void ResetTimestamp()
        {
            _timestamp = DateTime.Now;
        }

        public void UpdatePresence()
        {
            if (Config.Current != null && Config.Current.Activity != null && !string.IsNullOrEmpty(Config.Current.Activity.ClientID))
            {
                // Clear existing presence
                if (Client != null && !Client.IsDisposed) Client.ClearPresence();

                if (_lastClientID == Config.Current.Activity.ClientID)
                {
                    // Update existing client
                    SetPresence();
                }
                else
                {
                    // Create new client
                    Client = new DiscordRpcClient(Config.Current.Activity.ClientID);
                    Client.OnReady += Client_OnReady;
                    Client.Initialize();
                }
                _lastClientID = Client.ApplicationID;

                App app = Util.GetApp();
                MainWindow mainWindow = Util.GetMainWindow();

                if (mainWindow != null)
                {
                    // Hide start button
                    mainWindow.StartPresence_Button.IsEnabled = false;
                    mainWindow.StartPresence_Button.Opacity = 0.5f;
                    mainWindow.StartPresence_Button.Visibility = Visibility.Hidden;
                    mainWindow.StartPresence_Rectangle.Visibility = Visibility.Hidden;

                    // Show update button
                    mainWindow.UpdatePresence_Button.IsEnabled = false;
                    mainWindow.UpdatePresence_Button.Opacity = 0.5f;
                    mainWindow.UpdatePresence_Rectangle.Visibility = Visibility.Visible;
                    mainWindow.UpdatePresence_Button.Visibility = Visibility.Visible;

                    // Show stop button
                    mainWindow.StopPresence_Button.IsEnabled = false;
                    mainWindow.StopPresence_Button.Opacity = 0.5f;
                    mainWindow.StopPresence_Rectangle.Visibility = Visibility.Visible;
                    mainWindow.StopPresence_Button.Visibility = Visibility.Visible;

                    Util.WaitAction(() =>
                    {
                        mainWindow.UpdatePresence_Button.IsEnabled = true;
                        mainWindow.UpdatePresence_Button.Opacity = 1f;
                        mainWindow.StopPresence_Button.IsEnabled = true;
                        mainWindow.StopPresence_Button.Opacity = 1f;
                    }, 2f);
                }

                if (app != null)
                {
                    // Update tray icon
                    app.Tray.Icon = Properties.Resources.WumpusOnIcon;
                }
            }
            else
            {
                // Clear presence if config is invalid
                StopPresence();
            }
        }

        public void StopPresence()
        {
            Client?.ClearPresence();
            PresenceActive = false;

            App app = Util.GetApp();
            MainWindow mainWindow = Util.GetMainWindow();
            
            if (mainWindow != null)
            {
                // Hide update button
                mainWindow.UpdatePresence_Button.IsEnabled = false;
                mainWindow.UpdatePresence_Button.Opacity = 0.5f;
                mainWindow.UpdatePresence_Button.Visibility = Visibility.Hidden;
                mainWindow.UpdatePresence_Rectangle.Visibility = Visibility.Hidden;

                // Hide stop button
                mainWindow.StopPresence_Button.IsEnabled = false;
                mainWindow.StopPresence_Button.Opacity = 0.5f;
                mainWindow.StopPresence_Button.Visibility = Visibility.Hidden;
                mainWindow.StopPresence_Rectangle.Visibility = Visibility.Hidden;

                // Show start button
                mainWindow.StartPresence_Button.IsEnabled = false;
                mainWindow.StartPresence_Button.Opacity = 0.5f;
                mainWindow.StartPresence_Rectangle.Visibility = Visibility.Visible;
                mainWindow.StartPresence_Button.Visibility = Visibility.Visible;

                Util.WaitAction(() =>
                {
                    mainWindow.StartPresence_Button.IsEnabled = true;
                    mainWindow.StartPresence_Button.Opacity = 1f;
                }, 2f);
            }

            if (app != null)
            {
                // Update tray icon
                app.Tray.Icon = Properties.Resources.WumpusOffIcon;
            }
        }

        private void SetPresence()
        {
            Client?.SetPresence(CreatePresence());
            PresenceActive = true;
        }

        private RichPresence CreatePresence()
        {
            if (Config.Current == null) return new RichPresence();
            Activity activity = Config.Current.Activity;
            if (activity == null) return new RichPresence();

            // Create presence from config
            RichPresence presence = new RichPresence();
            Assets assets = new Assets();
            if (!string.IsNullOrEmpty(activity.Details) && activity.Details.Length > 2) presence.Details = activity.Details;
            if (!string.IsNullOrEmpty(activity.State) && activity.State.Length > 2) presence.State = activity.State;
            if (!string.IsNullOrEmpty(activity.LargeImageKey)) assets.LargeImageKey = activity.LargeImageKey;
            if (!string.IsNullOrEmpty(activity.LargeImageText)) assets.LargeImageText = activity.LargeImageText;
            if (!string.IsNullOrEmpty(activity.SmallImageKey)) assets.SmallImageKey = activity.SmallImageKey;
            if (!string.IsNullOrEmpty(activity.SmallImageText)) assets.SmallImageText = activity.SmallImageText;
            presence.Assets = assets;
            if (activity.ResetTimestamp) _timestamp = DateTime.Now;
            if (activity.ShowTimestamp) presence.Timestamps = new Timestamps(_timestamp);

            return presence;
        }

        private void Client_OnReady(object sender, ReadyMessage args)
        {
            SetPresence();
        }
    }
}