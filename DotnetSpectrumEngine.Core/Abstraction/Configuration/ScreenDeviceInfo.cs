using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the screen device.
    /// </summary>
    public sealed class ScreenDeviceInfo : 
        DeviceInfoBase<IScreenDevice, IScreenConfiguration, IScreenFrameProvider>
    {
        public ScreenDeviceInfo(IScreenConfiguration configurationData = default, 
            IScreenFrameProvider provider = default) : 
            base(provider, configurationData)
        {
        }
    }
}