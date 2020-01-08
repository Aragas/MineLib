using Aragas.QServer.Core;
using Aragas.QServer.Core.NetworkBus.Messages;

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace Aragas.QServer.GUI.ViewModels
{
    public class ServicesViewModel : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged;

        public ObservableCollection<Entry> ComboBoxItems { get; } = new ObservableCollection<Entry>();
        public Entry ComboBoxSelectedItem { get; set; }

        public string Text { get; set; }

        public ICommand MenuItemPrometheusCommand => new TaskCommand(async _ =>
        {
            var message = await BaseSingleton.Instance.PublishAndWaitForReplyAsync<AppMetricsPrometheusRequestMessage, AppMetricsPrometheusResponseMessage>(
                new AppMetricsPrometheusRequestMessage(), ComboBoxSelectedItem.ServiceId);

            Application.Current.Dispatcher.Invoke(() => Text = message == null ? "Received null!" : message.Report);
        });
        public ICommand MenuItemHealthCommand => new TaskCommand(async _ =>
        {
            var message = await BaseSingleton.Instance.PublishAndWaitForReplyAsync<AppMetricsHealthRequestMessage, AppMetricsHealthResponseMessage>(
                new AppMetricsHealthRequestMessage(), ComboBoxSelectedItem.ServiceId);

            Application.Current.Dispatcher.Invoke(() => Text = message == null ? "Received null!" : message.Report);
        });
        public ICommand MenuItemCopyCommand => new TaskCommand(_ =>
        {
            Clipboard.SetText(ComboBoxSelectedItem.ServiceId.ToString());
            return Task.CompletedTask;
        });

        public ServicesViewModel()
        {
            var token = CancellationToken.None;
            Task.Factory.StartNew(async () =>
            {
                while (!token.IsCancellationRequested)
                {
                    var toRemove = new List<Entry>();
                    foreach (var item in ComboBoxItems)
                    {
                        item.NotFoundCounter++;
                        if (item.NotFoundCounter == 2)
                            toRemove.Add(item);
                    }
                    foreach (var item in toRemove)
                        Application.Current.Dispatcher.Invoke(() => ComboBoxItems.Remove(item));

                    await BaseSingleton.Instance.PublishAsync(new ServicesPingMessage());
                    await Task.Delay(2000, token);
                }
            }, token, TaskCreationOptions.LongRunning, TaskScheduler.Default);

            BaseSingleton.Instance.Subscribe<ServicesPongMessage>(message =>
            {
                var item = ComboBoxItems.FirstOrDefault(i => i.ServiceType == message.ServiceType && i.ServiceId == message.ServiceId);
                if (item == null)
                    Application.Current.Dispatcher.Invoke(() => ComboBoxItems.Add(new Entry(message.ServiceType, message.ServiceId)));
                else
                    item.NotFoundCounter = 0;
            });
        }
    }
}