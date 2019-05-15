using DotnetSpectrumEngine.Core.Abstraction.Devices;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the floppy device.
    /// </summary>
    public class FloppyDeviceInfo :
        DeviceInfoBase<IFloppyDevice, IFloppyConfiguration, INoProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="config">Configuration data</param>
        /// <param name="device">DivIde device instance</param>
        public FloppyDeviceInfo(IFloppyConfiguration config, IFloppyDevice device) 
            : base(null, config, device)
        {
        }
    }
}