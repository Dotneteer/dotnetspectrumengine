﻿using System;
using DotnetSpectrumEngine.Core.Abstraction.Cpu;
using DotnetSpectrumEngine.Core.Abstraction.Devices;
using DotnetSpectrumEngine.Core.Cpu;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

// ReSharper disable UnusedAutoPropertyAccessor.Local

// ReSharper disable InconsistentNaming

namespace DotnetSpectrumEngine.Core.Test.Cpu
{
    [TestClass]
    public class Z80ExecutionCycleTest
    {
        [TestMethod]
        public void FullResetWhenCreatingZ80()
        {
            // --- Act
            var z80 = CreateZ80Cpu();

            // --- Assert
            z80.Registers.AF.ShouldBe((ushort)0);
            z80.Registers.BC.ShouldBe((ushort)0);
            z80.Registers.DE.ShouldBe((ushort)0);
            z80.Registers.HL.ShouldBe((ushort)0);
            z80.Registers._AF_.ShouldBe((ushort)0);
            z80.Registers._BC_.ShouldBe((ushort)0);
            z80.Registers._DE_.ShouldBe((ushort)0);
            z80.Registers._HL_.ShouldBe((ushort)0);
            z80.Registers.PC.ShouldBe((ushort)0);
            z80.Registers.SP.ShouldBe((ushort)0);
            z80.Registers.IX.ShouldBe((ushort)0);
            z80.Registers.IY.ShouldBe((ushort)0);
            z80.Registers.IR.ShouldBe((ushort)0);
            z80.Registers.WZ.ShouldBe((ushort)0);

            (z80.StateFlags & Z80StateFlags.Reset).ShouldBe(Z80StateFlags.None);
            (z80.StateFlags & Z80StateFlags.Int).ShouldBe(Z80StateFlags.None);
            z80.IsInterruptBlocked.ShouldBeFalse();
            (z80.StateFlags & Z80StateFlags.Nmi).ShouldBe(Z80StateFlags.None);
            z80.InterruptMode.ShouldBe((byte)0);
            z80.PrefixMode.ShouldBe(OpPrefixMode.None);
            z80.IndexMode.ShouldBe(OpIndexMode.None);
            z80.Tacts.ShouldBe(0L);
        }

        [TestMethod]
        public void HaltedStateExecutesNops()
        {
            // --- Arrange
            var z80 = CreateZ80Cpu();
            z80.StateFlags |= Z80StateFlags.Halted;

            // --- Act
            var ticksBefore = z80.Tacts;
            var regRBefore = z80.Registers.R;

            z80.ExecuteCpuCycle();
            var ticksAfter = z80.Tacts;
            var regRAfter = z80.Registers.R;

            // --- Assert
            ticksBefore.ShouldBe(0L);
            regRBefore.ShouldBe((byte)0x00);
            ticksAfter.ShouldBe(4L);
            regRAfter.ShouldBe((byte)0x01);
            (z80.StateFlags & Z80StateFlags.Halted).ShouldBe(Z80StateFlags.Halted);
        }

        [TestMethod]
        public void RSTSignalIsProcessed()
        {
            // --- Arrange
            var z80 = CreateZ80Cpu();
            z80.Registers.AF = 0x0001;
            z80.Registers.BC = 0x2345;
            z80.Registers.DE = 0x3456;
            z80.Registers.HL = 0x4567;
            z80.Registers.SP = 0x5678;
            z80.Registers.PC = 0x6789;
            z80.Registers.IR = 0x789A;
            z80.Registers.IX = 0x89AB;
            z80.Registers.IY = 0x9ABC;
            z80.Registers._AF_ = 0x9876;
            z80.Registers._BC_ = 0x8765;
            z80.Registers._DE_ = 0x7654;
            z80.Registers._HL_ = 0x6543;

            z80.BlockInterrupt();
            z80.IFF1 = true;
            z80.IFF2 = true;
            z80.PrefixMode = OpPrefixMode.Bit;
            z80.IndexMode = OpIndexMode.IY;
            z80.SetInterruptMode(2);
            z80.SetTacts(1000);

            // --- Act
            z80.StateFlags = Z80StateFlags.Reset;
            z80.ExecuteCpuCycle();

            // --- Assert
            z80.Registers.AF.ShouldBe((ushort)0x0001);
            z80.Registers.BC.ShouldBe((ushort)0x2345);
            z80.Registers.DE.ShouldBe((ushort)0x3456);
            z80.Registers.HL.ShouldBe((ushort)0x4567);
            z80.Registers.SP.ShouldBe((ushort)0x5678);
            z80.Registers.PC.ShouldBe((ushort)0);
            z80.Registers.IR.ShouldBe((ushort)0);
            z80.Registers.IX.ShouldBe((ushort)0x89AB);
            z80.Registers.IY.ShouldBe((ushort)0x9ABC);
            z80.Registers._AF_.ShouldBe((ushort)0x9876);
            z80.Registers._BC_.ShouldBe((ushort)0x8765);
            z80.Registers._DE_.ShouldBe((ushort)0x7654);
            z80.Registers._HL_.ShouldBe((ushort)0x6543);

            z80.IsInterruptBlocked.ShouldBeFalse();
            z80.IFF1.ShouldBeFalse();
            z80.IFF2.ShouldBeFalse();
            z80.PrefixMode.ShouldBe(OpPrefixMode.None);
            z80.IndexMode.ShouldBe(OpIndexMode.None);
            z80.InterruptMode.ShouldBe((byte)0);
            z80.Tacts.ShouldBe(0);
            (z80.StateFlags & Z80StateFlags.Reset).ShouldBe(Z80StateFlags.None);
        }

        private Z80Cpu CreateZ80Cpu()
        {
            var z80 = new Z80Cpu(new Z80TestMemoryDevice(), new Z80TestPortDevice())
            {
                StackDebugSupport = new DefaultStackDebugSupport()
            };
            return z80;
        }

        private class Z80TestMemoryDevice : IMemoryDevice
        {
            public byte Read(ushort addr, bool suppressContention) => 0;

            public void Write(ushort addr, byte value, bool suppressContention = false)
            { }

            public void ContentionWait(ushort addr)
            {
            }

            public byte[] CloneMemory() => null;

            public void CopyRom(byte[] buffer)
            {
                throw new NotImplementedException();
            }

            public void SelectRom(int romIndex)
            {
                throw new NotImplementedException();
            }

            public int GetSelectedRomIndex()
            {
                throw new NotImplementedException();
            }

            public void PageIn(int slot, int bank)
            {
                throw new NotImplementedException();
            }

            public int GetSelectedBankIndex(int slot)
            {
                throw new NotImplementedException();
            }

            public bool UsesShadowScreen { get; set; }
            public bool IsInAllRamMode => false;
            public bool IsIn8KMode => false;

            public byte[] GetRomBuffer(int romIndex)
            {
                throw new NotImplementedException();
            }

            public byte[] GetRamBank(int bankIndex)
            {
                throw new NotImplementedException();
            }

            public (bool IsInRom, int Index, ushort Address) GetAddressLocation(ushort addr)
            {
                throw new NotImplementedException();
            }

            public bool IsRamBankPagedIn(int index, out ushort baseAddress)
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
            }

            IDeviceState IDevice.GetState()
            {
                throw new NotImplementedException();
            }

            public void RestoreState(IDeviceState state)
            {
                throw new NotImplementedException();
            }

            public ISpectrumVm HostVm { get; set; }

            public void OnAttachedToVm(ISpectrumVm hostVm)
            {
            }
        }

        private class Z80TestPortDevice : IPortDevice
        {
            public byte ReadPort(ushort addr) => 0xFF;
            public void WritePort(ushort addr, byte data) { }

            public void ContentionWait(ushort addr)
            {
            }

            public void Reset() { }

            IDeviceState IDevice.GetState()
            {
                throw new NotImplementedException();
            }

            public void RestoreState(IDeviceState state)
            {
                throw new NotImplementedException();
            }

            public ISpectrumVm HostVm { get; set; }

            public void OnAttachedToVm(ISpectrumVm hostVm)
            {
            }
        }

        private class Z80SimpleMemoryDevice : IMemoryDevice
        {
            private readonly byte[] _buffer = new byte[1024];

            public Z80SimpleMemoryDevice()
            {
                for (var i = 0; i < _buffer.Length; i++)
                {
                    _buffer[i] = 0x00;
                }
            }

            public byte Read(ushort addr, bool suppressContention) => _buffer[addr];

            public void Write(ushort addr, byte value, bool suppressContention = false)

            {
                _buffer[addr] = value;
            }

            public void ContentionWait(ushort addr)
            {
            }

            public byte[] CloneMemory() => null;

            public void CopyRom(byte[] buffer)
            {
                throw new NotImplementedException();
            }

            public void SelectRom(int romIndex)
            {
                throw new NotImplementedException();
            }

            public int GetSelectedRomIndex()
            {
                throw new NotImplementedException();
            }

            public void PageIn(int slot, int bank)
            {
                throw new NotImplementedException();
            }

            public int GetSelectedBankIndex(int slot)
            {
                throw new NotImplementedException();
            }

            public bool UsesShadowScreen { get; set; }

            public bool IsInAllRamMode => false;

            public bool IsIn8KMode => false;

            public byte[] GetRomBuffer(int romIndex)
            {
                throw new NotImplementedException();
            }

            public byte[] GetRamBank(int bankIndex)
            {
                throw new NotImplementedException();
            }

            public (bool IsInRom, int Index, ushort Address) GetAddressLocation(ushort addr)
            {
                throw new NotImplementedException();
            }

            public bool IsRamBankPagedIn(int index, out ushort baseAddress)
            {
                throw new NotImplementedException();
            }

            public void Reset()
            {
            }

            IDeviceState IDevice.GetState()
            {
                throw new NotImplementedException();
            }

            public void RestoreState(IDeviceState state)
            {
                throw new NotImplementedException();
            }

            public ISpectrumVm HostVm { get; set; }

            public void OnAttachedToVm(ISpectrumVm hostVm)
            {
            }

        }
    }
}
