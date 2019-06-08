namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// This interface represent the theme properties
    /// </summary>
    public interface IThemeProperties
    {
        /// <summary>
        /// Background color of the shell
        /// </summary>
        string ShellBackgroundColor { get; }

        /// <summary>
        /// The default size of an SVG icon
        /// </summary>
        int DefaultSvgIconSize { get; }

        /// <summary>
        /// The default fill color of an SVG icon
        /// </summary>
        string DefaultSvgFill { get; }

        /// <summary>
        /// Fill color of an icon button's icon in disabled state
        /// </summary>
        string IconButtonDisabledFill { get; }

        /// <summary>
        /// The border color of an icon button
        /// </summary>
        string IconButtonBorder { get; }

        /// <summary>
        /// Border color of an icon button when it's hovered
        /// </summary>
        string IconButtonHoverBorderColor { get; }

        /// <summary>
        /// Background color of an icon button when it's hovered
        /// </summary>
        string IconButtonHoverBackgroundColor { get; }

        /// <summary>
        /// Icon button border when it's pressed
        /// </summary>
        string IconButtonPressedBorder { get; }

        /// <summary>
        /// Border color of a separator in the toolbar
        /// </summary>
        string SeparatorBorderColor { get; }
    }
}
