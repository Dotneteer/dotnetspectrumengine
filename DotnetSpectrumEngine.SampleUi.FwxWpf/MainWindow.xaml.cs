using System.Windows;
using DotnetSpectrumEngine.SampleUi.FwxWpf.ViewModels;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = AppViewModel.Default;

            // ---We need to stop playing sound whenever the app closes
            Application.Current.Exit += (sender, obj) =>
                AppViewModel.BeeperProvider?.KillSound();

            // --- Take care of resizing the ZX Spectrum control 
            SpectrumControl.Loaded += (sender, args) => Resize();
            MainPanel.SizeChanged += (sender, args) => Resize();

            void Resize() => SpectrumControl.ResizeFor(MainPanel.ActualWidth, MainPanel.ActualHeight);
        }
    }
}