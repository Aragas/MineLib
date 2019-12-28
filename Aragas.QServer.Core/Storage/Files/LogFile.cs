using Aragas.QServer.Core.Storage.Folders;

using PCLExt.FileStorage;

using System;

namespace Aragas.QServer.Core.Storage.Files
{
    public class LogFile : BaseFile
    {
        public LogFile() : base(new LogsFolder().CreateFile($"{DateTime.Now:yyyy-MM-dd_HH.mm.ss}.log", CreationCollisionOption.OpenIfExists)) { }
    }
}