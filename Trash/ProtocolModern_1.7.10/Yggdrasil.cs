using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

namespace ProtocolModern
{
    public enum YggdrasilResponse
    {
        Error,
        Success,

        AccountMigrated,
        WrongPassword,
        TooMuchAttempts,
        InvalidToken
    }

    public static partial class Yggdrasil
    {
        /// <summary>
        /// Authenticates a user using their <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        public static async Task<YggdrasilAuthenticate> Authenticate(string username, string password)
        {
            var json = JsonConvert.SerializeObject(new JsonLogin
            {
                Agent = Agent.Minecraft,
                Username = username,
                Password = password
                // "clientToken": "client identifier"     // optional
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://authserver.mojang.com/authenticate"), new StringContent(json, Encoding.UTF8, "application/json")))
            using (var content = response.Content)
            {
                var result = await content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    return new YggdrasilAuthenticate(YggdrasilResponse.Success, JsonConvert.DeserializeObject<AuthenticateInfo>(result));
                else
                    return new YggdrasilAuthenticate(await HandleHttpResponseMessage(response), AuthenticateInfo.Empty);
            }
        }

        /// <summary>
        /// Refreshes a valid <paramref name="accessToken"/>.
        /// It can be used to keep a user logged in between gaming sessions.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        public static async Task<YggdrasilAuthenticate> RefreshSession(string accessToken, string clientToken)
        {
            var json = JsonConvert.SerializeObject(new JsonRefreshSession
            {
                AccessToken = accessToken,
                ClientToken = clientToken,
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://authserver.mojang.com/refresh"), new StringContent(json, Encoding.UTF8, "application/json")))
            using (var content = response.Content)
            {
                var result = await content.ReadAsStringAsync();

                if (response.StatusCode == HttpStatusCode.OK)
                    return new YggdrasilAuthenticate(YggdrasilResponse.Success, JsonConvert.DeserializeObject<AuthenticateInfo>(result));
                else
                    return new YggdrasilAuthenticate(await HandleHttpResponseMessage(response), AuthenticateInfo.Empty);
            }
        }


        /// <summary>
        /// Checks if an <paramref name="accessToken"/> is a valid session token with a currently-active session.
        /// </summary>
        /// <param name="accessToken"></param>
        public static async Task<YggdrasilStatus> VerifySession(string accessToken)
        {
            var json = JsonConvert.SerializeObject(new JsonVerifySession
            {
                AccessToken = accessToken
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://authserver.mojang.com/validate"), new StringContent(json, Encoding.UTF8, "application/json")))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new YggdrasilStatus(YggdrasilResponse.Success, true);
                else
                    return new YggdrasilStatus(await HandleHttpResponseMessage(response), true);
            }
        }

        /// <summary>
        /// Invalidates accessTokens using an account's <paramref name="username"/> and <paramref name="password"/>.
        /// </summary>
        /// <param name="username">Login</param>
        /// <param name="password">Password</param>
        public static async Task<YggdrasilStatus> Logout(string username, string password)
        {
            var json = JsonConvert.SerializeObject(new JsonLogout
            {
                Username = username,
                Password = password
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://authserver.mojang.com/signout"), new StringContent(json, Encoding.UTF8, "application/json")))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new YggdrasilStatus(YggdrasilResponse.Success, true);
                else
                    return new YggdrasilStatus(await HandleHttpResponseMessage(response), true);
            }
        }

        /// <summary>
        /// Invalidates <paramref name="accessToken"/>  using a <paramref name="clientToken"/>/<paramref name="accessToken"/> pair.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="clientToken"></param>
        public static async Task<YggdrasilStatus> Invalidate(string accessToken, string clientToken)
        {
            var json = JsonConvert.SerializeObject(new JsonInvalidate
            {
                AccessToken = accessToken,
                ClientToken = clientToken
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://authserver.mojang.com/invalidate"), new StringContent(json, Encoding.UTF8, "application/json")))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new YggdrasilStatus(YggdrasilResponse.Success, true);
                else
                    return new YggdrasilStatus(await HandleHttpResponseMessage(response), true);
            }
        }


        /// <summary>
        /// Both server and client need to make a request to sessionserver.mojang.com if the server is in online-mode.
        /// </summary>
        /// <param name="accessToken"></param>
        /// <param name="selectedProfile"></param>
        /// <param name="serverHash"></param>
        public static async Task<YggdrasilStatus> JoinSession(string accessToken, string selectedProfile, string serverHash)
        {
            var json = JsonConvert.SerializeObject(new JsonClientAuth
            {
                AccessToken = accessToken,
                SelectedProfile = selectedProfile,
                ServerID = serverHash
            });

            using (var client = new HttpClient())
            using (var response = await client.PostAsync(new Uri("https://sessionserver.mojang.com/session/minecraft/join"), new StringContent(json, Encoding.UTF8, "application/json")))
            {
                if (response.StatusCode == HttpStatusCode.NoContent)
                    return new YggdrasilStatus(YggdrasilResponse.Success, true);
                else
                    return new YggdrasilStatus(await HandleHttpResponseMessage(response), true);
            }
        }



        private static async Task<YggdrasilResponse> HandleHttpResponseMessage(HttpResponseMessage message)
        {
            if (message == null)
                return YggdrasilResponse.Error;


            var error = JsonConvert.DeserializeObject<Error>(await message.Content.ReadAsStringAsync());

            if (error.ErrorDescription == ErrorType.ForbiddenOperationException)
            {
                if (error.Cause != null && error.Cause.Contains("UserMigratedException"))
                    return YggdrasilResponse.AccountMigrated;

                if (error.ErrorMessage != null && error.ErrorMessage.Contains("Invalid username or password"))
                    return YggdrasilResponse.WrongPassword;

                if (error.ErrorMessage != null && error.ErrorMessage.Contains("Invalid credentials"))
                    return YggdrasilResponse.TooMuchAttempts;

                if (error.ErrorMessage != null && error.ErrorMessage.Contains("Invalid token"))
                    return YggdrasilResponse.InvalidToken;
            }

            if (error.ErrorDescription == ErrorType.IllegalArgumentException)
            {
                if (error.ErrorMessage != null && error.ErrorMessage.Contains("Access Token can not be null or empty"))
                    return YggdrasilResponse.InvalidToken;

                if (error.ErrorMessage != null && error.ErrorMessage.Contains("Access token already has a profile assigned"))
                    return YggdrasilResponse.InvalidToken;
            }

            return YggdrasilResponse.Error;
        }
    }
}