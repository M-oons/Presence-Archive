using System.IO;
using Newtonsoft.Json;
using Presence.Utils;

namespace Presence
{
    public class Config
    {
        public bool AutoStartPresence { get; set; }
        public Activity Activity { get; set; } = new Activity();

        public static Config Current { get; set; }

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
        }

        public void Save()
        {
            Util.CreateRootFolder();
            File.WriteAllText(Util.ConfigLocation, JsonConvert.SerializeObject(this, Formatting.Indented));
            Current = this;
        }

        private static void TryCreateConfig()
        {
            Util.CreateRootFolder();
            if (!File.Exists(Util.ConfigLocation))
            {
                File.WriteAllText(Util.ConfigLocation, JsonConvert.SerializeObject(new Config(), Formatting.Indented));
            }
        }
    }
}