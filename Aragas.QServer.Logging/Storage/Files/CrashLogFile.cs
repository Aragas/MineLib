using Aragas.QServer.Core.Storage.Folders;

using PCLExt.FileStorage;

using System;

namespace Aragas.QServer.Core.Storage.Files
{
    public class CrashLogFile : BaseFile
    {
        public CrashLogFile() : base(new CrashLogsFolder().CreateFile($"{DateTime.UtcNow:yyyy-MM-dd_HH.mm.ss}.log", CreationCollisionOption.OpenIfExists)) { }
    }
}