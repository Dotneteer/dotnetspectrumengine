﻿using System;

namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// This interface represents the device that manages the tape.
    /// </summary>
    public interface ITapeDevice : ISpectrumBoundDevice
    {
        /// <summary>
        /// Allow the device to react to the start of a new frame.
        /// </summary>
        void OnCpuOperationCompleted();

        /// <summary>
        /// This flag indicates if the tape is in load mode (EAR bit is set by the tape).
        /// </summary>
        bool IsInLoadMode { get; }

        /// <summary>
        /// Gets the EAR bit read from the tape.
        /// </summary>
        /// <param name="cpuTicks">CPU tacts count when the EAR bit is read.</param>
        /// <returns></returns>
        bool GetEarBit(long cpuTicks);

        /// <summary>
        /// Sets the current tape mode according to the current PC register
        /// and the MIC bit state.
        /// </summary>
        void SetTapeMode();

        /// <summary>
        /// Processes the the change of the MIC bit.
        /// </summary>
        /// <param name="micBit">New MIC bit value.</param>
        void ProcessMicBit(bool micBit);

        /// <summary>
        /// External entities can respond to the event when a fast load completed.
        /// </summary>
        event EventHandler LoadCompleted;

        /// <summary>
        /// Signs that the device entered LOAD mode.
        /// </summary>
        event EventHandler EnteredLoadMode;

        /// <summary>
        /// Signs that the device has just left LOAD mode.
        /// </summary>
        event EventHandler LeftLoadMode;

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