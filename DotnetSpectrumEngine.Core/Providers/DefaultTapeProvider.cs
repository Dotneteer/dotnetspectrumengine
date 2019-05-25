using System.IO;
using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Tape;
using DotnetSpectrumEngine.Core.Abstraction.Providers;

namespace DotnetSpectrumEngine.Core.Providers
{
    /// <summary>
    /// This is the default tape provider used by the scripting engine
    /// </summary>
    public class DefaultTapeProvider : ITapeProvider
    {
        /// <summary>
        /// The component provider should be able to reset itself
        /// </summary>
        public void Reset()
        {
        }

        /// <summary>
        /// The virtual machine that hosts the provider
        /// </summary>
        public ISpectrumVm HostVm { get; set; }

        /// <summary>
        /// Signs that the provider has been attached to the Spectrum virtual machine
        /// </summary>
        public void OnAttachedToVm(ISpectrumVm hostVm)
        {
            HostVm = hostVm;
        }

        /// <summary>
        /// Tha tape set to load the content from
        /// </summary>
        public string TapeSetName { get; set; }

        /// <summary>
        /// Gets a binary reader that provider TZX content
        /// </summary>
        /// <returns>BinaryReader instance to obtain the content from</returns>
        public BinaryReader GetTapeContent()
        {
            return null;
        }

        /// <summary>
        /// Creates a tape file with the specified name
        /// </summary>
        /// <returns></returns>
        public void CreateTapeFile()
        {
        }

        /// <summary>
        /// This method sets the name of the file according to the 
        /// Spectrum SAVE HEADER information
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
        }

        /// <summary>
        /// Appends the TZX block to the tape file
        /// </summary>
        /// <param name="block"></param>
        public void SaveTapeBlock(ITapeDataSerialization block)
        {
        }

        /// <summary>
        /// The tape provider can finalize the tape when all 
        /// TZX blocks are written.
        /// </summary>
        public void FinalizeTapeFile()
        {
        }
    }

}