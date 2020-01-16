using System;

namespace Aragas.QServer.Core.Data
{
    public class ServiceOptions
    {
        public Guid Uid { get; } = Guid.NewGuid();
        public string Name { get; set; } = "NOT_SPECIFIED";
    }
}