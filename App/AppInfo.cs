using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Presence.Enums;
using Presence.Utils;

namespace Presence
{
    public static class AppInfo
    {
        public const string NAME = "Presence";
        public const string VERSION = "1.2.2";
        public const string URL = "https://github.com/M-oons/Presence";

        public static async void CheckForUpdate(bool alertNoUpdate = false)
        {
            UpdateResult result = await GetUpdateResult();
            switch (result.Type)
            {
                case UpdateResultType.Update:
                    string url = result.DownloadURL;
                    if (!string.IsNullOrEmpty(url))
                    {
                        Util.Popup("Update Available", "A newer version of Presence is available.\n\nWould you like to download the latest version?", new Action(() =>
                        {
                            Util.StartProcess(url);
                        }));
                    }
                    break;

                case UpdateResultType.NoUpdate:
                    if (alertNoUpdate)
                    {
                        Util.Info("No Update Available", "You are already running the latest version of Presence.");
                    }
                    break;

                case UpdateResultType.Error:
                default:
                    if (alertNoUpdate)
                    {
                        Util.Warning("Error Checking For Update", "An error occured when checking for the latest version of Presence.");
                    }
                    break;
            }
        }

        public static void CheckInstance()
        {
            if (Util.GetProcesses("Presence").Length > 1) // Presence is already running in another process
            {
                Util.Warning("Already running", "An instance of Presence is already running in another process.", new Action(() =>
                {
                    Util.GetApp()?.Quit();
                }));
            }
        }

        private static async Task<UpdateResult> GetUpdateResult()
        {
            HttpResponseMessage response = await API.Get("/repos/M-oons/Presence/releases/latest");
            if (response.IsSuccessStatusCode)
            {
                string content = await response.Content.ReadAsStringAsync();
                JObject data = JsonConvert.DeserializeObject<JObject>(content);

                if (data != null && data.TryGetValue("tag_name", out JToken tagToken))
                {
                    string latest = tagToken.ToString();
                    if (CompareVersion(latest) < 0 && data.TryGetValue("assets", out JToken assetsToken)) // newer version is available
                    {
                        if (assetsToken is JArray assets && assets.Count > 0 // "assets" is an array
                            && assets.First is JObject asset // "asset" is an object
                            && asset.TryGetValue("browser_download_url", out JToken urlToken))
                        {
                            string url = urlToken.ToString();
                            return UpdateResult.Update(url);
                        }
                    }
                    else // already running latest version
                    {
                        return UpdateResult.NoUpdate();
                    }
                }
            }
            return UpdateResult.Error();
        }

        private static int CompareVersion(string version)
        {
            // -1 : Current version is older than compared version
            //  0 : Current version is same as compared version
            //  1 : Current version is newer than compared version

            Version currentVersion = new Version(VERSION);
            Version compareVersion = new Version(version);

            return currentVersion.CompareTo(compareVersion);
        }
    }
}