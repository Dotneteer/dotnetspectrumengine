using DotnetSpectrumEngine.Core.Abstraction.Machine;

namespace DotnetSpectrumEngine.Core.Machine
{
    /// <summary>
    /// This class stores minimum breakpoint information
    /// </summary>
    public class MinimumBreakpointInfo : IBreakpointInfo
    {
        /// <summary>
        /// Empty breakpoint information
        /// </summary>
        public static IBreakpointInfo EmptyBreakpointInfo = new MinimumBreakpointInfo();

        /// <summary>
        /// This flag shows that the breakpoint is assigned to the
        /// CPU (and not to some source code).
        /// </summary>
        public bool IsCpuBreakpoint => true;

        /// <summary>
        /// Type of breakpoint hit condition
        /// </summary>
        public BreakpointHitType HitType => BreakpointHitType.None;

        /// <summary>
        /// Value of the hit condition
        /// </summary>
        public ushort HitConditionValue => 0;

        /// <summary>
        /// The current hit count value
        /// </summary>
        public int CurrentHitCount { get; set; }
    }

}