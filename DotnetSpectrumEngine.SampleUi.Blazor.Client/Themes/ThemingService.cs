using System;
using System.Collections.Generic;
using System.Text;

namespace DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes
{
    /// <summary>
    /// This class provides an implementation for theming service
    /// </summary>
    public class ThemingService : IThemingService
    {
        private readonly Dictionary<string, IThemeProperties> _themes =
            new Dictionary<string, IThemeProperties>();
        private string _activeName;
        private IThemeProperties _activeTheme;

        /// <summary>
        /// Registers the theme
        /// </summary>
        /// <param name="theme">Theme to register</param>
        public void RegisterTheme(ThemeInfo theme)
            => _themes[theme.Name] = theme.Properties;

        /// <summary>
        /// Sets the theme to the specified one
        /// </summary>
        /// <param name="name">Theme name</param>
        public void SetTheme(string name)
        {
            if (name == _activeName) return;
            if (!_themes.TryGetValue(name, out var theme)) return;

            _activeName = name;
            _activeTheme = theme;
            OnThemeChanged();
        }

        /// <summary>
        /// This event is raised when the active theme changes.
        /// </summary>
        public event EventHandler ThemeChanged;

        /// <summary>
        /// Gets the icon with the specified name
        /// </summary>
        /// <param name="iconName">Name of the icon</param>
        /// <returns>Icon information</returns>
        public IconInfo GetIcon(string iconName) => IconDefs.GetIcon(iconName);

        /// <summary>
        /// Gets the active theme
        /// </summary>
        /// <returns>Active theme information</returns>
        public ThemeInfo GetActiveTheme()
            => new ThemeInfo
            {
                Name = _activeName,
                Properties = _activeTheme
            };

        /// <summary>
        /// Gets the specified property
        /// </summary>
        /// <typeparam name="TProp">Property type</typeparam>
        /// <param name="propName">Property name</param>
        /// <returns>Property value</returns>
        public TProp GetProperty<TProp>(string propName)
        {
            var propInfo = _activeTheme?.GetType().GetProperty(propName);
            return propInfo == null 
                ? default
                : (TProp)propInfo.GetValue(_activeTheme);
        }

        /// <summary>
        /// Gets the style attribute to be used with the current theme
        /// </summary>
        /// <returns>Value of the style attribute</returns>
        public string ComposeStyleAttributeFromTheme()
        {
            var sb = new StringBuilder(1024);
            foreach (var propInfo in _activeTheme.GetType().GetProperties())
            {
                sb.Append($"--{NameHelper.ToCssName(propInfo.Name)}:{propInfo.GetValue(_activeTheme)};");
            }
            return sb.ToString();
        }

        /// <summary>
        /// Invokes the ThemeChanged event
        /// </summary>
        protected virtual void OnThemeChanged()
        {
            ThemeChanged?.Invoke(this, EventArgs.Empty);
        }
    }
}
