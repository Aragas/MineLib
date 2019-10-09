using System;
using System.Collections.Generic;

using MineLib.Core;
using MineLib.Core.Exceptions;
using MineLib.Core.Loader;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol : IModAPIContext
    {
        private List<ModAPI> ModAPIs { get; } = new List<ModAPI>();


        private void LoadForgeModAPI(AssemblyInfo assemblyInfo)
        {
            var modAPIType = AssemblyParser.FindType<ModAPI>(assemblyInfo, "ModForge_1.7.10");
            if (modAPIType == null)
                throw new NetworkHandlerException($"ModAPI loading error: {assemblyInfo.FileName} was not found or corrupted.");

            try { ModAPIs.Add((ModAPI) Activator.CreateInstance(modAPIType, ModAPISide.Client, this)); }
            catch (MissingMemberException) { throw new NetworkHandlerException("Protocol not supported."); }
        }
    }
}
