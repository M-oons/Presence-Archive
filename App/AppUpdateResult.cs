namespace Presence
{
    public struct AppUpdateResult
    {
        public ResultType Type { get; }
        public string DownloadURL { get; }

        private AppUpdateResult(ResultType type, string downloadURL)
        {
            Type = type;
            DownloadURL = downloadURL;
        }

        public static AppUpdateResult Update(string downloadURL)
        {
            return new AppUpdateResult(ResultType.Update, downloadURL);
        }

        public static AppUpdateResult NoUpdate()
        {
            return new AppUpdateResult(ResultType.NoUpdate, null);
        }

        public static AppUpdateResult Error()
        {
            return new AppUpdateResult(ResultType.Error, null);
        }

        public enum ResultType
        {
            NoUpdate,
            Update,
            Error
        }
    }
}