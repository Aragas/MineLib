using PCLExt.FileStorage;
using PCLExt.FileStorage.Folders;

namespace Aragas.QServer.Core.Storage.Folders
{
    public class LogsFolder : BaseFolder
    {
        public LogsFolder() : base(new ApplicationRootFolder().CreateFolder("Logs", CreationCollisionOption.OpenIfExists)) { }
    }
}