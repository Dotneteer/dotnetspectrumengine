using System.Reflection;
using DotnetSpectrumEngine.Core;
using DotnetSpectrumEngine.Core.Abstraction.Providers;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.Core.Providers;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Machine;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Mvvm;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Providers;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf
{
    /// <summary>
    /// This class represents the view model of the app
    /// </summary>
    public class AppViewModel : EnhancedViewModelBase
    {
        /// <summary>
        /// The singleton instance of the application's view model
        /// </summary>
        public static AppViewModel Default { get; private set; }

        /// <summary>
        /// The keyboard provider of the machine
        /// </summary>
        public static KeyboardScanner KeyboardScanner { get; private set; }

        /// <summary>
        /// The tape load provider of the machine
        /// </summary>
        public static ResourceBasedTapeLoadProvider TapeLoadProvider { get; private set; }

        /// <summary>
        /// Resets the app's singleton view model at startup time
        /// </summary>
        static AppViewModel()
        {
            Reset();
        }

        /// <summary>
        /// Resets the application's singleton view model at any time
        /// </summary>
        public static void Reset()
        {
            SpectrumMachine.Reset();
            SpectrumMachine.RegisterDefaultProviders();
            SpectrumMachine.RegisterProvider<IBeeperProvider>(() => new AudioWaveProvider());
            TapeLoadProvider = new ResourceBasedTapeLoadProvider(Assembly.GetExecutingAssembly());
            SpectrumMachine.RegisterProvider<ITapeLoadProvider>(() => TapeLoadProvider);
            KeyboardScanner = new KeyboardScanner();
            Default = new AppViewModel();
        }

        /// <summary>
        /// Hides the constructor from external actors
        /// </summary>
        private AppViewModel()
        {
            var machine = SpectrumMachine.CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.PAL);
            var vm = MachineViewModel = new MachineViewModel(machine);
            vm.DisplayMode = SpectrumDisplayMode.Fit;
            vm.AssignTapeSetName.Execute("Pac-Man.tzx");
        }

        /// <summary>
        /// Contains the view model used by Spectrum control
        /// </summary>
        public MachineViewModel MachineViewModel { get; }

    }
}