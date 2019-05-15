﻿// ReSharper disable InconsistentNaming
namespace DotnetSpectrumEngine.Core.Abstraction.Cpu
{
    /// <summary>
    /// 26-bit Register indexes.
    /// </summary>
    /// <remarks>
    /// This enum defines indexes to access register values through the
    /// C# this[] operator.
    /// </remarks>
    public enum Reg16Index : byte
    {
        BC = 0,
        DE = 1,
        HL = 2,
        SP = 3
    }
}