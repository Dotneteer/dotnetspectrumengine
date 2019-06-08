namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// This class represents the properties of a particular theme
    /// </summary>
    public class ThemeProperties : IThemeProperties
    {
        public string ShellBackgroundColor { get; set; }
        public int DefaultSvgIconSize { get; set; }
        public string DefaultSvgFill { get; set; }
        public string IconButtonDisabledFill { get; set; }
        public string IconButtonBorder { get; set; }
        public string IconButtonHoverBorderColor { get; set; }
        public string IconButtonHoverBackgroundColor { get; set; }
        public string IconButtonPressedBorder { get; set; }
        public string SeparatorBorderColor { get; set; }
    }
}