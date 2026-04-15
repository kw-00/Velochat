using System.Collections.Concurrent;
using System.Diagnostics.Metrics;

namespace Velochat.Backend.App.Shared.Metrics
{
    public static class VelochatMetrics
    {
        public static readonly string MetricsName = "Velochat";
        public static readonly string DuplicateInvitation = "duplicate_invitation";

        private static readonly ConcurrentDictionary<string, Counter<long>> _counters = new();

        private static readonly Meter _velochatMeter = new(MetricsName);

        static VelochatMetrics()
        {
            Register(DuplicateInvitation);
        }

        public static void Increment(string key) => Add(key, 1);

        public static void Add(string key, int value)
        {
            if (_counters.TryGetValue(key, out var counter))
            {
                counter.Add(value);
            }
            else
            {
                throw new KeyNotFoundException($"Key of \"{key}\" not found among metric keys.");
            }
        }

        private static void Register(string key)
        {
            if (_counters.ContainsKey(key))
            {
                throw new ArgumentException($"Counter with key of \"{key}\" already registered.");
            }
            _counters[key] = _velochatMeter.CreateCounter<long>(key);
        }
    }
}