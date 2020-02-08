using Aragas.QServer.NetworkBus;
using Microsoft.Extensions.Hosting;

using PokeD.Core;
using PokeD.Server.Core.NetworkBus.Messages;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace PokeD.Server.WorldBus.BackgroundServices
{
    public sealed class WorldService : BackgroundService,
        IMessageHandler<GetWorldStateRequestMessage, GetWorldStateResponseMessage>
    {
        public bool UseLocation { get; set; }
        private bool LocationChanged { get; set; }

        public string Location
        {
            get => _location;
            set { LocationChanged = _location != value; _location = value; }
        }
        private string _location = string.Empty;

        public bool UseRealTime { get; set; } = true;

        public bool DoDayCycle { get; set; } = true;

        public int WeatherUpdateTimeInMinutes { get; set; } = 60;

        public Season Season { get; set; } = Season.Spring;

        public Weather Weather { get; set; } = Weather.Sunny;

        public TimeSpan CurrentTime
        {
            get => TimeSpan.TryParseExact(CurrentTimeString, "hh\\,mm\\,ss", CultureInfo.InvariantCulture, TimeSpanStyles.None, out var timeSpan) ? timeSpan : TimeSpan.Zero;
            set => CurrentTimeString = $"{value.Hours:00},{value.Minutes:00},{value.Seconds:00}";
        }
        private string CurrentTimeString { get; set; }

        public TimeSpan TimeSpanOffset { get; set; }
        //private TimeSpan TimeSpanOffset => TimeSpan.FromSeconds(TimeOffset);
        //private int TimeOffset { get; set; }

        private DateTime WorldUpdateTime { get; set; } = DateTime.UtcNow;


        public WorldService() { }


        private int WeekOfYear => (int) (DateTime.Now.DayOfYear - ((DateTime.Now.DayOfWeek - DayOfWeek.Monday) / 7.0) + 1.0);
        private void UpdateWorld()
        {
            Season = (WeekOfYear % 4) switch
            {
                1 => Season.Winter,
                2 => Season.Spring,
                3 => Season.Summer,
                0 => Season.Fall,

                _ => Season.Summer,
            };
            var r = new Random().Next(0, 100);
            switch (Season)
            {
                case Season.Winter:
                    if (r < 20)
                        Weather = Weather.Rain;
                    else if (r >= 20 && r < 50)
                        Weather = Weather.Clear;
                    //else
                    //    Weather = Weather.Snow;
                    break;

                case Season.Spring:
                    if (r < 5)
                        Weather = Weather.Sunny;
                    else if (r >= 5 && r < 40)
                        Weather = Weather.Rain;
                    else
                        Weather = Weather.Clear;
                    break;

                case Season.Summer:
                    if (r < 40)
                        Weather = Weather.Clear;
                    else if (r >= 40 && r < 80)
                        Weather = Weather.Rain;
                    else
                        Weather = Weather.Sunny;
                    break;

                case Season.Fall:
                    //if (r < 5)
                    //    Weather = Weather.Snow;
                    //else 
                    if (r >= 5 && r < 80)
                        Weather = Weather.Rain;
                    else
                        Weather = Weather.Clear;
                    break;

                default:
                    Weather = Weather.Clear;
                    break;
            }
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            var watch = Stopwatch.StartNew();
            while (!stoppingToken.IsCancellationRequested)
            {
                if (WorldUpdateTime < DateTime.UtcNow)
                {
                    UpdateWorld();
                    WorldUpdateTime = DateTime.UtcNow.AddMinutes(WeatherUpdateTimeInMinutes);
                }

                if (watch.ElapsedMilliseconds < 1000)
                {
                    var time = (int)(10 - watch.ElapsedMilliseconds);
                    if (time < 0) time = 0;
                    await Task.Delay(time);
                }
                TimeSpanOffset.Add(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
                watch.Reset();
                watch.Start();
            }
        }

        Task<GetWorldStateResponseMessage> IMessageHandler<GetWorldStateRequestMessage, GetWorldStateResponseMessage>.HandleAsync(GetWorldStateRequestMessage message)
        {
            return Task.FromResult(new GetWorldStateResponseMessage()
            {
                Season = Season,
                Weather = Weather,
                DoDayCycle = DoDayCycle,
                Time = CurrentTime,
                TimeSpanOffset = TimeSpanOffset,
                UseRealTime = UseRealTime,
            });
        }
    }
}