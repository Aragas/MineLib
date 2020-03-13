using System;

namespace Aragas.QServer.Prometheus.Exporter.Data
{
    public class ServiceEntry
    {
        public string ServiceType { get; }
        public Guid ServiceId { get; }
        public int NotFoundCounter { get; set; }

        public ServiceEntry(string serviceType, Guid serviceId)
        {
            ServiceType = serviceType;
            ServiceId = serviceId;
        }

        public override string ToString() => $"{ServiceType}: {ServiceId}";
    }
}