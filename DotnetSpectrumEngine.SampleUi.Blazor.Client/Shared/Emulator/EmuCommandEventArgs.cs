using System;

namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Shared.Emulator
{
    /// <summary>
    /// This class represents an emulator command event argument
    /// </summary>
    public class EmuCommandEventArgs : EventArgs
    {
        public EmuCommandEventArgs(string command)
        {
            Command = command;
        }

        /// <summary>
        /// Emulator command to execute
        /// </summary>
        public string Command { get; }
    }
}
