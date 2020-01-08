using PokeD.Core;

using System;
using System.Diagnostics;
using System.Globalization;
using System.Threading;

namespace PokeD.Server.WorldBus
{
    public class WorldService : IDisposable
    {
        private CancellationTokenSource UpdateToken { get; set; } = new CancellationTokenSource();
        private ManualResetEventSlim UpdateLock { get; } = new ManualResetEventSlim(false);

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

        private bool IsDisposed { get; set; }


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

        public void UpdateCycle()
        {
            UpdateLock.Reset();

            var watch = Stopwatch.StartNew();
            while (!UpdateToken.IsCancellationRequested)
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
                    Thread.Sleep(time);
                }
                TimeSpanOffset.Add(TimeSpan.FromMilliseconds(watch.ElapsedMilliseconds));
                watch.Reset();
                watch.Start();
            }

            UpdateLock.Set();
        }

        public bool Start()
        {
            UpdateToken = new CancellationTokenSource();
            new Thread(UpdateCycle)
            {
                Name = "ModuleManagerUpdateTread",
                IsBackground = true
            }.Start();

            return true;
        }
        public bool Stop()
        {
            if (UpdateToken?.IsCancellationRequested == false)
            {
                UpdateToken.Cancel();
                UpdateLock.Wait();
            }

            return true;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        protected void Dispose(bool disposing)
        {
            if (!IsDisposed)
            {
                if (disposing)
                {
                    if (UpdateToken?.IsCancellationRequested == false)
                    {
                        UpdateToken.Cancel();
                        UpdateLock.Wait();
                    }
                }

                IsDisposed = true;
            }
        }
        ~WorldService()
        {
            Dispose(false);
        }
    }
}