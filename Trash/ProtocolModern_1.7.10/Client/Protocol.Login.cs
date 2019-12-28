using System.Threading.Tasks;

namespace ProtocolModern.Client
{
    public sealed partial class Protocol
    {
        private string YggLogin { get; set; }
        private string YggPassword { get; set; } // -- Say thanks to Mojang https://bugs.mojang.com/browse/WEB-327

        internal string AccessToken { get; private set; }
        internal string ClientToken { get; private set; }
        internal string SelectedProfile { get; private set; }


        public override async Task<bool> Login(string login, string password)
        {
            YggLogin = login;
            YggPassword = password;

            var result = await Yggdrasil.Authenticate(YggLogin, YggPassword);

            switch (result.Status)
            {
                case YggdrasilResponse.Success:
                    AccessToken                 = result.Info.AccessToken;
                    ClientToken                 = result.Info.ClientToken;
                    SelectedProfile             = result.Info.Profile.ID;
                    Client.Username       = result.Info.Profile.Name;
                    return true;

                default:
                    AccessToken                 = "None";
                    ClientToken                 = "None";
                    SelectedProfile             = "None";
                    Client.Username       = "None";
                    return false;
            }
        }

        /// <summary>
        /// Uses a client's stored credentials to verify with Minecraft.net
        /// </summary>
        public async Task<bool> RefreshSession()
        {
            var result = await Yggdrasil.RefreshSession(AccessToken, ClientToken);

            switch (result.Status)
            {
                case YggdrasilResponse.Success:
                    AccessToken = result.Info.AccessToken;
                    ClientToken = result.Info.ClientToken;
                    return true;

                default:
                    return false;
            }
        }

        public async Task<bool> VerifySession() => (await Yggdrasil.VerifySession(AccessToken)).Response;

        public async Task<bool> Invalidate() => (await Yggdrasil.Invalidate(AccessToken, ClientToken)).Response;

        public override async Task<bool> Logout()
        {
            return (await Yggdrasil.Logout(YggLogin, YggPassword)).Response;
        }
    }
}
