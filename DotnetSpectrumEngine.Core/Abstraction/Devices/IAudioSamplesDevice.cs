﻿namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// This interface describes a device that works with audio samples.
    /// </summary>
    public interface IAudioSamplesDevice
    {
        /// <summary>
        /// Audio samples to build the audio stream.
        /// </summary>
        float[] AudioSamples { get; }
    }
}