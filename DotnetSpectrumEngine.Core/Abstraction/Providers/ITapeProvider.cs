using System.IO;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Tape;

namespace DotnetSpectrumEngine.Core.Abstraction.Providers
{
    /// <summary>
    /// This interface describes the behavior of an object that
    /// provides TZX/TAP tape content.
    /// </summary>
    public interface ITapeProvider: IVmComponentProvider
    {
        /// <summary>
        /// Tha tape set to load the content from.
        /// </summary>
        string TapeSetName { get; set; }

        /// <summary>
        /// Gets a binary reader that provides tape content
        /// </summary>
        /// <returns>BinaryReader instance to obtain the content from.</returns>
        BinaryReader GetTapeContent();

        /// <summary>
        /// Creates a tape file with the specified name.
        /// </summary>
        /// <returns></returns>
        void CreateTapeFile();

        /// <summary>
        /// This method sets the name of the file according to the 
        /// Spectrum SAVE HEADER information.
        /// </summary>
        /// <param name="name"></param>
        void SetName(string name);

        /// <summary>
        /// Appends the tape block to the tape file.
        /// </summary>
        /// <param name="block">Tape block</param>
        void SaveTapeBlock(ITapeDataSerialization block);

        /// <summary>
        /// The tape provider can finalize the tape when all 
        /// tape blocks are written.
        /// </summary>
        void FinalizeTapeFile();
    }
}