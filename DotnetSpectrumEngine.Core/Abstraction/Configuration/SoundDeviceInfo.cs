using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the sound device.
    /// </summary>
    public sealed class SoundDeviceInfo:
        DeviceInfoBase<ISoundDevice, IAudioConfiguration, ISoundProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="provider">Optional provider instance.</param>
        /// <param name="configuration">Configuration data.</param>
        public SoundDeviceInfo(IAudioConfiguration configuration, ISoundProvider provider) : 
            base(provider, configuration)
        {
        }
    }
}