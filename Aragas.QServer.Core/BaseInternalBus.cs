using Aragas.Network.Data;
using Aragas.QServer.Core.Extensions;
using Aragas.QServer.Core.Packets;
using Aragas.QServer.Core.Packets.PlayerHandler;
using Aragas.QServer.Core.Protocol;

using System;
using System.Collections.Generic;
using System.Net.Sockets;
using System.Threading;

namespace Aragas.QServer.Core
{
    // Warning, you can't use the same instance for sending and getting the same message
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "RCS1102:Make class static.", Justification = "<Pending>")]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Design", "CA1052:Static holder types should be Static or NotInheritable", Justification = "<Pending>")]
    [Obsolete]
    public class BaseInternalBus
    {
        //public static string Host { get; } = "mbus://0.0.0.0";
        public static string Host { get; } = "bus";

        public const int Timeout = 10000;


        private static IMBus? _proxyBus;
        public static IMBus ProxyBus => _proxyBus ?? (_proxyBus = new NatsMBus($"{Host}/qserver/server/proxybus"));
        public static Socket GetFirstAvailablePlayerHandlerConnection(VarInt protocolVersion, IMBus? proxyBus = null)
        {
            return HandleResponse<Socket, AvailableSocketRequestPacket, AvailableSocketResponsePacket>(proxyBus ?? ProxyBus,
                new AvailableSocketRequestPacket() { ProtocolVersion = protocolVersion },
                response =>
                {
                    var sock = new Socket(SocketType.Stream, ProtocolType.Tcp);
                    sock.Connect(response.Endpoint);
                    return sock;
                });
        }

        public static TReturn HandleResponse<TReturn, TPacketRequest, TPacketResponse>(IMBus bus, TPacketRequest request, Func<TPacketResponse, TReturn> responseHandler, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            var @return = default(TReturn)!;
            var guid = Guid.NewGuid();

            using var eventLock = new ManualResetEventSlim(false);
            bus.MessageReceived += (sender, args) =>
            {
                using var internalFactory = new InternalFactory();
                switch (internalFactory.GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case TPacketResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        @return = responseHandler(response);
                        eventLock.Set();
                        break;
                }
            };

            request.GUID = guid;
            bus.SendPacket(request);

            eventLock.Wait(timeout);

            return @return;
        }
        public static IEnumerable<TReturn> HandleResponse<TReturn, TPacketRequest, TPacketEnumerableResponse, TPacketBulkResponse>(
            IMBus bus,
            TPacketRequest request,
            Func<TPacketEnumerableResponse, TReturn> responseEnumerableHandler,
            Func<TPacketBulkResponse, TReturn[]> responseBulkHandler,
            bool inBulk = false, int timeout = Timeout)
            where TPacketRequest : InternalPacket
            where TPacketEnumerableResponse : InternalPacket
            where TPacketBulkResponse : InternalPacket
        {
            var bulk = Array.Empty<TReturn>();
            var currentReturn = default(TReturn)!;
            using var lock1 = new ManualResetEventSlim(false);
            using var lock2 = new ManualResetEventSlim(true);
            var lastPacket = DateTime.UtcNow;

            var guid = Guid.NewGuid();

            using var eventLock = new ManualResetEventSlim(false);
            bus.MessageReceived += (sender, args) =>
            {
                using var internalFactory = new InternalFactory();
                switch (internalFactory.GetPacket(args.Message))
                {
                    case PingPacket pingPacket:
                        break;

                    case TPacketEnumerableResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        lock2.Wait(); // Ждем завершения отправки данных
                        currentReturn = responseEnumerableHandler(response); // Ставим новые данные
                        lastPacket = DateTime.UtcNow;
                        lock2.Reset();
                        lock1.Set(); // Завершили отправку данных
                        break;

                    case TPacketBulkResponse response:
                        if (!guid.Equals(response.GUID))
                            return;

                        bulk = responseBulkHandler(response);
                        eventLock.Set();
                        break;
                }
            };

            request.GUID = guid;
            bus.SendPacket(request);

            if (inBulk)
            {
                eventLock.Wait(timeout * 2);

                foreach (var item in bulk)
                    yield return item;
            }
            else
            {
                while (DateTime.UtcNow - lastPacket < TimeSpan.FromMilliseconds(timeout))
                {
                    lock1.Wait(timeout); // Ждем пока появятся данные
                    yield return currentReturn; // Поставили данные
                    lock1.Reset();
                    lock2.Set(); // Завершили отправку данных
                }
            }
        }
        public static void HandleRequest<TPacketRequest, TPacketResponse>(IMBus bus, MBusMessageReceivedEventArgs args, Func<TPacketRequest, TPacketResponse?> requestHandler/*, int timeout = Timeout*/)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            using var internalFactory = new InternalFactory();
            switch (internalFactory.GetPacket(args.Message))
            {
                case TPacketRequest request:
                    var response = requestHandler(request);
                    if (response == null) return;
                    response.GUID = request.GUID;
                    bus.SendPacket(response);
                    break;
            }
        }
        public static void HandleRequest<TPacketRequest, TPacketResponse>(IMBus bus, MBusMessageReceivedEventArgs args, Func<TPacketRequest, IEnumerable<TPacketResponse>?> requestHandler/*, int timeout = Timeout*/)
            where TPacketRequest : InternalPacket
            where TPacketResponse : InternalPacket
        {
            using var internalFactory = new InternalFactory();
            switch (internalFactory.GetPacket(args.Message))
            {
                case TPacketRequest request:
                    var responseEnumerable = requestHandler(request);
                    if (responseEnumerable == null) return;
                    foreach (var response in responseEnumerable)
                    {
                        response.GUID = request.GUID;
                        bus.SendPacket(response);
                    }
                    break;
            }
        }
    }
}