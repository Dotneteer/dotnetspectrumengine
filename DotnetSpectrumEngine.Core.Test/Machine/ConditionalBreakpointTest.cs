using System.Threading;
using DotnetSpectrumEngine.Core.Abstraction.Machine;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.Core.Test.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Shouldly;

// ReSharper disable CommentTypo

namespace DotnetSpectrumEngine.Core.Test.Machine
{
    [TestClass]
    public class ConditionalBreakpointTest: ConditionalBreakpointTestBed
    {
        [TestMethod]
        public void MachineWorksWithNoDebugCondition()
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
            });

            var bp = CreateBreakpoint(null);
            debugProvider.Breakpoints.Add(0x8000, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x00);
            regs.C.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8000);
        }

        [TestMethod]
        public void CycleWorksWithNoDebugCondition()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(null);
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithLessHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<2");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithLessHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<3");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x01);
            regs.B.ShouldBe((byte)0x7F);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithLessHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<1");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithLessHitCondition4()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<2");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithLessThanOrEqualHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<=1");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithLessThanOrEqualHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<=2");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x01);
            regs.B.ShouldBe((byte)0x7F);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithLessThanOrEqualHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<=0");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithLessThanOrEqualHitCondition4()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("<=1");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithEqualHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("=1");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithEqualHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("=19");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x12);
            regs.B.ShouldBe((byte)0x6E);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithEqualHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("=999");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithEqualHitCondition4()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("=19");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x12);
            regs.B.ShouldBe((byte)0x6E);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithGreaterHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">0");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">3");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x03);
            regs.B.ShouldBe((byte)0x7D);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">3");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x03);
            regs.B.ShouldBe((byte)0x7D);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x04);
            regs.B.ShouldBe((byte)0x7C);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterHitCondition4()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">127");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7F);
            regs.B.ShouldBe((byte)0x01);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithGreaterHitCondition5()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">126");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7E);
            regs.B.ShouldBe((byte)0x02);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7F);
            regs.B.ShouldBe((byte)0x01);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 3)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithGreaterThanOrEqualHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">=1");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x00);
            regs.B.ShouldBe((byte)0x80);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterThanOrEqualHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">=4");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x03);
            regs.B.ShouldBe((byte)0x7D);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterThanOrEqualHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">=4");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x03);
            regs.B.ShouldBe((byte)0x7D);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x04);
            regs.B.ShouldBe((byte)0x7C);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithGreaterThanOrEqualHitCondition4()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">=128");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7F);
            regs.B.ShouldBe((byte)0x01);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithGreaterThanOrEqualHitCondition5()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint(">=127");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7E);
            regs.B.ShouldBe((byte)0x02);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x7F);
            regs.B.ShouldBe((byte)0x01);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 3)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }

        [TestMethod]
        public void CycleWorksWithMultiplyHitCondition1()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("*8");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));

            // --- Assert
            regs.A.ShouldBe((byte)0x07);
            regs.B.ShouldBe((byte)0x79);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithMultiplyHitCondition2()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("*8");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x07);
            regs.B.ShouldBe((byte)0x79);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x0F);
            regs.B.ShouldBe((byte)0x71);
            regs.PC.ShouldBe((ushort)0x8004);
        }

        [TestMethod]
        public void CycleWorksWithMultiplyHitCondition3()
        {
            // --- Arrange
            var spectrum = new SpectrumAdvancedTestMachine();
            var debugProvider = new TestDebugInfoProvider();
            spectrum.SetDebugInfoProvider(debugProvider);

            // --- We render the screen while the interrupt is disabled
            spectrum.InitCode(new byte[]
            {
                0x06, 0x80,       // LD B,$80
                0x3E, 0x00,       // LD A,$00
                0x3C,             // INC A
                0x10, 0xFD,       // DJNZ -3
                0x76,             // HALT
            });

            var bp = CreateBreakpoint("*100");
            debugProvider.Breakpoints.Add(0x8004, bp);
            debugProvider.Breakpoints.Add(0x8007, MinimumBreakpointInfo.EmptyBreakpointInfo);
            var regs = spectrum.Cpu.Registers;

            // --- Act/Assert (Run 1)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)99);
            regs.B.ShouldBe((byte)29);
            regs.PC.ShouldBe((ushort)0x8004);

            // --- Act/Assert (Run 2)
            spectrum.ExecuteCycle(CancellationToken.None, new ExecuteCycleOptions(EmulationMode.Debugger));
            regs.A.ShouldBe((byte)0x80);
            regs.B.ShouldBe((byte)0x00);
            regs.PC.ShouldBe((ushort)0x8007);
        }
    }
}
