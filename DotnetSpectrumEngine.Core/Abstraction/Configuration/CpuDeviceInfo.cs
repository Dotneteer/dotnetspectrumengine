using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class describes configuration information for the CPU device.
    /// </summary>
    public sealed class CpuDeviceInfo : DeviceInfoBase<IZ80Cpu, ICpuConfiguration, IVmComponentProvider>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="T:System.Object" /> class.
        /// </summary>
        /// <param name="configurationData">Optional configuration information.</param>
        public CpuDeviceInfo(ICpuConfiguration configurationData) : base(null, configurationData)
        {
        }
    }
}