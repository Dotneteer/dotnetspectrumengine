using DotnetSpectrumEngine.Core.Abstraction.Devices;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the sound device.
    /// </summary>
    public sealed class SoundDeviceInfo:
        DeviceInfoBase<ISoundDevice, IAudioConfiguration, INoProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="configuration">Configuration data.</param>
        /// <param name="device">Sound device</param>
        public SoundDeviceInfo(IAudioConfiguration configuration, ISoundDevice device) : 
            base(null, configuration, device)
        {
        }
    }
}