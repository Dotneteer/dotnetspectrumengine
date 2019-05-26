using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the I/O ports.
    /// </summary>
    public sealed class PortDeviceInfo:
        DeviceInfoBase<IPortDevice, IPortConfiguration, INoProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="configurationData">Optional configuration information.</param>
        /// <param name="device">Port device.</param>
        public PortDeviceInfo(IPortConfiguration configurationData, IPortDevice device) : 
            base(null, configurationData, device)
        {
        }
    }
}