using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the tape device.
    /// </summary>
    public sealed class TapeDeviceInfo:
        DeviceInfoBase<ITapeDevice, INoConfiguration, ITapeProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="provider">Optional provider instance.</param>
        public TapeDeviceInfo(ITapeProvider provider = default) : base(provider)
        {
        }
    }
}