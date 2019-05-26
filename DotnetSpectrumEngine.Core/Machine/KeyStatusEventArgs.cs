using System;
using System.Collections.Generic;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Keyboard;

namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This class represents the event arguments of the KeyScanning event
    /// </summary>
    public class KeyStatusEventArgs: EventArgs
    {
        /// <summary>
        /// Key statuses to pass to the provider
        /// </summary>
        public List<KeyStatus> KeyStatusList { get; } = new List<KeyStatus>();

        /// <summary>
        /// Registers a change in the key status
        /// </summary>
        /// <param name="key">Key code</param>
        /// <param name="isDown">Is the key pressed down?</param>
        public void RegisterKeyStatus(SpectrumKeyCode key, bool isDown)
        {
            KeyStatusList.Add(new KeyStatus(key, isDown));
        }
    }
}