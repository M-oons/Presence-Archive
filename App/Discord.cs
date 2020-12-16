using System;
using System.Diagnostics;
using System.Timers;
using System.Windows;
using DiscordRPC;
using DiscordRPC.Message;
using Presence.Activities;
using Presence.Utils;

namespace Presence
{
    public class Discord
    {
        public DiscordRpcClient Client { get; private set; }
        public bool PresenceActive { get; private set; }

        public bool ShouldResetTimestamp { get; set; } = true;
        public Activity CurrentActivity
        {
            get
            {
                return _currentActivity;
            }

            set
            {
                _currentActivity = value;
                Util.GetMainWindow()?.SetupInputs(_currentActivity);
            }
        }

        private Activity _currentActivity;
        private DateTime _timestamp;
        private string _lastClientID;
        private bool _ready;
        private bool _newProcess = true;

        public void Start()
        {
            if (Config.Current != null)
            {
                CurrentActivity = Config.Current.GetActivity()?.Activity;
                StartTimer();
                if (Config.Current.AutoStartPresence)
                {
                    UpdatePresence(CurrentActivity);
                }
            }
        }

        private void StartTimer()
        {
            Timer timer = new Timer(100f);
            timer.Elapsed += (s, e) =>
            {
                if (_ready && PresenceActive)
                {
                    SetPresence();
                }
                else
                {
                    ClearPresence();
                }
            };
            timer.Start();
        }

        public void Quit()
        {
            Client?.Dispose();
            Client = null;
        }

        public void ResetTimestamp()
        {
            _timestamp = DateTime.Now;
            ShouldResetTimestamp = false;
        }

        public void UpdatePresence(Activity activity)
        {
            if (activity != null && !string.IsNullOrEmpty(activity.ClientID))
            {
                if (_lastClientID != activity.ClientID)
                {
                    // Create new client
                    Client = new DiscordRpcClient(activity.ClientID)
                    {
                        SkipIdenticalPresence = true
                    };
                    Client.OnReady += Client_OnReady;
                    Client.Initialize();
                }
                PresenceActive = true;
                CurrentActivity = activity;
                _lastClientID = Client.ApplicationID;

                App app = Util.GetApp();
                MainWindow mainWindow = Util.GetMainWindow();

                if (mainWindow != null)
                {
                    // Hide start button
                    mainWindow.StartPresence_Button.IsEnabled = false;
                    mainWindow.StartPresence_Button.Opacity = 0.75f;
                    mainWindow.StartPresence_Button.Visibility = Visibility.Hidden;
                    mainWindow.StartPresence_Rectangle.Visibility = Visibility.Hidden;

                    // Show update button
                    mainWindow.UpdatePresence_Button.IsEnabled = false;
                    mainWindow.UpdatePresence_Button.Opacity = 0.75f;
                    mainWindow.UpdatePresence_Rectangle.Visibility = Visibility.Visible;
                    mainWindow.UpdatePresence_Button.Visibility = Visibility.Visible;

                    // Show stop button
                    mainWindow.StopPresence_Button.IsEnabled = false;
                    mainWindow.StopPresence_Button.Opacity = 0.75f;
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
                StopPresence();
            }
        }

        public void StopPresence()
        {
            PresenceActive = false;

            App app = Util.GetApp();
            MainWindow mainWindow = Util.GetMainWindow();

            if (mainWindow != null)
            {
                // Hide update button
                mainWindow.UpdatePresence_Button.IsEnabled = false;
                mainWindow.UpdatePresence_Button.Opacity = 0.75f;
                mainWindow.UpdatePresence_Button.Visibility = Visibility.Hidden;
                mainWindow.UpdatePresence_Rectangle.Visibility = Visibility.Hidden;

                // Hide stop button
                mainWindow.StopPresence_Button.IsEnabled = false;
                mainWindow.StopPresence_Button.Opacity = 0.75f;
                mainWindow.StopPresence_Button.Visibility = Visibility.Hidden;
                mainWindow.StopPresence_Rectangle.Visibility = Visibility.Hidden;

                // Show start button
                mainWindow.StartPresence_Button.IsEnabled = false;
                mainWindow.StartPresence_Button.Opacity = 0.75f;
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
            if (ShouldResetTimestamp)
            {
                ResetTimestamp();
            }
            Client?.SetPresence(CreatePresence());
        }

        private void ClearPresence()
        {
            Client?.ClearPresence();
        }

        private RichPresence CreatePresence()
        {
            if (Config.Current == null) return new RichPresence();
            SavedActivity savedActivity = Config.Current.GetActivity();
            Activity activity = savedActivity?.Activity;
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
            if (activity.ShowTimestamp) presence.Timestamps = new Timestamps(_timestamp);

            return presence;
        }

        private void WatchProcess()
        {
            Process process = Util.GetDiscordProcess();
            if (process != null)
            {
                process.EnableRaisingEvents = true;
                process.Exited += (s, e) =>
                {
                    _ready = false;
                    _newProcess = true; // next ready event will be fired when a new Discord process is started
                };
                _newProcess = false;
            }
        }

        private void Client_OnReady(object sender, ReadyMessage args)
        {
            _ready = true;

            Util.WaitAction(() =>
            {
                if (PresenceActive)
                {
                    ClearPresence();
                    SetPresence();
                }
            }, _newProcess ? 0f : 30f); // wait 30 seconds before setting presence if Discord was restarted with Ctrl + R

            if (_newProcess)
            {
                WatchProcess();
            }
        }
    }
}