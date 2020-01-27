using AngleSharp;
using AngleSharp.Dom;

using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace MineLib.Protocol.Generator
{
    public class Program
    {
        static async Task Main(string[] args)
        {
            var webClient = new WebClient();
            var page = webClient.DownloadString("https://wiki.vg/Classic_Protocol");
            //var page = webClient.DownloadString("https://wiki.vg/Protocol");
            //var page = webClient.DownloadString("https://wiki.vg/index.php?title=Protocol&oldid=6003");
            var data = HtmlTableNormalizer.FormatAllTables(page);

            var config = Configuration.Default.WithDefaultLoader();
            var context = BrowsingContext.New(config);

            var document = await context.OpenAsync(req => req.Content(data));

            var packets = new List<Packet>();
            var ignoredPackets = new List<string>();
            foreach (var table in document.QuerySelectorAll(".wikitable"))
            {
                // Parsing normalized table to data
                var trElements = table.QuerySelectorAll("tr");
                if (!trElements.Any())
                    continue;
                else
                {
                    var thElements = trElements[0].QuerySelectorAll("th");
                    if (!thElements.Any())
                        continue;
                }

                if (!string.Equals(trElements[0].QuerySelectorAll("th")[0].TextContent.Sanitize(), "Packet ID", StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var tableIndex = table.ParentElement.ChildNodes.Index(table);
                var h4Element = default(INode);
                var k = 0;
                while (h4Element == null || h4Element.NodeName != "H4")
                    h4Element = table.ParentElement.ChildNodes[tableIndex - ++k];

                var row1PacketData = trElements.Skip(1).First().QuerySelectorAll("td").Take(3).Select(e => e.TextContent.Sanitize()).ToList();
                var id = row1PacketData[0];
                var state = row1PacketData[1];
                var boundTo = row1PacketData[2];
                var rows1 = trElements.Skip(1).First().QuerySelectorAll("td").Skip(3).Select(e => e.TextContent.Sanitize()).ToList();
                var rowsRest = trElements.Skip(2).Select(e => e.QuerySelectorAll("td").Skip(3).Select(e => e.TextContent.Sanitize()).ToList()).ToList();
                if(rows1.Count != 0)
                    rowsRest.Insert(0, rows1);


                // Parsing provided data
                var packet = new Packet(h4Element.TextContent.Sanitize(), id, state, boundTo);

                if(rowsRest[0].Count == 3)
                {
                    foreach (var row in rowsRest)
                    {
                        packet.Fields.Add(new Field(row[0], row[1]));
                    }
                }
                else if (rowsRest[0].Count == 5)
                {
                    for (int i = 0; i < rowsRest.Count; i++)
                    {
                        var row = rowsRest[i];
                        if (!string.IsNullOrEmpty(row[0]))
                        {
                            var parentField = new Field(row[0], row[2]);
                            if (!string.IsNullOrEmpty(row[1]))
                            {
                                parentField.SubFields.Add(new Field(row[1], row[3]));
                            }
                            packet.Fields.Add(parentField);
                        }
                        else // child field
                        {
                            var parentFieldName = "";
                            var j = 0;
                            while (string.IsNullOrEmpty(parentFieldName))
                                parentFieldName = rowsRest[i - ++j][0];

                            var parentField = packet.Fields.First(f => f.OriginalName == parentFieldName);
                            parentField.SubFields.Add(new Field(row[1], row[3]));
                        }
                    }
                }
                else
                {
                    ignoredPackets.Add(id);
                }
                packets.Add(packet);
            }


            var packetBuilder = new ProtobufPacketGenerator();
            var packetsWithMeta = packetBuilder.GenerateClasses(packets).ToList();
            ;

            // Saving to FS
            var directory = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated");

            var client = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Client");
            var clientHandshake = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Client", "Handshake");
            var clientStatus = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Client", "Status");
            var clientLogin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Client", "Login");
            var clientPlay = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Client", "Play");

            var server = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Server");
            var serverHandshake = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Server", "Handshake");
            var serverStatus = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Server", "Status");
            var serverLogin = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Server", "Login");
            var serverPlay = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Generated", "Server", "Play");
            Directory.CreateDirectory(directory);
            Directory.CreateDirectory(client);
            Directory.CreateDirectory(clientHandshake);
            Directory.CreateDirectory(clientStatus);
            Directory.CreateDirectory(clientLogin);
            Directory.CreateDirectory(clientPlay);
            Directory.CreateDirectory(server);
            Directory.CreateDirectory(serverHandshake);
            Directory.CreateDirectory(serverStatus);
            Directory.CreateDirectory(serverLogin);
            Directory.CreateDirectory(serverPlay);

            foreach (var (packet, content) in packetsWithMeta)
            {
                var dir = directory;
                switch (packet.BoundTo.ToLowerInvariant())
                {
                    case "server":
                        switch (packet.State.ToLowerInvariant())
                        {
                            case "handshake":
                            case "handshaking":
                                dir = serverHandshake;
                                break;
                            case "status":
                                dir = serverStatus;
                                break;
                            case "login":
                                dir = serverLogin;
                                break;
                            case "play":
                                dir = serverPlay;
                                break;
                        }
                        break;
                    case "client":
                        switch (packet.State.ToLowerInvariant())
                        {
                            case "handshake":
                            case "handshaking":
                                dir = clientHandshake;
                                break;
                            case "status":
                                dir = clientStatus;
                                break;
                            case "login":
                                dir = clientLogin;
                                break;
                            case "play":
                                dir = clientPlay;
                                break;
                        }
                        break;
                }
                File.WriteAllText(Path.Combine(dir, $"{packet.PacketID}_{packet.Name}.cs"), content);
            }

            if (Field.UnrecognizedTypes.Count > 0)
            {
                var list = Field.UnrecognizedTypes.Distinct().ToList();
                ;
            }
        }
    }
}