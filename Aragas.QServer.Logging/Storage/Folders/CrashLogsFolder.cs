using PCLExt.FileStorage;

namespace Aragas.QServer.Core.Storage.Folders
{
    public class CrashLogsFolder : BaseFolder
    {
        public CrashLogsFolder() : base(new LogsFolder().CreateFolder("CrashLogs", CreationCollisionOption.OpenIfExists)) { }
    }
}