using Microsoft.Extensions.DependencyInjection;

namespace Aragas.QServer.GUI.ViewModels
{
    public class ViewModelLocator
    {
        public MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

        public ServicesViewModel ServicesViewModel => App.ServiceProvider.GetRequiredService<ServicesViewModel>();
    }
}