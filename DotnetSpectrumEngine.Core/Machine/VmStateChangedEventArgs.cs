using System;

namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This class represents the arguments of the VmStateChanged event.
    /// </summary>
    public class VmStateChangedEventArgs: EventArgs
    {
        public VmStateChangedEventArgs(VmState oldState, VmState newState)
        {
            OldState = oldState;
            NewState = newState;
        }

        /// <summary>
        /// Old machine state
        /// </summary>
        public VmState OldState { get; }

        /// <summary>
        /// New machine state
        /// </summary>
        public VmState NewState { get; }

    }
}