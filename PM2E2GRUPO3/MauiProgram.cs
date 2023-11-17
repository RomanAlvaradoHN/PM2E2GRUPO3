using CommunityToolkit.Maui;
using Plugin.Maui.Audio;
using Microsoft.Extensions.Logging;
using PM2E2GRUPO3.Views;

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

            builder.UseMauiCommunityToolkit();
            builder.UseMauiCommunityToolkitMediaElement();

            builder.Services.AddSingleton(AudioManager.Current);
            builder.Services.AddTransient<CapturaDatos>();

#if DEBUG
        builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}