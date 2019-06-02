using System.Reflection;
using DotnetSpectrumEngine.Core;
using DotnetSpectrumEngine.Core.Abstraction.Providers;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.Core.Providers;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Providers;
using GalaSoft.MvvmLight;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf.ViewModels
{
    /// <summary>
    /// This class represents the view model of the app
    /// </summary>
    public class AppViewModel : ViewModelBase
    {
        /// <summary>
        /// The Singleton instance of this class
        /// </summary>
        public static AppViewModel Default { get; private set; }

        /// <summary>
        /// The keyboard provider of the machine
        /// </summary>
        public static KeyboardScanner KeyboardScanner { get; private set; }

        /// <summary>
        /// The default beeper provider 
        /// </summary>
        public static AudioWaveProvider BeeperProvider { get; private set; }

        /// <summary>
        /// The default sound provider 
        /// </summary>
        public static AudioWaveProvider SoundProvider { get; private set; }

        /// <summary>
        /// The tape load provider of the machine
        /// </summary>
        public static ResourceBasedTapeLoadProvider TapeLoadProvider { get; private set; }

        /// <summary>
        /// Resets the application's singleton view model at startup time
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
            BeeperProvider = new AudioWaveProvider();
            SpectrumMachine.RegisterProvider<IBeeperProvider>(() => BeeperProvider);
            SoundProvider = new AudioWaveProvider(AudioProviderType.Psg);
            SpectrumMachine.RegisterProvider<ISoundProvider>(() => SoundProvider);
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
            vm.AssignTapeSetName.Execute("Pac-Man.tzx");
        }

        /// <summary>
        /// Contains the view model used by Spectrum control
        /// </summary>
        public MachineViewModel MachineViewModel { get; }
    }
}