namespace DotnetSpectrumEngine.Core.Abstraction.Configuration
{
    /// <summary>
    /// This class can be used to describe a Spectrum model's screen data
    /// for configuration.
    /// </summary>
    public sealed class ScreenConfigurationData : IScreenConfiguration
    {
        /// <summary>
        /// The tact index of the interrupt relative to the top-left
        /// screen pixel.
        /// </summary>
        public int InterruptTact { get; set; }

        /// <summary>
        /// Number of lines used for vertical synch.
        /// </summary>
        public int VerticalSyncLines { get; set; }

        /// <summary>
        /// The number of top border lines that are not visible
        /// when rendering the screen.
        /// </summary>
        public int NonVisibleBorderTopLines { get; set; }

        /// <summary>
        /// The number of border lines before the display.
        /// </summary>
        public int BorderTopLines { get; set; }

        /// <summary>
        /// Number of display lines.
        /// </summary>
        public int DisplayLines { get; set; }

        /// <summary>
        /// The number of border lines after the display.
        /// </summary>
        public int BorderBottomLines { get; set; }

        /// <summary>
        /// The number of bottom border lines that are not visible
        /// when rendering the screen.
        /// </summary>
        public int NonVisibleBorderBottomLines { get; set; }

        /// <summary>
        /// Horizontal blanking time (HSync+blanking).
        /// Given in Z80 clock cycles.
        /// </summary>
        public int HorizontalBlankingTime { get; set; }

        /// <summary>
        /// The time of displaying left part of the border.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int BorderLeftTime { get; set; }

        /// <summary>
        /// The time of displaying a pixel row.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int DisplayLineTime { get; set; }

        /// <summary>
        /// The time of displaying right part of the border.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int BorderRightTime { get; set; }

        /// <summary>
        /// The time used to render the non-visible right part of the border.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int NonVisibleBorderRightTime { get; set; }

        /// <summary>
        /// The time the data of a particular pixel should be pre-fetched
        /// before displaying it.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int PixelDataPrefetchTime { get; set; }

        /// <summary>
        /// The time the data of a particular pixel attribute should be pre-fetched
        /// before displaying it.
        /// Given in Z80 clock cycles.
        /// </summary>
        public int AttributeDataPrefetchTime { get; set; }

        /// <summary>
        /// Returns a clone of this instance.
        /// </summary>
        /// <returns>A clone of this instance.</returns>
        public ScreenConfigurationData Clone()
        {
            return new ScreenConfigurationData
            {
                InterruptTact = InterruptTact,
                BorderLeftTime = BorderLeftTime,
                DisplayLineTime = DisplayLineTime,
                BorderRightTime = BorderRightTime,
                AttributeDataPrefetchTime = AttributeDataPrefetchTime,
                BorderBottomLines = BorderBottomLines,
                BorderTopLines = BorderTopLines,
                DisplayLines = DisplayLines,
                HorizontalBlankingTime = HorizontalBlankingTime,
                NonVisibleBorderBottomLines = NonVisibleBorderBottomLines,
                NonVisibleBorderRightTime = NonVisibleBorderRightTime,
                NonVisibleBorderTopLines = NonVisibleBorderTopLines,
                PixelDataPrefetchTime = PixelDataPrefetchTime,
                VerticalSyncLines = VerticalSyncLines
            };
        }
    }
}