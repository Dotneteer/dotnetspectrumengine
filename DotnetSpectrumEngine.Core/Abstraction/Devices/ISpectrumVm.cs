﻿using System.Threading;
using DotnetSpectrumEngine.Core.Abstraction.Configuration;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Screen;
using DotnetSpectrumEngine.Core.Abstraction.Machine;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Abstraction.Devices
{
    /// <summary>
    /// This interface represents a Spectrum virtual machine
    /// </summary>
    public interface ISpectrumVm : IRenderFrameBoundDevice
    {
        /// <summary>
        /// The Z80 CPU of the machine.
        /// </summary>
        IZ80Cpu Cpu { get; }

        /// <summary>
        /// Gets the ULA revision (2/3).
        /// </summary>
        string UlaIssue { get; }

        /// <summary>
        /// Gets the frequency of the virtual machine's clock in Hz.
        /// </summary>
        int BaseClockFrequency { get; }

        /// <summary>
        /// #of tacts within the frame.
        /// </summary>
        int FrameTacts { get; }

        /// <summary>
        /// The current tact within the frame.
        /// </summary>
        int CurrentFrameTact { get; }

        /// <summary>
        /// The number of frame tact at which the interrupt signal is generated.
        /// </summary>
        int InterruptTact { get; }

        /// <summary>
        /// Gets or sets the value of the contention accumulated since the start of 
        /// the machine.
        /// </summary>
        long ContentionAccumulated { get; set; }

        /// <summary>
        /// Gets the value of the contention accumulated when the 
        /// execution cycle started.
        /// </summary>
        long LastExecutionContentionValue { get; }

        /// <summary>
        /// The current execution cycle options.
        /// </summary>
        ExecuteCycleOptions ExecuteCycleOptions { get; }

        /// <summary>
        /// The memory device used by the virtual machine.
        /// </summary>
        IMemoryDevice MemoryDevice { get; }

        /// <summary>
        /// The port device used by the virtual machine.
        /// </summary>
        IPortDevice PortDevice { get; }

        /// <summary>
        /// The ULA device that renders the VM screen.
        /// </summary>
        IScreenDevice ScreenDevice { get; }

        /// <summary>
        /// The ULA device that takes care of raising interrupts.
        /// </summary>
        IInterruptDevice InterruptDevice { get; }

        /// <summary>
        /// The beeper device attached to the VM.
        /// </summary>
        IBeeperDevice BeeperDevice { get; }

        /// <summary>
        /// The current status of the keyboard.
        /// </summary>
        IKeyboardDevice KeyboardDevice { get; }

        /// <summary>
        /// The tape device attached to the VM.
        /// </summary>
        ITapeDevice TapeDevice { get; }

        /// <summary>
        /// Debug info provider object.
        /// </summary>
        ISpectrumDebugInfoProvider DebugInfoProvider { get; set; }

        /// <summary>
        /// Allows to set a clock frequency multiplier value (1, 2, 4, or 8).
        /// </summary>
        int ClockMultiplier { get; }

        /// <summary>
        /// Collection of Spectrum devices.
        /// </summary>
        DeviceInfoCollection DeviceData { get; }

        /// <summary>
        /// The ROM device used by the virtual machine.
        /// </summary>
        IRomDevice RomDevice { get; }

        /// <summary>
        /// The configuration of the ROM.
        /// </summary>
        IRomConfiguration RomConfiguration { get; }

        /// <summary>
        /// The ROM provider object.
        /// </summary>
        IRomProvider RomProvider { get; }

        /// <summary>
        /// The configuration of the memory.
        /// </summary>
        IMemoryConfiguration MemoryConfiguration { get; }

        /// <summary>
        /// The configuration of the screen.
        /// </summary>
        ScreenConfiguration ScreenConfiguration { get; }

        /// <summary>
        /// The provider that handles the keyboard.
        /// </summary>
        IKeyboardProvider KeyboardProvider { get; }

        /// <summary>
        /// Beeper configuration.
        /// </summary>
        IAudioConfiguration AudioConfiguration { get; }

        /// <summary>
        /// The sound device attached to the VM.
        /// </summary>
        ISoundDevice SoundDevice { get; }

        /// <summary>
        /// Sound configuration.
        /// </summary>
        IAudioConfiguration SoundConfiguration { get; }

        /// <summary>
        /// The tape device attached to the VM
        /// </summary>
        ITapeProvider TapeProvider { get; }

        /// <summary>
        /// Gets the reason why the execution cycle of the SpectrumEngine completed.
        /// </summary>
        ExecutionCompletionReason ExecutionCompletionReason { get; }

        /// <summary>
        /// The optional Floppy device.
        /// </summary>
        IFloppyDevice FloppyDevice { get; }

        /// <summary>
        /// The configuration of the floppy.
        /// </summary>
        IFloppyConfiguration FloppyConfiguration { get; }

        /// <summary>
        /// The main execution cycle of the Spectrum VM
        /// </summary>
        /// <param name="token">Cancellation token</param>
        /// <param name="options">Execution options</param>
        /// <param name="completeOnCpuFrame">The cycle should complete on CPU frame completion</param>
        /// <return>True, if the cycle completed; false, if it has been cancelled</return>
        bool ExecuteCycle(CancellationToken token, ExecuteCycleOptions options, bool completeOnCpuFrame = false);

        /// <summary>
        /// Gets the device with the provided type.
        /// </summary>
        /// <typeparam name="TDevice">Device type.</typeparam>
        /// <returns>Device object.</returns>
        IDeviceInfo<TDevice, IDeviceConfiguration, IVmComponentProvider> GetDeviceInfo<TDevice>()
            where TDevice : class, IDevice;

        /// <summary>
        /// Gets the virtual machine's state serialized to JSON.
        /// </summary>
        /// <param name="modelName">Current virtual machine model name.</param>
        /// <returns>JSON representation of the virtual machine's state.</returns>
        string GetVmState(string modelName);

        /// <summary>
        /// Sets the virtual machine's state from the JSON string.
        /// </summary>
        /// <param name="json">JSON representation of the virtual machine's state.</param>
        /// <param name="modelName">Current virtual machine model name.</param>
        void SetVmState(string json, string modelName);
    }
}