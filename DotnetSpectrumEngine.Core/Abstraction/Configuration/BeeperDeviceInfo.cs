using DotnetSpectrumEngine.Core.Abstraction.Devices;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the beeper device.
    /// </summary>
    public class BeeperDeviceInfo: DeviceInfoBase<IBeeperDevice, IAudioConfiguration, INoProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="configurationData">Optional configuration information.</param>
        /// <param name="device">Beeper device</param>
        public BeeperDeviceInfo(IAudioConfiguration configurationData, IBeeperDevice device) : 
            base(null, configurationData, device)
        {
        }
    }
}