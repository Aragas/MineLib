using System.Collections.Generic;

namespace Aragas.QServer.Prometheus.Exporter
{
    public interface IPingService
    {
        List<ServiceEntry> Services { get; }
    }
}