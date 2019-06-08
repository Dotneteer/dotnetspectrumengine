using System;

namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// This class represents the theming service
    /// </summary>
    public interface IThemingService
    {
        /// <summary>
        /// Registers the theme
        /// </summary>
        /// <param name="theme">Theme to register</param>
        void RegisterTheme(ThemeInfo theme);

        /// <summary>
        /// Sets the theme to the specified one
        /// </summary>
        /// <param name="name">Theme name</param>
        void SetTheme(string name);

        /// <summary>
        /// Gets the active theme
        /// </summary>
        /// <returns>Active theme information</returns>
        ThemeInfo GetActiveTheme();

        /// <summary>
        /// Gets the specified property
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="propName">Property name</param>
        /// <returns>Property value</returns>
        TProp GetProperty<TProp>(string propName);

        /// <summary>
        /// Gets the style attribute to be used with the current theme
        /// </summary>
        /// <returns>Value of the style attribute</returns>
        string ComposeStyleAttributeFromTheme();

        /// <summary>
        /// This event is raised when the active theme changes.
        /// </summary>
        event EventHandler ThemeChanged;

        /// <summary>
        /// Gets the icon with the specified name
        /// </summary>
        /// <param name="iconName">Name of the icon</param>
        /// <returns>Icon information</returns>
        IconInfo GetIcon(string iconName);
    }
}
