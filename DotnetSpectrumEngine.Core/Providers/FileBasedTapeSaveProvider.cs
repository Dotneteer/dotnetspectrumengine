using System;
using System.IO;
using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Abstraction.Devices.Tape;
using DotnetSpectrumEngine.Core.Abstraction.Providers;
using DotnetSpectrumEngine.Core.Devices.Tape.Tzx;

namespace DotnetSpectrumEngine.Core.Providers
{
    /// <summary>
    /// This is the default tape provider used by the scripting engine
    /// </summary>
    public class FileBasedTapeSaveProvider : ITapeSaveProvider
    {
        public const string DEFAULT_NAME = "SavedFile";
        public const string DEFAULT_EXT = ".tzx";

        private string _suggestedName;
        private string _fullFileName;
        private int _dataBlockCount;

        public FileBasedTapeSaveProvider()
        {
            SaveFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile),
                "SavedSpectrumFiles");
        }

        /// <summary>
        /// Folder to save the ZX Spectrum files
        /// </summary>
        public string SaveFolder { get; set; }

        /// <summary>
        /// The component provider should be able to reset itself
        /// </summary>
        public void Reset()
        {
            _dataBlockCount = 0;
            _suggestedName = null;
            _fullFileName = null;
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
        /// Creates a tape file with the specified name
        /// </summary>
        /// <returns></returns>
        public void CreateTapeFile()
        {
            Reset();
        }

        /// <summary>
        /// This method sets the name of the file according to the 
        /// Spectrum SAVE HEADER information
        /// </summary>
        /// <param name="name"></param>
        public void SetName(string name)
        {
            _suggestedName = name;
        }

        /// <summary>
        /// Appends the TZX block to the tape file
        /// </summary>
        /// <param name="block"></param>
        public void SaveTapeBlock(ITapeDataSerialization block)
        {
            if (_dataBlockCount == 0)
            {
                if (!Directory.Exists(SaveFolder))
                {
                    Directory.CreateDirectory(SaveFolder);
                }
                var baseFileName = $"{_suggestedName ?? DEFAULT_NAME}_{DateTime.Now:yyyyMMdd_HHmmss}{DEFAULT_EXT}";
                _fullFileName = Path.Combine(SaveFolder, baseFileName);
                using (var writer = new BinaryWriter(File.Create(_fullFileName)))
                {
                    var header = new TzxHeader();
                    header.WriteTo(writer);
                }
            }
            _dataBlockCount++;

            var stream = File.Open(_fullFileName, FileMode.Append);
            using (var writer = new BinaryWriter(stream))
            {
                block.WriteTo(writer);
            }
        }
    }
}