using Aragas.QServer.GUI.ViewModels;
using Aragas.QServer.NetworkBus;

using Caliburn.Micro;

using NATS.Client;

using System;
using System.Collections.Generic;
using System.Windows;

namespace Aragas.QServer.GUI
{
    public class Bootstrapper : BootstrapperBase
    {
        private SimpleContainer _container;

        public Bootstrapper()
        {
            Initialize();
        }

        protected override void Configure()
        {
            _container = new SimpleContainer();

            _container.Instance(ConnectionFactory.GetDefaultOptions().SetDefaultArgs());
            _container.Instance<IAsyncNetworkBus>(new AsyncNATSBus(_container.GetInstance<Options>()));
            _container.Instance<INetworkBus>(_container.GetInstance<IAsyncNetworkBus>());

            _container.Singleton<IWindowManager, WindowManager>();

            _container.PerRequest<ShellViewModel>();
        }

        protected override void OnStartup(object sender, StartupEventArgs e)
        {
            DisplayRootViewFor<ShellViewModel>();
        }

        protected override object GetInstance(Type service, string key)
        {
            return _container.GetInstance(service, key);
        }

        protected override IEnumerable<object> GetAllInstances(Type service)
        {
            return _container.GetAllInstances(service);
        }

        protected override void BuildUp(object instance)
        {
            _container.BuildUp(instance);
        }
    }
}