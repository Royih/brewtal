using System;
using System.Diagnostics;
using Brewtal2.Pid;
using Microsoft.Extensions.DependencyInjection;

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


        public static string Bash(this string cmd)
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

            return result;
        }
    }
}