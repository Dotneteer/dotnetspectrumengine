using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the ROM device.
    /// </summary>
    public sealed class RomDeviceInfo: DeviceInfoBase<IRomDevice, IRomConfiguration, IRomProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="provider">Optional provider instance.</param>
        /// <param name="configurationData">Optional configuration information.</param>
        /// <param name="device">Device instance.</param>
        public RomDeviceInfo(IRomProvider provider, IRomConfiguration configurationData, IRomDevice device) : 
            base(provider, configurationData, device)
        {
        }
    }
}