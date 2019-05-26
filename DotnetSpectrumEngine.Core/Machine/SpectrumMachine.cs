// ReSharper disable IdentifierTypo

using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Threading;
using System.Threading.Tasks;
using DotnetSpectrumEngine.Core.Abstraction.Configuration;
using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Screen;
using DotnetSpectrumEngine.Core.Abstraction.Machine;
using DotnetSpectrumEngine.Core.Abstraction.Models;
using DotnetSpectrumEngine.Core.Abstraction.Providers;
using DotnetSpectrumEngine.Core.Devices.Beeper;
using DotnetSpectrumEngine.Core.Devices.Keyboard;
using DotnetSpectrumEngine.Core.Devices.Memory;
using DotnetSpectrumEngine.Core.Devices.Ports;
using DotnetSpectrumEngine.Core.Devices.Rom;
using DotnetSpectrumEngine.Core.Devices.Sound;
using DotnetSpectrumEngine.Core.Providers;

namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This class represents a ZX Spectrum virtual machine
    /// </summary>
    public class SpectrumMachine
    {
        private VmState _machineState;
        private readonly ISpectrumVm _spectrumVm;
        private readonly IClockProvider _clockProvider;
        public readonly double _physicalFrameClockCount;

        #region Static data members and their initialization

        // --- Provider factories to create providers when instantiating the machine
        private static readonly Dictionary<Type, Func<object>> s_ProviderFactories =
            new Dictionary<Type, Func<object>>();

        /// <summary>
        /// Resets the class members when first accessed
        /// </summary>
        static SpectrumMachine()
        {
            Reset();
            RegisterDefaultProviders();
        }

        /// <summary>
        /// Resets the static members of this class
        /// </summary>
        public static void Reset()
        {
            s_ProviderFactories.Clear();
        }

        /// <summary>
        /// Registers a provider
        /// </summary>
        /// <typeparam name="TProvider">Provider type</typeparam>
        /// <param name="factory">Factory method for the specified provider</param>
        public static void RegisterProvider<TProvider>(Func<TProvider> factory)
            where TProvider : class, IVmComponentProvider
        {
            s_ProviderFactories[typeof(TProvider)] = factory;
        }

        /// <summary>
        /// Registers the default providers
        /// </summary>
        public static void RegisterDefaultProviders()
        {
            RegisterProvider<IClockProvider>(() => new DefaultClockProvider());
            RegisterProvider<IRomProvider>(() => new DefaultRomProvider());
            RegisterProvider<IKeyboardProvider>(() => new DefaultKeyboardProvider());
            RegisterProvider<ITapeProvider>(() => new DefaultTapeProvider());
        }

        /// <summary>
        /// Gets a provider instance for the specified provider types
        /// </summary>
        /// <typeparam name="TProvider">Service provider type</typeparam>
        /// <param name="optional">In the provider optional?</param>
        /// <exception cref="KeyNotFoundException">
        /// The requested mandatory provider cannot be found
        /// </exception>
        /// <returns>Provider instance, if found; otherwise, null</returns>
        private static TProvider GetProvider<TProvider>(bool optional = true)
            where TProvider : class, IVmComponentProvider
        {
            if (s_ProviderFactories.TryGetValue(typeof(TProvider), out var factory))
            {
                return (TProvider)factory();
            }

            return optional
                ? (TProvider)default
                : throw new KeyNotFoundException($"Cannot find a factory for {typeof(TProvider)}");
        }

        #endregion

        /// <summary>
        /// Creates an instance of the virtual machine
        /// </summary>
        /// <param name="modelKey">The model key of the virtual machine</param>
        /// <param name="editionKey">The edition key of the virtual machine</param>
        /// <param name="devices">Devices to create the machine</param>
        private SpectrumMachine(string modelKey, string editionKey, DeviceInfoCollection devices)
        {
            ModelKey = modelKey;
            EditionKey = editionKey;
            RealTimeMode = false;
            DisableScreenRendering = false;

            _spectrumVm = new SpectrumEngine(devices);

            Cpu = new CpuZ80(_spectrumVm.Cpu);

            var roms = new List<ReadOnlyMemorySlice>();
            for (var i = 0; i < _spectrumVm.RomConfiguration.NumberOfRoms; i++)
            {
                roms.Add(new ReadOnlyMemorySlice(_spectrumVm.RomDevice.GetRomBytes(i)));
            }
            Roms = new ReadOnlyCollection<ReadOnlyMemorySlice>(roms);

            PagingInfo = new MemoryPagingInfo(_spectrumVm.MemoryDevice);
            Memory = new SpectrumMemoryContents(_spectrumVm.MemoryDevice, _spectrumVm.Cpu);

            var ramBanks = new List<MemorySlice>();
            if (_spectrumVm.MemoryConfiguration.RamBanks != null)
            {
                for (var i = 0; i < _spectrumVm.MemoryConfiguration.RamBanks; i++)
                {
                    ramBanks.Add(new MemorySlice(_spectrumVm.MemoryDevice.GetRamBank(i)));
                }
            }
            RamBanks = new ReadOnlyCollection<MemorySlice>(ramBanks);

            Keyboard = new KeyboardEmulator(_spectrumVm.KeyboardDevice);
            ScreenConfiguration = _spectrumVm.ScreenConfiguration;
            ScreenRenderingTable = new ScreenRenderingTable(_spectrumVm.ScreenDevice);
            ScreenBitmap = new ScreenBitmap(_spectrumVm.ScreenDevice);
            ScreenRenderingStatus = new ScreenRenderingStatus(_spectrumVm);
            BeeperConfiguration = _spectrumVm.AudioConfiguration;
            BeeperSamples = new AudioSamples(_spectrumVm.BeeperDevice);
            SoundConfiguration = _spectrumVm.SoundConfiguration;
            AudioSamples = new AudioSamples(_spectrumVm.SoundDevice);
            Breakpoints = new CodeBreakpoints(_spectrumVm.DebugInfoProvider);

            // --- Initialize machine state
            _clockProvider = GetProvider<IClockProvider>();
            _physicalFrameClockCount = _clockProvider.GetFrequency() / (double) _spectrumVm.BaseClockFrequency *
                                       ScreenConfiguration.ScreenRenderingFrameTactCount;
            MachineState = VmState.Off;
            ExecutionCompletionReason = ExecutionCompletionReason.None;
        }

        #region Machine factory methods

        /// <summary>
        /// Creates a Spectrum instance with the specified model and edition name
        /// </summary>
        /// <param name="modelKey">Spectrum model name</param>
        /// <param name="editionKey">Edition name</param>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateMachine(string modelKey, string editionKey)
        {
            // --- Check input
            if (modelKey == null) throw new ArgumentNullException(nameof(modelKey));
            if (editionKey == null) throw new ArgumentNullException(nameof(editionKey));

            if (!SpectrumModels.StockModels.TryGetValue(modelKey, out var model))
            {
                throw new KeyNotFoundException($"Cannot find a Spectrum model with key '{modelKey}'");
            }

            if (!model.Editions.TryGetValue(editionKey, out var edition))
            {
                throw new KeyNotFoundException($"Cannot find an edition of {modelKey} with key '{editionKey}'");
            }

            // --- Create the selected Spectrum model/edition
            DeviceInfoCollection devices;
            switch (modelKey)
            {
                case SpectrumModels.ZX_SPECTRUM_128:
                    devices = CreateSpectrum128Devices(edition);
                    break;
                case SpectrumModels.ZX_SPECTRUM_P3_E:
                    devices = CreateSpectrumP3Devices(edition);
                    break;
                default:
                    devices = CreateSpectrum48Devices(edition);
                    break;
            }

            // --- Setup the machine
            var machine = new SpectrumMachine(modelKey, editionKey, devices);
            return machine;
        }

        /// <summary>
        /// Creates a Spectrum 48K instance PAL edition
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrum48Pal()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.PAL);
        }

        /// <summary>
        /// Creates a Spectrum 48K instance PAL edition with turbo mode (2xCPU)
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrum48PalTurbo()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.PAL_2_X);
        }

        /// <summary>
        /// Creates a Spectrum 48K instance NTSC edition
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrum48Ntsc()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.NTSC);
        }

        /// <summary>
        /// Creates a Spectrum 48K instance NTSC edition with turbo mode
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrum48NtscTurbo()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.NTSC_2_X);
        }

        /// <summary>
        /// Creates a Spectrum 128K instance
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrum128()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_128, SpectrumModels.PAL);
        }

        /// <summary>
        /// Creates a Spectrum +3E instance
        /// </summary>
        /// <returns>The newly create Spectrum machine</returns>
        public static SpectrumMachine CreateSpectrumP3E()
        {
            return CreateMachine(SpectrumModels.ZX_SPECTRUM_P3_E, SpectrumModels.PAL);
        }

        /// <summary>
        /// Create the collection of devices for the Spectrum 48K virtual machine
        /// </summary>
        /// <param name="spectrumConfig">Machine configuration</param>
        /// <returns></returns>
        private static DeviceInfoCollection CreateSpectrum48Devices(SpectrumEdition spectrumConfig)
        {
            return new DeviceInfoCollection
            {
                new CpuDeviceInfo(spectrumConfig.Cpu),
                new RomDeviceInfo(GetProvider<IRomProvider>(false), spectrumConfig.Rom, new SpectrumRomDevice()),
                new MemoryDeviceInfo(spectrumConfig.Memory, new Spectrum48MemoryDevice()),
                new PortDeviceInfo(null, new Spectrum48PortDevice()),
                new KeyboardDeviceInfo(GetProvider<IKeyboardProvider>(), new KeyboardDevice()),
                new ScreenDeviceInfo(spectrumConfig.Screen),
                new BeeperDeviceInfo(spectrumConfig.Beeper, new BeeperDevice()),
                new TapeDeviceInfo(GetProvider<ITapeProvider>())
            };
        }

        /// <summary>
        /// Create the collection of devices for the Spectrum 48K virtual machine
        /// </summary>
        /// <param name="spectrumConfig">Machine configuration</param>
        /// <returns></returns>
        private static DeviceInfoCollection CreateSpectrum128Devices(SpectrumEdition spectrumConfig)
        {
            return new DeviceInfoCollection
            {
                new CpuDeviceInfo(spectrumConfig.Cpu),
                new RomDeviceInfo(GetProvider<IRomProvider>(false), spectrumConfig.Rom, new SpectrumRomDevice()),
                new MemoryDeviceInfo(spectrumConfig.Memory, new Spectrum128MemoryDevice()),
                new PortDeviceInfo(null, new Spectrum128PortDevice()),
                new KeyboardDeviceInfo(GetProvider<IKeyboardProvider>(), new KeyboardDevice()),
                new ScreenDeviceInfo(spectrumConfig.Screen),
                new BeeperDeviceInfo(spectrumConfig.Beeper, new BeeperDevice()),
                new TapeDeviceInfo(GetProvider<ITapeProvider>()),
                new SoundDeviceInfo(spectrumConfig.Sound, new SoundDevice())
            };
        }

        /// <summary>
        /// Create the collection of devices for the Spectrum +3E virtual machine
        /// </summary>
        /// <param name="spectrumConfig">Machine configuration</param>
        /// <returns></returns>
        private static DeviceInfoCollection CreateSpectrumP3Devices(SpectrumEdition spectrumConfig)
        {
            return new DeviceInfoCollection
            {
                new CpuDeviceInfo(spectrumConfig.Cpu),
                new RomDeviceInfo(GetProvider<IRomProvider>(false), spectrumConfig.Rom, new SpectrumRomDevice()),
                new MemoryDeviceInfo(spectrumConfig.Memory, new SpectrumP3MemoryDevice()),
                new PortDeviceInfo(null, new SpectrumP3PortDevice()),
                new KeyboardDeviceInfo(GetProvider<IKeyboardProvider>(), new KeyboardDevice()),
                new ScreenDeviceInfo(spectrumConfig.Screen),
                new BeeperDeviceInfo(spectrumConfig.Beeper, new BeeperDevice()),
                new TapeDeviceInfo(GetProvider<ITapeProvider>()),
                new SoundDeviceInfo(spectrumConfig.Sound, new SoundDevice())
            };
        }

        #endregion

        #region Machine properties

        /// <summary>
        /// The current machine's model key.
        /// </summary>
        public string ModelKey { get; }

        /// <summary>
        /// The current machine's edition key.
        /// </summary>
        public string EditionKey { get; }

        /// <summary>
        /// Gets or sets the state of the virtual machine
        /// </summary>
        public VmState MachineState
        {
            get => _machineState;
            private set
            {
                if (value == _machineState) return;

                var oldState = _machineState;
                _machineState = value;
                OnVmStateChanged(new VmStateChangedEventArgs(oldState, value));
            }
        }

        /// <summary>
        /// This event is raised when the state of the virtual machine has been changed.
        /// </summary>
        public event EventHandler<VmStateChangedEventArgs> VmStateChanged;

        /// <summary>
        /// Signs if the machine runs in debug mode.
        /// </summary>
        public bool RunsInDebugMode { get; private set; }

        /// <summary>
        /// The CPU of the machine
        /// </summary>
        public CpuZ80 Cpu { get; }

        /// <summary>
        /// Provides access to the individual ROM pages of the machine
        /// </summary>
        public IReadOnlyList<ReadOnlyMemorySlice> Roms { get; }

        /// <summary>
        /// Gets the number of ROM pages
        /// </summary>
        public int RomCount => Roms.Count;

        /// <summary>
        /// Allows to obtain paging information about the memory
        /// </summary>
        public MemoryPagingInfo PagingInfo { get; }

        /// <summary>
        /// The current Contents of the machine's 64K addressable memory
        /// </summary>
        public SpectrumMemoryContents Memory { get; }

        /// <summary>
        /// Provides access to the individual RAM banks of the machine
        /// </summary>
        public IReadOnlyList<MemorySlice> RamBanks { get; }

        /// <summary>
        /// Gets the number of RAM banks
        /// </summary>
        public int RamBankCount => RamBanks.Count;

        /// <summary>
        /// Allows to emulate keyboard keys and query the keyboard state
        /// </summary>
        public KeyboardEmulator Keyboard { get; }

        /// <summary>
        /// Allows read-only access to screen rendering configuration
        /// </summary>
        public ScreenConfiguration ScreenConfiguration { get; }

        /// <summary>
        /// Allows read-only access to the screen rendering table
        /// </summary>
        public ScreenRenderingTable ScreenRenderingTable { get; }

        /// <summary>
        /// A bitmap that represents the current visible screen's pixels, including the border
        /// </summary>
        public ScreenBitmap ScreenBitmap { get; }

        /// <summary>
        /// Gets the current screen rendering status of the machine.
        /// </summary>
        public ScreenRenderingStatus ScreenRenderingStatus { get; }

        /// <summary>
        /// Gets the beeper configuration of the machine
        /// </summary>
        public IAudioConfiguration BeeperConfiguration { get; }

        /// <summary>
        /// Gets the beeper samples of the current rendering frame
        /// </summary>
        public AudioSamples BeeperSamples { get; }

        /// <summary>
        /// Gets the sound (PSG) configuration of the machine
        /// </summary>
        public IAudioConfiguration SoundConfiguration { get; }

        /// <summary>
        /// Gets the sound (PSG) samples of the current rendering frame
        /// </summary>
        public AudioSamples AudioSamples { get; }

        /// <summary>
        /// The collection of breakpoints
        /// </summary>
        public CodeBreakpoints Breakpoints { get; }

        /// <summary>
        /// Indicates if the machine runs in real time mode
        /// </summary>
        public bool RealTimeMode { get; set; }

        /// <summary>
        /// Indicates if the machine renders the screen
        /// </summary>
        public bool DisableScreenRendering { get; set; }

        /// <summary>
        /// Gets the reason that tells why the machine has been stopped or paused
        /// </summary>
        public ExecutionCompletionReason ExecutionCompletionReason { get; private set; }

        /// <summary>
        /// This event is executed whenever the CPU frame has been completed.
        /// </summary>
        public event EventHandler<CancelEventArgs> CpuFrameCompleted;

        /// <summary>
        /// This event is executed whenever the render frame has been completed.
        /// </summary>
        public event EventHandler<CancelEventArgs> RenderFrameCompleted;

        #endregion

        #region Machine control methods

        /// <summary>
        /// Turns on the virtual machine.
        /// </summary>
        /// <remarks>
        /// If the virtual machine is turned on, this method does not change its state.
        /// </remarks>
        public void TurnOn()
        {
            if (MachineState != VmState.Off) return;

            MachineState = VmState.TurningOn;
            //TODO: Create the machine and load its devices.
            MachineState = VmState.On;
        }

        /// <summary>
        /// Starts the virtual machine and runs it until the execution cycle is completed for
        /// a reason.
        /// </summary>
        /// <param name="cancellationToken">Cancellation token</param>
        /// <param name="options">Virtual machine execution options</param>
        /// <returns></returns>
        public async Task<ExecutionCompletionReason> StartAndRun(CancellationToken cancellationToken,
            ExecuteCycleOptions options)
        {
            var lastFrameStart = _clockProvider.GetCounter();
            var completed = false;
            while (!completed)
            {
                // --- Execute a single CPU Frame
                var cancelled = _spectrumVm.ExecuteCycle(cancellationToken, options, true);
                if (cancelled) return ExecutionCompletionReason.Cancelled;

                // --- Do additional tasks when CPU frame completed
                var cancelArgs = new CancelEventArgs(false);
                CpuFrameCompleted?.Invoke(this, cancelArgs);
                if (cancelArgs.Cancel) return ExecutionCompletionReason.Cancelled;

                switch (_spectrumVm.ExecutionCompletionReason)
                {
                    case ExecutionCompletionReason.TerminationPointReached:
                    case ExecutionCompletionReason.BreakpointReached:
                    case ExecutionCompletionReason.Halted:
                    case ExecutionCompletionReason.Exception:
                        completed = true;
                        break;
                    case ExecutionCompletionReason.RenderFrameCompleted:
                        completed = options.EmulationMode == EmulationMode.UntilRenderFrameEnds;
                        break;
                }

            }

            // --- Done, pass back the reason of completing the run
            return _spectrumVm.ExecutionCompletionReason;
        }

        #endregion

        #region Helpers

        protected virtual void OnVmStateChanged(VmStateChangedEventArgs e)
        {
            VmStateChanged?.Invoke(this, e);
        }

        #endregion
    }

}
