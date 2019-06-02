using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the tape save device.
    /// </summary>
    public sealed class TapeSaveDeviceInfo:
        DeviceInfoBase<ITapeSaveDevice, INoConfiguration, ITapeSaveProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="saveProvider">Optional provider instance.</param>
        public TapeSaveDeviceInfo(ITapeSaveProvider saveProvider = default) : base(saveProvider)
        {
        }
    }
}