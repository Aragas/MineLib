using Aragas.QServer.Prometheus.Exporter.Data;

using System.Collections.Generic;

namespace Aragas.QServer.Prometheus.Exporter.Services
{
    public interface IPingService
    {
        List<ServiceEntry> Services { get; }
    }
}