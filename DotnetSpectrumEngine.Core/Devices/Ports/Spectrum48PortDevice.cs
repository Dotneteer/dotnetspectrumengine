using DotnetSpectrumEngine.Core.Abstraction.Machine;

namespace DotnetSpectrumEngine.Core.Devices.Ports
{
    /// <summary>
    /// This class represents the port device used by the Spectrum 48 virtual machine
    /// </summary>
    public class Spectrum48PortDevice: UlaGenericPortDeviceBase
    {
        public Spectrum48PortDevice()
        {
            IPortHandler handler = new Spectrum48PortHandler(this);
            Handlers.Add(handler);
        }
    }
}