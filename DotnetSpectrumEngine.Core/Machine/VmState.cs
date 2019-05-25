namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This enumeration represents the state of a ZX Spectrum virtual machine
    /// </summary>
    public enum VmState
    {
        /// <summary>The machine is turned off</summary>
        Off,

        /// <summary>The machine is being turned on</summary>
        TurningOn,

        /// <summary>The machine is turned on</summary>
        On,

        /// <summary>The machine is starting (after On, Paused, or Stopped states)</summary>
        Starting,

        /// <summary>The machine has successfully started (after Starting)</summary>
        Running,

        /// <summary>The machine is getting paused (after Running)</summary>
        Pausing,

        /// <summary>The machine has successfully paused (after Running)</summary>
        Paused,

        /// <summary>The machine is getting stopped (after Running)</summary>
        Stopping,

        /// <summary>The machine has successfully stopped (after Stopping)</summary>
        Stopped,

        /// <summary>The machine is being turned off (from any state)</summary>
        TurningOff
    }
}