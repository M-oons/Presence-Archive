namespace Presence
{
    public class Activity
    {
        public string ClientID { get; set; } = "";
        public string Details { get; set; } = "";
        public string State { get; set; } = "";
        public string LargeImageKey { get; set; } = "";
        public string LargeImageText { get; set; } = "";
        public string SmallImageKey { get; set; } = "";
        public string SmallImageText { get; set; } = "";
        public bool ShowTimestamp { get; set; }
        public bool ResetTimestamp { get; set; }

        public bool Equals(Activity other)
        {
            if (other == null) return false;
            return
                ClientID == other.ClientID &&
                Details == other.Details &&
                State == other.State &&
                LargeImageKey == other.LargeImageKey &&
                LargeImageText == other.LargeImageText &&
                SmallImageKey == other.SmallImageKey &&
                SmallImageText == other.SmallImageText &&
                ShowTimestamp == other.ShowTimestamp &&
                ResetTimestamp == other.ResetTimestamp;
        }
    }
}