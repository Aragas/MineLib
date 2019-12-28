using Aragas.QServer.Core;

using System;

namespace PokeD.Server.Core
{
    // Warning, you can't use the same instance for sending and getting the same message
    public class InternalBus : BaseInternalBus
    {
        private static IMBus? _worldBus;
        public static IMBus WorldBus => _worldBus ?? (_worldBus = new NatsMBus($"{Host}/poked/server/worldbus", TimeSpan.FromMilliseconds(Timeout)));

        private static IMBus? _entityBus;
        public static IMBus EntityBus => _entityBus ?? (_entityBus = new NatsMBus($"{Host}/poked/server/entitybus"));

        private static IMBus? _playerBus;
        public static IMBus PlayerBus => _playerBus ?? (_playerBus = new NatsMBus($"{Host}/poked/server/playerbus"));
    }
}