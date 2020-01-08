using System;
using System.ComponentModel;

namespace Aragas.QServer.GUI
{
    public class Entry : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public string ServiceType { get; set; }
        public Guid ServiceId { get; set; }
        public int NotFoundCounter { get; set; }

        public Entry(string serviceType, Guid serviceId)
        {
            ServiceType = serviceType;
            ServiceId = serviceId;
        }

        public override string ToString() => $"{ServiceType}: {ServiceId}";
    }
}