namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// This class contains default stock themes
    /// </summary>
    public class StockThemes
    {
        /// <summary>
        /// Dark theme
        /// </summary>
        public static IThemeProperties DarkTheme = new ThemeProperties
        {
            ShellBackgroundColor = "#202020",
            DefaultSvgFill = "white",
            DefaultSvgIconSize = 16,
            IconButtonDisabledFill = "#707070",
            IconButtonBorder = "1px solid #007acc",
            IconButtonHoverBackgroundColor = "#00aaff",
            IconButtonHoverBorderColor = "#00aaff",
            IconButtonPressedBorder = "1px solid #094771",
            SeparatorBorderColor = "#383838"
        };

        /// <summary>
        /// Light theme
        /// </summary>
        public static IThemeProperties LightTheme = new ThemeProperties
        {
            ShellBackgroundColor = "#e0e0e0",
            DefaultSvgFill = "black",
            DefaultSvgIconSize = 16,
            IconButtonDisabledFill = "#808080",
            IconButtonBorder = "1px solid #007acc",
            IconButtonHoverBackgroundColor = "#2477ce",
            IconButtonHoverBorderColor = "#2477ce",
            IconButtonPressedBorder = "1px solid #2477ce",
            SeparatorBorderColor = "#c0c0c0"
        };
    }
}
