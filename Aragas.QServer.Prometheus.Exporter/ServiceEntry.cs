using System;

namespace Aragas.QServer.Prometheus.Exporter
{
    public class ServiceEntry
    {
        public string ServiceType { get; set; }
        public Guid ServiceId { get; set; }
        public int NotFoundCounter { get; set; }

        public ServiceEntry(string serviceType, Guid serviceId)
        {
            ServiceType = serviceType;
            ServiceId = serviceId;
        }

        public override string ToString() => $"{ServiceType}: {ServiceId}";
    }
}