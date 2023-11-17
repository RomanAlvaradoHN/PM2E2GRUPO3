using Plugin.Maui.Audio;

namespace PM2E2GRUPO3 {
    public partial class App : Application {
        public App() {
            InitializeComponent();

            //MainPage = new AppShell();
            MainPage = new NavigationPage(new Views.CapturaDatos(AudioManager.Current));
        }
    }
}