using System;
using System.Diagnostics;
using Brewtal2.Pid;
using Microsoft.Extensions.DependencyInjection;
using Serilog;

namespace Brewtal2.Infrastructure
{
    public static class Extensions
    {

        public static void StartBackgroundWorker(this IServiceProvider serviceProvider)
        {
            var worker = serviceProvider.GetRequiredService<BackgroundWorker>();
            var pidRepo = serviceProvider.GetRequiredService<IPidRepository>();
            worker.Start(pidRepo);
        }

        public static BashResult Bash(this string cmd)
        {

            var escapedArgs = cmd.Replace("\"", "\\\"");

            var process = new Process()
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = "/bin/bash",
                    Arguments = $"-c \"{escapedArgs}\"",
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };
            process.Start();
            string result = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            if (process.ExitCode == 0)
            {
                return new BashResult { Success = true, Result = result };
            }
            Log.Warning($"Error executing bash. Exit Code = {process.ExitCode}");
            return new BashResult { Success = false, Result = result };

        }
    }
}