﻿namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// This device is used to generate maskable interrupts in every frame
    /// </summary>
    public interface IInterruptDevice: ISpectrumBoundDevice, IRenderFrameBoundDevice
    {
        /// <summary>
        /// The ULA tact to raise the interrupt at.
        /// </summary>
        int InterruptTact { get; }

        /// <summary>
        /// Signs that an interrupt has been raised in this frame.
        /// </summary>
        bool InterruptRaised { get; }

        /// <summary>
        /// Signs that the interrupt signal has been revoked.
        /// </summary>
        bool InterruptRevoked { get; }

        /// <summary>
        /// Generates an interrupt in the current phase, if time has come.
        /// </summary>
        /// <param name="currentTact">Current frame tact</param>
        void CheckForInterrupt(int currentTact);
    }
}