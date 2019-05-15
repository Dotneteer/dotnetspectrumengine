using DotnetSpectrumEngine.Core.Abstraction.Machine;

namespace DotnetSpectrumEngine.Core.Test.Machine
{
    public class ConditionalBreakpointTestBed
    {
        protected static IBreakpointInfo CreateBreakpoint(string hitCondition)
        {
            var breakpoint = new BreakpointInfo
            {
                IsCpuBreakpoint = true,
                HitType = BreakpointHitType.None
            };
            if (hitCondition != null)
            {
                var condStart = 1;
                if (hitCondition.StartsWith("<="))
                {
                    breakpoint.HitType = BreakpointHitType.LessOrEqual;
                    condStart = 2;
                }
                else if (hitCondition.StartsWith(">="))
                {
                    breakpoint.HitType = BreakpointHitType.GreaterOrEqual;
                    condStart = 2;
                }
                else if (hitCondition.StartsWith("<"))
                {
                    breakpoint.HitType = BreakpointHitType.Less;
                }
                else if (hitCondition.StartsWith(">"))
                {
                    breakpoint.HitType = BreakpointHitType.Greater;
                }
                else if (hitCondition.StartsWith("="))
                {
                    breakpoint.HitType = BreakpointHitType.Equal;
                }
                else if (hitCondition.StartsWith("*"))
                {
                    breakpoint.HitType = BreakpointHitType.Multiple;
                }
                breakpoint.HitConditionValue = ushort.Parse(hitCondition.Substring(condStart));
            }
            return breakpoint;
        }

        /// <summary>
        /// This class holds breakpoint information used to debug Z80 Assembler
        /// source code
        /// </summary>
        protected class BreakpointInfo : IBreakpointInfo
        {
            /// <summary>
            /// This flag shows that the breakpoint is assigned to source code.
            /// </summary>
            public bool IsCpuBreakpoint { get; set; }

            /// <summary>
            /// Type of breakpoint hit condition
            /// </summary>
            public BreakpointHitType HitType { get; set; }

            /// <summary>
            /// Value of the hit condition
            /// </summary>
            public ushort HitConditionValue { get; set; }

            /// <summary>
            /// The current hit count value
            /// </summary>
            public int CurrentHitCount { get; set; }
        }
    }
}