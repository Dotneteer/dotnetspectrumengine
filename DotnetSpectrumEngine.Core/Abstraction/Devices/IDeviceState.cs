﻿namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// Objects that describe the state of a device should implement this
    /// interface.
    /// </summary>
    public interface IDeviceState
    {
        /// <summary>
        /// Restores the device state from this state object.
        /// </summary>
        /// <param name="device">Device instance.</param>
        void RestoreDeviceState(IDevice device);
    }
}