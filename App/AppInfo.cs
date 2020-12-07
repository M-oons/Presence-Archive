using System;
using System.Collections.Generic;
using System.Net.Http;
using Newtonsoft.Json;
using Presence.Utils;

namespace Presence
{
    public static class AppInfo
    {
        public const string NAME = "Presence";
        public const string VERSION = "1.1.0";
        public const string URL = "https://github.com/M-oons/Presence";

        private static readonly HttpClient _client;

        static AppInfo()
        {
            _client = new HttpClient
            {
                BaseAddress = new Uri("https://api.github.com")
            };
            _client.DefaultRequestHeaders.Clear();
            _client.DefaultRequestHeaders.Add("Accept", "application/vnd.github.v3+json"); // Use GitHub API v3
            _client.DefaultRequestHeaders.Add("User-Agent", "M-oons/Presence"); // GitHub API requires User-Agent header
        }

        public static async void CheckForUpdate()
        {
            HttpResponseMessage response = await _client.GetAsync("/repos/M-oons/Presence/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                Dictionary<string, object> data = JsonConvert.DeserializeObject<Dictionary<string, object>>(content);

                if (data != null && data.TryGetValue("tag_name", out object tag))
                {
                    string latest = tag.ToString();
                    if (CompareVersion(latest) == 1) // newer version is available
                    {
                        Util.Popup("Update Available", "A newer version of Presence is available.\n\nWould you like to open the GitHub releases page?", new Action(() =>
                        {
                            Util.StartProcess($"{URL}/releases");
                        }));
                    }
                }
            }
        }

        public static void CheckInstance()
        {
            if (Util.GetProcesses("Presence").Length > 1) // Presence is already running in another process
            {
                Util.Alert("Already running", "An instance of Presence is already running in another process.", new Action(() =>
                {
                    Util.GetApp()?.Quit();
                }));
            }
        }

        private static int CompareVersion(string version)
        {
            //  1 : Compared version is newer than current
            //  0 : Compared version is same as current
            // -1 : Compared version is older than current

            int currentVersion = int.TryParse(VERSION.Replace(".", ""), out int current) ? current : 0;
            int compareVersion = int.TryParse(version.Replace(".", ""), out int compare) ? compare : 0;

            int result = -1;

            if (compareVersion > currentVersion)
            {
                result = 1;
            }
            else if (compareVersion == currentVersion)
            {
                result = 0;
            }
            return result;
        }
    }
}