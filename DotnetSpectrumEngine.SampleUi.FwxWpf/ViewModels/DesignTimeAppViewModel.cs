using GalaSoft.MvvmLight;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf.ViewModels
{
    /// <summary>
    /// This class represents the design time version view model of the app
    /// </summary>
    /// <remarks>
    /// The sole aim of this class is to provide a design time help for the
    /// XAML editor to carry out data binding
    /// </remarks>

    public class DesignTimeAppViewModel : ViewModelBase
    {
        /// <summary>
        /// Hides the constructor from external actors
        /// </summary>
        public DesignTimeAppViewModel()
        {
            MachineViewModel = new MachineViewModel();
        }

        /// <summary>
        /// Contains the view model used by Spectrum control
        /// </summary>
        public MachineViewModel MachineViewModel { get; }
    }
}