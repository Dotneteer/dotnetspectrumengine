﻿using System;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.SampleUi.FwxWpf.Mvvm;

namespace DotnetSpectrumEngine.SampleUi.FwxWpf.Machine
{
    /// <summary>
    /// This view model represents the view that displays a run time Spectrum virtual machine
    /// </summary>
    public class MachineViewModel: EnhancedViewModelBase, IDisposable
    {
        private SpectrumDisplayMode _displayMode;
        private string _tapeSetName;

        #region ViewModel properties

        /// <summary>
        /// The Spectrum machine to use in the emulator
        /// </summary>
        public SpectrumMachine Machine { get; }

        /// <summary>
        /// The current state of the virtual machine
        /// </summary>
        public VmState MachineState => Machine.MachineState;

        /// <summary>
        /// The current display mode of the Spectrum control
        /// </summary>
        public SpectrumDisplayMode DisplayMode
        {
            get => _displayMode;
            set => Set(ref _displayMode, value);
        }

        /// <summary>
        /// Initializes the ZX Spectrum virtual machine
        /// </summary>
        public RelayCommand StartVmCommand { get; set; }

        /// <summary>
        /// Pauses the virtual machine
        /// </summary>
        public RelayCommand PauseVmCommand { get; set; }

        /// <summary>
        /// Stops the ZX Spectrum virtual machine
        /// </summary>
        public RelayCommand StopVmCommand { get; set; }

        /// <summary>
        /// Resets the ZX Spectrum virtual machine
        /// </summary>
        public RelayCommand ResetVmCommand { get; set; }

        /// <summary>
        /// Sets the zoom according to the specified string
        /// </summary>
        public RelayCommand<SpectrumDisplayMode> SetZoomCommand { get; set; }

        /// <summary>
        /// Assigns the tape set name to the load content provider
        /// </summary>
        public RelayCommand<string> AssignTapeSetName { get; set; }

        /// <summary>
        /// Gets the flag that indicates if fast load mode is allowed
        /// </summary>
        public bool FastTapeMode { get; set; }

        /// <summary>
        /// Signs when the display mode changes
        /// </summary>
        public event EventHandler DisplayModeChanged; 

        #endregion

        #region Life cycle methods

        /// <summary>
        /// Initializes the view model
        /// </summary>
        public MachineViewModel()
        {
            DisplayMode = SpectrumDisplayMode.Fit;
            StartVmCommand = new RelayCommand(
                OnStartVm, 
                () => MachineState != VmState.Running);
            PauseVmCommand = new RelayCommand(
                OnPauseVm, 
                () => MachineState == VmState.Running);
            StopVmCommand = new RelayCommand(
                OnStopVm, 
                () => MachineState == VmState.Running || MachineState == VmState.Paused);
            ResetVmCommand = new RelayCommand(
                OnResetVm, 
                () => MachineState == VmState.Running || MachineState == VmState.Paused);
            SetZoomCommand = new RelayCommand<SpectrumDisplayMode>(OnZoomSet);
            AssignTapeSetName = new RelayCommand<string>(OnAssignTapeSet);
            Machine = SpectrumMachine.CreateSpectrum48Pal();
        }

        /// <summary>
        /// Initializes the view model with the specified machine
        /// </summary>
        /// <param name="machine"></param>
        public MachineViewModel(SpectrumMachine machine) : this()
        {
            Machine = machine;
            Machine.VmStateChanged += OnMachineStateChanged;
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, 
        /// releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Machine.VmStateChanged -= OnMachineStateChanged;
        }

        #endregion

        #region Virtual machine command handlers

        /// <summary>
        /// Starts the Spectrum virtual machine
        /// </summary>
        protected virtual void OnStartVm()
        {
            Machine.Start();
        }

        /// <summary>
        /// Pauses the Spectrum virtual machine
        /// </summary>
        protected virtual async void OnPauseVm()
        {
            await Machine.Pause();
        }

        /// <summary>
        /// Stops the Spectrum virtual machine
        /// </summary>
        protected virtual async void OnStopVm()
        {
            await Machine.Stop();
        }

        /// <summary>
        /// Resets the Spectrum virtual machine
        /// </summary>
        protected virtual async void OnResetVm()
        {
            await Machine.Stop();
            Machine.Start();
        }

        /// <summary>
        /// Sets the zoom mode of the virtual machine display
        /// </summary>
        /// <param name="zoom"></param>
        protected virtual void OnZoomSet(SpectrumDisplayMode zoom)
        {
            DisplayMode = zoom;
            DisplayModeChanged?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Assigns the specified tape set name to the load content provider
        /// </summary>
        /// <param name="tapeSetName"></param>
        protected virtual void OnAssignTapeSet(string tapeSetName)
        {
            AppViewModel.TapeLoadProvider.ResourceName = $"TzxResources.{tapeSetName}";
        }

        #endregion

        #region Helpers

        private void OnMachineStateChanged(object sender, VmStateChangedEventArgs e)
        {
            RaisePropertyChanged(nameof(MachineState));
            StartVmCommand.RaiseCanExecuteChanged();
            PauseVmCommand.RaiseCanExecuteChanged();
            StopVmCommand.RaiseCanExecuteChanged();
            ResetVmCommand.RaiseCanExecuteChanged();
        }

        #endregion
    }
}