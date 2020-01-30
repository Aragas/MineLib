using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

using MineLib.Server.Proxy.Data;

using System;
using System.IO;
using System.Net;
using System.Text;

namespace MineLib.Server.Proxy.BackgroundServices
{
    public enum BeatType { Minecraft }

    public class Heartbeat
    {
        private readonly int _protocolVersion = 7;
        private readonly bool _isPublic = true;
        private readonly ushort _port;
        private readonly MineLibOptions _mineLibOptions;
        private readonly ServerInfo _serverInfo;
        private readonly ClassicServerInfo _classicServerInfo;
        private readonly ILogger _logger;
        private readonly BeatType beatType;

        public Heartbeat(BeatType bType, ushort port, IOptions<MineLibOptions> mineLibOptions, ServerInfo serverInfo, ClassicServerInfo classicServerInfo, ILogger<Heartbeat> logger)
        {
            _port = port;
            _mineLibOptions = mineLibOptions.Value;
            _serverInfo = serverInfo;
            _classicServerInfo = classicServerInfo;
            _logger = logger;
            beatType = bType;
        }

        public bool Beat(bool initial)
        {
            if (beatType == BeatType.Minecraft)
            {
                try
                {
                    var args = $"port={_port}&max={_mineLibOptions.MaxConnections}&name={Uri.EscapeUriString(_mineLibOptions.Name)}&public={_isPublic}&version={_protocolVersion}&salt={_classicServerInfo.Salt}&users={_serverInfo.CurrentConnections}";
                    var fullUrl = $"{_mineLibOptions.ClassicHeartbeatUrl}?" + args;
                    _logger.LogInformation("{TypeName}: Sending Heartbeat with url ({Url})", GetType().FullName, fullUrl);
                    var beatRequest = (HttpWebRequest)WebRequest.Create(new Uri(fullUrl));
                    beatRequest.Method = "GET";
                    beatRequest.ContentType = "application/x-www-form-urlencoded";
                    beatRequest.ContentLength = args.Length;
                    using var responseStream = beatRequest.GetResponse().GetResponseStream();
                    var responseBytes = new byte[73]; //URL should be 73 characters long
                    responseStream.Read(responseBytes, 0, 73);
                    if (initial)
                    {
                        _logger.LogInformation("{TypeName}: Received URL: {Url}", GetType().FullName, Encoding.ASCII.GetString(responseBytes));
                        using var fileWriter = new StreamWriter(File.OpenWrite("externalurl.txt"));
                        fileWriter.Write(Encoding.ASCII.GetString(responseBytes));
                    }
                }
                catch (WebException ex)
                {
                    _logger.LogWarning(ex, "{TypeName}: Unable to send Heartbeat", GetType().FullName);
                    if (initial)
                    {
                        //Program.server.verify_names = false;
                        _logger.LogInformation("{TypeName}: Initial heartbeat failed. Turning verify-names off", GetType().FullName);
                    }
                    return false;
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "{TypeName}: Error occurred during heartbeat", GetType().FullName);
                    if (initial)
                    {
                        //Program.server.verify_names = false;
                        _logger.LogInformation("{TypeName}: Initial heartbeat failed. Turning verify-names off", GetType().FullName);
                    }
                    return false;
                }
                return true;
            }
            return false;
        }
    }
}