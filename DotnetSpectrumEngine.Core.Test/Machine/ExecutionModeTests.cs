﻿using System.Threading;
using DotnetSpectrumEngine.Core.Abstraction.Machine;
using DotnetSpectrumEngine.Core.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

namespace DotnetSpectrumEngine.Core.Test.Machine
{
    [TestClass]
    public class ExecutionModeTests
    {
        [TestMethod]
        public void DoesNotStopAtTerminationPointInNormalExecutionMode()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x3E, 0x10,       // LD A,$10
                0x87,             // ADD A,A
                0x47,             // LD B,A
                0x4F,             // LD C,A
                0x76              // HALT
            });
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, 
                new ExecuteCycleOptions(EmulationMode.UntilHalt, terminationPoint: 0x0002));

            // --- Assert
            regs.A.ShouldBe((byte)0x20);
            regs.B.ShouldBe((byte)0x20);
            regs.C.ShouldBe((byte)0x20);
            regs.PC.ShouldBe((ushort)0x8005);
        }

        [TestMethod]
        public void StopsAtTerminationPointWhenRequested()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x3E, 0x10,       // LD A,$10
                0x87,             // ADD A,A
                0x47,             // LD B,A
                0x4F,             // LD C,A
                0x76              // HALT
            });
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None,
                new ExecuteCycleOptions(EmulationMode.UntilExecutionPoint, terminationPoint: 0x8003));

            // --- Assert
            regs.A.ShouldBe((byte)0x20);
            regs.B.ShouldBe((byte)0x00);
            regs.C.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8003);
        }
    }
}
