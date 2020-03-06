using Microsoft.Extensions.DependencyInjection;

namespace Aragas.QServer.GUI.ViewModels
{
    public class ViewModelLocator
    {
        public static MainViewModel MainViewModel => App.ServiceProvider.GetRequiredService<MainViewModel>();

        public static ServicesViewModel ServicesViewModel => App.ServiceProvider.GetRequiredService<ServicesViewModel>();
    }
}