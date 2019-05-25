using System.Diagnostics;
using System.Threading;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Providers
{
    /// <summary>
    /// This class implements a clock provider that allows access to the 
    /// high resolution system clock.
    /// </summary>
    public class ClockProvider : VmComponentProviderBase, IClockProvider
    {
        private readonly Stopwatch _stopwatch = new Stopwatch();
        /// <summary>
        /// Initializes the provider
        /// </summary>
        public ClockProvider()
        {
            // ReSharper disable once VirtualMemberCallInConstructor
            Reset();
        }

        /// <summary>
        /// The component provider should be able to reset itself
        /// </summary>
        public override void Reset()
        {
            _stopwatch.Reset();
            _stopwatch.Restart();
        }

        /// <summary>
        /// Retrieves the frequency of the clock. This value shows new
        /// number of clock ticks per second.
        /// </summary>
        public long GetFrequency() => Stopwatch.Frequency;

        /// <summary>
        /// Retrieves the current counter value of the clock.
        /// </summary>
        public long GetCounter()
        {
            return _stopwatch.ElapsedTicks;
        }

        /// <summary>
        /// Waits until the specified counter value is reached
        /// </summary>
        /// <param name="counterValue">Counter value to reach</param>
        /// <param name="token">Token that can cancel the wait cycle</param>
        public void WaitUntil(long counterValue, CancellationToken token)
        {
            // --- Calculate the number of milliseconds to wait
            var millisecond = Stopwatch.Frequency / 1000;

            // --- Wait until we have up to 4 milliseconds left
            while (!token.IsCancellationRequested)
            {
                var milliseconds = (counterValue - GetCounter()) / millisecond;
                if (milliseconds < 0)
                {
                    return;
                }
                if (milliseconds < 4) break;
                Thread.Sleep(2);
            }

            // --- Use SpinWait
            while (!token.IsCancellationRequested)
            {
                if (counterValue < GetCounter()) break;
                Thread.SpinWait(1);
            }
        }
    }

}