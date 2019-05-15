using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the clock device.
    /// </summary>
    public sealed class ClockDeviceInfo: DeviceInfoBase<IClockDevice, INoConfiguration, IClockProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="provider">Optional provider instance.</param>
        public ClockDeviceInfo(IClockProvider provider) : base(provider)
        {
        }
    }
}