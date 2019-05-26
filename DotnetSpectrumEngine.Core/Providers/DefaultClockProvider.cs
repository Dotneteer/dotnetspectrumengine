using System.Diagnostics;
using DotnetSpectrumEngine.Core.Abstraction.Providers;
// ReSharper disable IdentifierTypo

namespace DotnetSpectrumEngine.Core.Providers
{
    /// <summary>
    /// This class implements a clock provider that allows access to the 
    /// high resolution system clock through the StopWatch class.
    /// </summary>
    public class DefaultClockProvider : VmComponentProviderBase, IClockProvider
    {
        private long _frequency;
        private readonly Stopwatch _watch = new Stopwatch();

        /// <summary>
        /// Initializes the provider
        /// </summary>
        public DefaultClockProvider()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Reset();
            _watch.Start();
        }

        /// <summary>
        /// The component provider should be able to reset itself
        /// </summary>
        public override void Reset()
        {
            _frequency = Stopwatch.Frequency;
        }

        /// <summary>
        /// Retrieves the frequency of the clock. This value shows new
        /// number of clock ticks per second.
        /// </summary>
        public long GetFrequency() => _frequency;

        /// <summary>
        /// Retrieves the current counter value of the clock.
        /// </summary>
        public long GetCounter()
        {
            return _watch.ElapsedTicks;
        }
    }
}