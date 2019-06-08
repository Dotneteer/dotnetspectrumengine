using DotnetSpectrumEngine.Core;
using DotnetSpectrumEngine.Core.Abstraction;
using DotnetSpectrumEngine.Core.Machine;
using DotnetSpectrumEngine.SampleUi.Blazor.Client.Themes;
using Microsoft.AspNetCore.Components.Builder;
using Microsoft.Extensions.DependencyInjection;

namespace DotnetSpectrumEngine.SampleUi.Blazor.Client
{
    public class Startup
    {
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IThemingService, ThemingService>();
            var machine = SpectrumMachine.CreateMachine(SpectrumModels.ZX_SPECTRUM_48, SpectrumModels.PAL);
            services.AddSingleton<ISpectrumMachine>(machine);
        }
        
        public void Configure(IComponentsApplicationBuilder app)
        {
            app.AddComponent<App>("app");
        }
    }
}
