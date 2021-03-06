﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Threading.Tasks;
using DotnetSpectrumEngine.Core.Abstraction.Configuration;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Screen;
using DotnetSpectrumEngine.Core.Abstraction.Machine;
using DotnetSpectrumEngine.Core.Machine;

namespace DotnetSpectrumEngine.Core.Abstraction
{
    /// <summary>
    /// This class represents the API of a ZX Spectrum virtual machine.
    /// </summary>
    public interface ISpectrumMachine
    {
        /// <summary>
        /// The current machine's model key.
        /// </summary>
        string ModelKey { get; }

        /// <summary>
        /// The current machine's edition key.
        /// </summary>
        string EditionKey { get; }

        /// <summary>
        /// Gets or sets the state of the virtual machine
        /// </summary>
        VmState MachineState { get; }

        /// <summary>
        /// Signs that this is the very first start of the
        /// virtual machine 
        /// </summary>
        bool IsFirstStart { get; }

        /// <summary>
        /// Signs that this is the very first paused state
        /// of the virtual machine
        /// </summary>
        bool IsFirstPause { get; }

        /// <summary>
        /// Signs if the machine runs in debug mode.
        /// </summary>
        bool RunsInDebugMode { get; }

        /// <summary>
        /// Indicates if fast tape mode is allowed.
        /// </summary>
        bool FastTapeMode { get; set; }

        /// <summary>
        /// The CPU of the machine
        /// </summary>
        CpuZ80 Cpu { get; }

        /// <summary>
        /// Provides access to the individual ROM pages of the machine
        /// </summary>
        IReadOnlyList<ReadOnlyMemorySlice> Roms { get; }

        /// <summary>
        /// Gets the number of ROM pages
        /// </summary>
        int RomCount { get; }

        /// <summary>
        /// Allows to obtain paging information about the memory
        /// </summary>
        MemoryPagingInfo PagingInfo { get; }

        /// <summary>
        /// The current Contents of the machine's 64K addressable memory
        /// </summary>
        SpectrumMemoryContents Memory { get; }

        /// <summary>
        /// Provides access to the individual RAM banks of the machine
        /// </summary>
        IReadOnlyList<MemorySlice> RamBanks { get; }

        /// <summary>
        /// Gets the number of RAM banks
        /// </summary>
        int RamBankCount { get; }

        /// <summary>
        /// Allows to emulate keyboard keys and query the keyboard state
        /// </summary>
        KeyboardEmulator Keyboard { get; }

        /// <summary>
        /// Allows read-only access to screen rendering configuration
        /// </summary>
        ScreenConfiguration ScreenConfiguration { get; }

        /// <summary>
        /// Allows read-only access to the screen rendering table
        /// </summary>
        ScreenRenderingTable ScreenRenderingTable { get; }

        /// <summary>
        /// A bitmap that represents the current visible screen's pixels, including the border
        /// </summary>
        ScreenBitmap ScreenBitmap { get; }

        /// <summary>
        /// Gets the current screen rendering status of the machine.
        /// </summary>
        ScreenRenderingStatus ScreenRenderingStatus { get; }

        /// <summary>
        /// Gets the beeper configuration of the machine
        /// </summary>
        IAudioConfiguration BeeperConfiguration { get; }

        /// <summary>
        /// Gets the beeper samples of the current rendering frame
        /// </summary>
        AudioSamples BeeperSamples { get; }

        /// <summary>
        /// Gets the sound (PSG) configuration of the machine
        /// </summary>
        IAudioConfiguration SoundConfiguration { get; }

        /// <summary>
        /// Gets the sound (PSG) samples of the current rendering frame
        /// </summary>
        AudioSamples AudioSamples { get; }

        /// <summary>
        /// The collection of breakpoints
        /// </summary>
        CodeBreakpoints Breakpoints { get; }

        /// <summary>
        /// Gets the reason that tells why the machine has been stopped or paused
        /// </summary>
        ExecutionCompletionReason ExecutionCompletionReason { get; }

        /// <summary>
        /// The number of CPU frame counts since the machine has started.
        /// </summary>
        int CpuFrameCount { get; }

        /// <summary>
        /// The number of render frame counts since the machine has started.
        /// </summary>
        int RenderFrameCount { get; }

        /// <summary>
        /// The length of last CPU frame in ticks
        /// </summary>
        long LastCpuFrameTicks { get; }

        /// <summary>
        /// The length of last render frame in ticks
        /// </summary>
        long LastRenderFrameTicks { get; }

        /// <summary>
        /// This event is raised when the state of the virtual machine has been changed.
        /// </summary>
        event EventHandler<VmStateChangedEventArgs> VmStateChanged;

        /// <summary>
        /// This event is executed when it is time to scan the ZX Spectrum keyboard.
        /// </summary>
        event EventHandler<KeyStatusEventArgs> KeyScanning;

        /// <summary>
        /// This event is executed whenever the CPU frame has been completed.
        /// </summary>
        event EventHandler<CancelEventArgs> CpuFrameCompleted;

        /// <summary>
        /// This event is executed whenever the render frame has been completed.
        /// </summary>
        event EventHandler<RenderFrameEventArgs> RenderFrameCompleted;

        /// <summary>
        /// This event is raised when a fast load operation has been completed.
        /// </summary>
        event EventHandler FastLoadCompleted;

        /// <summary>
        /// This event fires when the virtual machine engine raised an exception.
        /// </summary>
        event EventHandler<VmExceptionArgs> ExceptionRaised;

        /// <summary>
        /// Starts the machine in a background thread with the specified options.
        /// </summary>
        /// <remarks>
        /// Reports completion when the machine starts executing its cycles. The machine can
        /// go into Paused or Stopped state, if the execution options allow, for example, 
        /// when it runs to a predefined breakpoint.
        /// </remarks>
        void StartWithOptions(ExecuteCycleOptions options);

        /// <summary>
        /// Starts the machine in a background thread in continuous running mode.
        /// </summary>
        /// <remarks>
        /// Reports completion when the machine starts executing its cycles. The machine can
        /// go into Paused or Stopped state, if the execution options allow, for example, 
        /// when it runs to a predefined breakpoint.
        /// </remarks>
        void Start();

        /// <summary>
        /// Pauses the running machine.
        /// </summary>
        /// <remarks>
        /// Reports completion when the background execution has been stopped.
        /// </remarks>
        Task Pause();

        /// <summary>
        /// Stops the Spectrum machine.
        /// </summary>
        /// <remarks>
        /// If the machine is paused or stopped, it leaves the machine in its state.
        /// The task completes when the machine has completed its execution cycle.
        /// </remarks>
        Task Stop();

        /// <summary>
        /// Starts the Spectrum machine and runs it on a background thread unless it reaches a breakpoint.
        /// </summary>
        void StartDebug();

        /// <summary>
        /// Executes the subsequent Z80 instruction.
        /// </summary>
        /// <remarks>
        /// The task completes when the machine has completed its execution cycle.
        /// </remarks>
        void StepInto();

        /// <summary>
        /// Executes the subsequent Z80 CALL, RST, or block instruction entirely.
        /// </summary>
        /// <remarks>
        /// The task completes when the machine has completed its execution cycle.
        /// </remarks>
        void StepOver();

        /// <summary>
        /// Executes the subsequent Z80 CALL, RST, or block instruction entirely.
        /// </summary>
        /// <remarks>
        /// The task completes when the machine has completed its execution cycle.
        /// </remarks>
        void StepOut();
    }
}