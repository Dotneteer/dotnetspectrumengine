using System;

namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This class stores the exception raised in the ZX Spectrum virtual machine.
    /// </summary>
    public class VmExceptionArgs: EventArgs
    {
        public VmExceptionArgs(Exception exception)
        {
            Exception = exception;
        }

        /// <summary>
        /// The exception raised
        /// </summary>
        public Exception Exception { get; }
    }
}