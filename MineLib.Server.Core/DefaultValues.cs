namespace MineLib.Server.Core
{
    public static class DefaultValues
    {
        // -- One Instance
        public static ushort MBus_Port = 0x5000;
        public static ushort Proxy_Port = 0x5001;
        //public static ushort EntityBus_Port = 0x5002;
        //public static ushort WorldBus_Port = 0x5003;

        // -- Multiple Instances
        //public static ushort PlayerHandler_Port = 0x6100;
        //public static ushort EntityHandler_Port = 0x6200;
        //public static ushort WorldHandler_Port  = 0x6300;


        public static ushort Proxy_Netty_Port = 25565;
        public static ushort Proxy_Legacy_Port = 25560;
    }
}