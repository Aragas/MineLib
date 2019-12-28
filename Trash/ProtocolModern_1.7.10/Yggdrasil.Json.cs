using Newtonsoft.Json;

namespace ProtocolModern
{
    public static partial class Yggdrasil
    {
        public enum ErrorType
        {
            ForbiddenOperationException,
            IllegalArgumentException,
            UnsupportedMediaType
        }

        #region Response

        /*
        public struct AvailableProfiles
        {
            [JsonProperty("id")]
            public string ID { get; set; }

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("legacy")]
            public bool Legacy { get; set; }
        }
        */

        public struct SelectedProfile
        {
            [JsonProperty("id")]
            public string ID { get; private set; }

            [JsonProperty("name")]
            public string Name { get; private set; }

            [JsonProperty("legacy")]
            public bool Legacy { get; private set; }
        }

        public struct AuthenticateInfo
        {
            public static AuthenticateInfo Empty => new AuthenticateInfo();

            [JsonProperty("accessToken")]
            public string AccessToken { get; private set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; private set; }

            [JsonProperty("selectedProfile")]
            public SelectedProfile Profile { get; private set; }

            /*
            [JsonProperty("availableProfiles")]
            public List<AvailableProfiles> AvailableProfiles { get; set; }
            */
        }

        public struct Error
        {
            [JsonProperty("error")]
            public ErrorType ErrorDescription { get; private set; }

            [JsonProperty("errorMessage")]
            public string ErrorMessage { get; private set; }

            [JsonProperty("cause")]
            public string Cause { get; private set; }
        }

        public class YggdrasilAuthenticate
        {
            public YggdrasilResponse Status { get; private set; }

            public AuthenticateInfo Info { get; private set; }

            public YggdrasilAuthenticate(YggdrasilResponse status, AuthenticateInfo info)
            {
                Status = status;
                Info = info;
            }
        }

        public class YggdrasilStatus
        {
            public YggdrasilResponse Status { get; private set; }

            public bool Response { get; private set; }

            public YggdrasilStatus(YggdrasilResponse status, bool response)
            {
                Status = status;
                Response = response;
            }
        }

        #endregion Response

        #region Requests

        private struct Agent
        {
            public static Agent Minecraft => new Agent { Name = "Minecraft", Version = 1 };

            [JsonProperty("name")]
            public string Name { get; set; }

            [JsonProperty("version")]
            public int Version { get; set; }
        }

        private struct JsonLogin
        {
            [JsonProperty("agent")]
            public Agent Agent { get; set; }

            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }

        private struct JsonRefreshSession
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
        }

        private struct JsonVerifySession
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }
        }

        private struct JsonLogout
        {
            [JsonProperty("username")]
            public string Username { get; set; }

            [JsonProperty("password")]
            public string Password { get; set; }
        }

        private struct JsonInvalidate
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("clientToken")]
            public string ClientToken { get; set; }
        }

        private struct JsonClientAuth
        {
            [JsonProperty("accessToken")]
            public string AccessToken { get; set; }

            [JsonProperty("selectedProfile")]
            public string SelectedProfile { get; set; }

            [JsonProperty("serverId")]
            public string ServerID { get; set; }
        }

        #endregion Requests
    }
}
