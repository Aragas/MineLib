using Aragas.QServer.Core.NetworkBus.Messages;
using PokeD.Core;
using PokeD.Core.Data.P3D;
using System;

namespace PokeD.Server.Core.NetworkBus.Messages
{

    public sealed class GetWorldStateRequestMessage : JsonMessage
    {
        public override string Name => "poked.server.worldbus.world.getstate.request";


    }
    public sealed class GetWorldStateResponseMessage : JsonMessage
    {
        public override string Name => "poked.server.worldbus.world.getstate.response";

        public Season Season { get; set; } 
        public Weather Weather { get; set; }
        public bool DoDayCycle { get; set; }
        public TimeSpan Time { get; set; }
        public TimeSpan TimeSpanOffset { get; set; }
        public bool UseRealTime { get; set; }

        public DataItems GenerateDataItems()
        {
            string currentTimeString = string.Empty;

            if (DoDayCycle)
            {
                if (TimeSpanOffset != TimeSpan.Zero)
                    if (UseRealTime)
                    {
                        var time = DateTime.Now.Add(TimeSpanOffset);
                        currentTimeString = $"{time.Hour:00},{time.Minute:00},{time.Second:00}";
                    }
                    else
                    {
                        Time += TimeSpanOffset;
                    }
                else if (UseRealTime)
                {
                    var time = DateTime.Now.Add(TimeSpanOffset);
                    currentTimeString = $"{time.Hour:00},{time.Minute:00},{time.Second:00}";
                }
            }
            else
                currentTimeString = "12,00,00";

            return new DataItems(((int) Season).ToString(), ((int) Weather).ToString(), currentTimeString);
        }
    }
}