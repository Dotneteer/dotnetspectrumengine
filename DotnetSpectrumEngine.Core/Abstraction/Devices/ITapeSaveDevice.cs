using System;

namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// This interface represents the device that manages the tape.
    /// </summary>
    public interface ITapeSaveDevice : ISpectrumBoundDevice
    {
        /// <summary>
        /// Processes the the change of the MIC bit.
        /// </summary>
        /// <param name="micBit">New MIC bit value.</param>
        void ProcessMicBit(bool micBit);

        /// <summary>
        /// Signs that the device entered SAVE mode.
        /// </summary>
        event EventHandler EnteredSaveMode;

        /// <summary>
        /// Signs that the device has just left SAVE mode.
        /// </summary>
        event EventHandler LeftSaveMode;
    }
}