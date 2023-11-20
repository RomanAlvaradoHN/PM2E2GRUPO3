using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using Microsoft.Extensions.Logging;

namespace PM2E2GRUPO3 {
    public static class MauiProgram {
        public static MauiApp CreateMauiApp() {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts => {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
            builder.UseMauiMaps();
            builder.UseMauiCommunityToolkit();
            builder.UseMauiCommunityToolkitMediaElement();

            builder.Services.AddSingleton(AudioManager.Current);
            //builder.Services.AddTransient<Views.CapturaDatos>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}