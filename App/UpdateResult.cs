using Presence.Enums;

namespace Presence
{
    public struct UpdateResult
    {
        public UpdateResultType Type { get; }
        public string DownloadURL { get; }

        private UpdateResult(UpdateResultType type, string downloadURL)
        {
            Type = type;
            DownloadURL = downloadURL;
        }

        public static UpdateResult Update(string downloadURL)
        {
            return new UpdateResult(UpdateResultType.Update, downloadURL);
        }

        public static UpdateResult NoUpdate()
        {
            return new UpdateResult(UpdateResultType.NoUpdate, null);
        }

        public static UpdateResult Error()
        {
            return new UpdateResult(UpdateResultType.Error, null);
        }
    }
}