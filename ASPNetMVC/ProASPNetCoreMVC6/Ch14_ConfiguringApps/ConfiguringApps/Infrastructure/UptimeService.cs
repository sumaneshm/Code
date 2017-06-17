using System.Diagnostics;

namespace ConfiguringApps.Infrastructure
{
    public class UptimeService
    {
        private Stopwatch timer;

        public UptimeService()=>timer = Stopwatch.StartNew();

        public double Uptime => timer.Elapsed.TotalSeconds;
    }
}
