﻿using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Aragas.QServer.Metrics
{
    internal class MemoryMetricsClient
    {
        public class MemoryMetrics
        {
            public double Total;
            public double Used;
            public double Free;
        }

        private static bool IsUnix() => RuntimeInformation.IsOSPlatform(OSPlatform.OSX) || RuntimeInformation.IsOSPlatform(OSPlatform.Linux);
        public MemoryMetrics GetMetrics() => IsUnix() ? GetUnixMetrics() : GetWindowsMetrics();

        private MemoryMetrics GetWindowsMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo
            {
                FileName = "wmic",
                Arguments = "OS get FreePhysicalMemory,TotalVisibleMemorySize /Value",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
            }

            var lines = output.Trim().Split("\n");
            var freeMemoryParts = lines[0].Split("=", StringSplitOptions.RemoveEmptyEntries);
            var totalMemoryParts = lines[1].Split("=", StringSplitOptions.RemoveEmptyEntries);

            var metrics = new MemoryMetrics
            {
                Total = Math.Round(double.Parse(totalMemoryParts[1]) / 1024, 0),
                Free = Math.Round(double.Parse(freeMemoryParts[1]) / 1024, 0)
            };
            metrics.Used = metrics.Total - metrics.Free;

            return metrics;
        }

        private MemoryMetrics GetUnixMetrics()
        {
            var output = "";

            var info = new ProcessStartInfo("free -m")
            {
                FileName = "/bin/sh",
                Arguments = "-c \"free -m\"",
                RedirectStandardOutput = true
            };

            using (var process = Process.Start(info))
            {
                output = process.StandardOutput.ReadToEnd();
                Console.WriteLine(output);
            }

            var lines = output.Split("\n");
            var memory = lines.Length > 1 ? lines[1].Split(" ", StringSplitOptions.RemoveEmptyEntries) : null;

            return memory != null
                ? new MemoryMetrics { Total = double.Parse(memory[1]), Used = double.Parse(memory[2]), Free = double.Parse(memory[3]) }
                : new MemoryMetrics() { Total = 0, Used = 0, Free = 0 };
        }
    }
}