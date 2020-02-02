namespace MineLib.Server.Heartbeat.Models.ManageViewModels
{
    public class RemoveLoginViewModel
    {
        public string LoginProvider { get; set; } = default!;
        public string ProviderKey { get; set; } = default!;
    }
}