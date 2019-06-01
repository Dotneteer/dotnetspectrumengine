using DotnetSpectrumEngine.Core;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Machine;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Mvvm;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf
{
    /// <summary>
    /// This class represents the view model of the app
    /// </summary>
    public class AppViewModel : EnhancedViewModelBase
    {
        /// <summary>
        /// The singleton instance of the app's view model
        /// </summary>
        public static AppViewModel Default { get; private set; }

        /// <summary>
        /// Resets the app's singleton view model at startup time
        /// </summary>
        static AppViewModel()
        {
            Reset();
        }

        /// <summary>
        /// Resets the app's singleton view model at any time
        /// </summary>
        public static void Reset()
        {
            SpectrumMachine.Reset();
            SpectrumMachine.RegisterDefaultProviders();
            Default = new AppViewModel();
        }

        /// <summary>
        /// Hides the constructor from external actors
        /// </summary>
        private AppViewModel()
        {
            var machine = SpectrumMachine.CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.PAL);
            var vm = MachineViewModel = new MachineViewModel(machine);
            vm.AllowKeyboardScan = true;
            vm.DisplayMode = SpectrumDisplayMode.Fit;
            vm.TapeSetName = "Pac-Man.tzx";
        }

        /// <summary>
        /// Contains the view model used by Spectrum control
        /// </summary>
        public MachineViewModel MachineViewModel { get; }
    }
}