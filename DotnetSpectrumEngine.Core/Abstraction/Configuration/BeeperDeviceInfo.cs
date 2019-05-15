using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the beeper device.
    /// </summary>
    public class BeeperDeviceInfo: DeviceInfoBase<IBeeperDevice, IAudioConfiguration, IBeeperProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="configurationData">Optional configuration information.</param>
        /// <param name="provider">Optional provider instance.</param>
        public BeeperDeviceInfo(IAudioConfiguration configurationData, IBeeperProvider provider) : 
            base(provider, configurationData)
        {
        }
    }
}