using System.Collections.Generic;
using System.IO;
using System.Linq;
using Newtonsoft.Json;
using Presence.Activities;
using Presence.Utils;

namespace Presence
{
    public class Config
    {
        public bool AutoStartPresence { get; set; }
        public int Activity { get; set; }
        public SavedActivity[] Activities { get; set; } = new SavedActivity[0];

        public static Config Current { get; set; }

        public SavedActivity GetActivity()
        {
            if (Current == null || Current.Activities == null || Current.Activities.Length == 0 || Current.Activities.Length <= Current.Activity || Current.Activity < 0) return new SavedActivity { Name = "Default", Activity = new Activity() };
            return Current.Activities[Current.Activity];
        }

        public void SaveActivity(int index, SavedActivity activity)
        {
            if (index < 0 || index > 5) return;

            SetupActivities();

            if (activity == null)
            {
                activity = new SavedActivity();
            }
            if (string.IsNullOrEmpty(activity.Name))
            {
                activity.Name = index == 0 ? "Default" : $"Activity {index}";
            }

            // Save new activity
            Activities[index] = activity;

            // Clamp current activity index
            if (Activity < 0)
            {
                Activity = 0;
            }
            else if (Activity >= Activities.Length)
            {
                Activity = Activities.Length - 1;
            }

            Save();
        }

        public void SetupActivities()
        {
            if (Activities == null)
            {
                Activities = new SavedActivity[0];
            }

            List<SavedActivity> activityList = Activities.ToList();

            // Ensure there is always 1 default activity and 5 custom activities saved
            for (int i = 0; i <= 5; i++)
            {
                if (activityList.Count <= i)
                {
                    activityList.Add(new SavedActivity
                    {
                        Name = i == 0 ? "Default" : $"Activity {i}",
                        Activity = new Activity()
                    });
                }
                else if (activityList[i] == null)
                {
                    activityList[i] = new SavedActivity
                    {
                        Name = i == 0 ? "Default" : $"Activity {i}",
                        Activity = new Activity()
                    };
                }
                else if (string.IsNullOrEmpty(activityList[i].Name))
                {
                    activityList[i].Name = $"Activity {i}";
                }
            }

            Activities = activityList.ToArray();
        }

        public bool UsingSavedActivity()
        {
            return Activities != null && Activities.Length > 1 && Activity > 0 && Activity < Activities.Length;
        }

        public void Save()
        {
            Util.CreateDataFolder();
            File.WriteAllText(Util.ConfigLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            Current = this;
        }

        public static void Load()
        {
            TryCreateConfig();
            string json = File.ReadAllText(Util.ConfigLocation);
            Config config;
            try
            {
                config = JsonConvert.DeserializeObject<Config>(json);
            }
            catch
            {
                config = new Config(); // corrupt config, use default config instead
            }
            Current = config;
            Current.SetupActivities();
            Current.Save();
        }

        private static void TryCreateConfig()
        {
            Util.CreateDataFolder();
            if (!File.Exists(Util.ConfigLocation))
            {
                File.WriteAllText(Util.ConfigLocation, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
        }
    }
}