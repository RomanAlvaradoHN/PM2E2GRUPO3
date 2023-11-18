using CommunityToolkit.Maui.Views;
using Plugin.Maui.Audio;
using PM2E2GRUPO3.Models;

namespace PM2E2GRUPO3 {
    public partial class MainPage : ContentPage 
    {
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;
        private readonly IAudioRecorder audioRecorder;
        private PermissionStatus permisoMicrofono;
        private PermissionStatus permisoGPS;

        private Location locacion = new Location();
        private byte[] firmaImageArray;
        private byte[] videoArray;
        private byte[] audioArray;

        public MainPage(IAudioManager audioManager) {
            InitializeComponent();
            audioRecorder = audioManager.CreateRecorder();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            ValidarPermisos();
        }

        /*
         <Frame VerticalOptions="FillAndExpand" Padding="0">
                <toolkit:DrawingView x:Name="signPad"
                DrawingLineCompleted="OnPadSignedEvent"
                BackgroundColor="white"
                IsMultiLineModeEnabled="True"
                LineColor="BlueViolet"
                LineWidth="5"/>
         </Frame>
     
     
        public async void OnPadSignedEvent(object sender, EventArgs args) {
            //Guardar el byte[] de la imagen de la firma:
            using (MemoryStream ms = new MemoryStream()) {
                Stream st = await signPad.GetImageStream(signPad.Width, signPad.Height);
                await st.CopyToAsync(ms);
                firmaImageArray = ms.ToArray();
            }

            await ObtenerCoordenadas();
        }
        */

        public async void OnBtnVideoClicked(object sender, EventArgs args)
        {
            await TakeVideo();
            await ObtenerCoordenadas();
        }

        public async Task TakeVideo()
        {
            if (MediaPicker.Default.IsCaptureSupported)
            {
                FileResult video = await MediaPicker.Default.CaptureVideoAsync();

                if (video != null)
                {
                    try
                    {
                        using (MemoryStream ms = new MemoryStream())
                        {
                            Stream st = await video.OpenReadAsync();
                            await st.CopyToAsync(ms);
                            videoArray = ms.ToArray();
                        }

                        // save the file into local storage
                        string localFilePath = Path.Combine(FileSystem.CacheDirectory, video.FileName);
                        using (FileStream videoFile = File.OpenWrite(localFilePath))
                        {
                            Stream st = new MemoryStream(videoArray);
                            await st.CopyToAsync(videoFile);
                        }
                        videoElement.Source = MediaSource.FromFile(localFilePath);
                    }
                    catch (Exception ex)
                    {
                        await DisplayAlert("¡Atención!", ex.Message, "Aceptar");
                    }
                }
            }
        }

        //Geolocacion=========================================================================================================
        //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?view=net-maui-7.0&tabs=android
        private async Task ObtenerCoordenadas()
        {
            //if (permisoGPS == PermissionStatus.Granted) {
            try
            {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(5));
                _cancelTokenSource = new CancellationTokenSource();

                locacion = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                if (locacion != null)
                {
                    txtLatitud.Text = $"{locacion.Latitude}";
                    txtLongitud.Text = $"{locacion.Longitude}";
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("¡Atención!", ex.Message, "Aceptar");
            }
            finally
            {
                _isCheckingLocation = false;
            }
            //} else {
            //    bool resp = await DisplayAlert("Datos", "No se concedieron permisos para usar GPS, desea otorgarlos?", "Si", "No");
            //    if (resp) { ValidarPermisos(); }
            //}
        }

        public void CancelRequest()
        {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false)
            {
                _cancelTokenSource.Cancel();
            }
        }

        //Grabar audio:=========================================================================================================
        //https://www.google.com/search?q=.net+maui+record+audio&oq=.net+maui+record+audio&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQLhhA0gEINTMxOWowajSoAgCwAgA&sourceid=chrome&ie=UTF-8#fpstate=ive&vld=cid:d924060a,vid:KaHyRSy5sAs,st:0
        private async void OnBtnStartRecordingClicked(object sender, EventArgs args)
        {
            if (permisoMicrofono == PermissionStatus.Granted)
            {
                if (!audioRecorder.IsRecording)
                {
                    await audioRecorder.StartAsync();
                    SetButtonRecordingStyle();
                }
                else
                {
                    var audio = await audioRecorder.StopAsync();
                    using (MemoryStream ms = new MemoryStream())
                    {
                        Stream st = audio.GetAudioStream();
                        await st.CopyToAsync(ms);
                        audioArray = ms.ToArray();
                    }
                    SetButtonRecordedStyle();
                }
            }
            else
            {
                bool resp = await DisplayAlert("¡Atención!", "No se concedieron permisos para grabar, desea otorgarlos?", "Sí", "No");
                if (resp) { ValidarPermisos(); }
            }
        }

        public async void OnBtnGuardarClicked(object sender, EventArgs args)
        {
            try
            {
                //Datos datos = new Datos(
                //    firmaImageArray,
                //    audioArray,
                //    locacion.Latitude,
                //    locacion.Longitude
                //);

                Sitios datos = new Sitios(
                    videoArray,
                    audioArray,
                    locacion.Latitude,
                    locacion.Longitude
                );
                if (!datos.GetDatosInvalidos().Any())
                {
                    Console.WriteLine("#############################");
                    Console.WriteLine("Guardando datos");
                    Console.WriteLine("#############################");

                    await DisplayAlert("Aviso", "¡Los datos se han guardado exitosamente!", "Aceptar");
                    LimpiarCampos();
                }
                else
                {
                    string msj = string.Join("\n", datos.GetDatosInvalidos());
                    await DisplayAlert("¡Alerta!", msj, "Aceptar");
                }
            }
            catch (Exception ex)
            {
                await DisplayAlert("¡Guardar!", ex.Message, "Aceptar");
            }
        }

        public async void OnBtnListaClicked(object sender, EventArgs args)
        {
            //await Navigation.PushAsync(new Listado());
        }

        private async void ValidarPermisos()
        {
            //Validar permisos para usar microfono
            permisoMicrofono = await Permissions.CheckStatusAsync<Permissions.Microphone>();
            if (permisoMicrofono == PermissionStatus.Granted)
            {
                return;
            }
            else
            {
                permisoMicrofono = await Permissions.RequestAsync<Permissions.Microphone>();
            }
            //Validar permisos para usar gps
            //permisoGPS = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();
            //if (permisoGPS == PermissionStatus.Granted) {
            //    return;
            //} else {
            //    permisoGPS = await Permissions.RequestAsync<Permissions.LocationAlways>();
            //}
        }

        public void OnBtnLimpiarClicked(object sender, EventArgs args)
        {
            LimpiarCampos();
        }

        private void SetButtonNormalStyle()
        {
            btnBtnStartRecording.BackgroundColor = Color.FromHex("#8bc34a");
            btnBtnStartRecording.BorderColor = Color.FromHex("#8bc34a");
            btnBtnStartRecording.ImageSource = new FileImageSource().File = "microfonoa.png";
            //btnBtnStartRecording.Text = "Grabar";
        }

        private void SetButtonRecordingStyle()
        {
            btnBtnStartRecording.BackgroundColor = Color.FromHex("#F0394D");
            btnBtnStartRecording.BorderColor = Color.FromHex("#F0394D");
            btnBtnStartRecording.ImageSource = new FileImageSource().File = "microfonoam.png";
            //btnBtnStartRecording.Text = "Detener";
        }

        private void SetButtonRecordedStyle()
        {
            btnBtnStartRecording.BackgroundColor = Color.FromHex("#8bc34a");
            btnBtnStartRecording.BorderColor = Color.FromHex("#8bc34a");
            btnBtnStartRecording.ImageSource = new FileImageSource().File = "donea.png";
            //btnBtnStartRecording.Text = "Detener";
        }

        public void ContentPage_Unloaded(object? sender, EventArgs e)
        {
            videoElement.Handler?.DisconnectHandler();
        }

        private async void LimpiarCampos()
        {
            //firmaImageArray = new byte[0];
            videoArray = new byte[0];
            audioArray = new byte[0];
            //signPad.Clear();

            videoElement.Source = null;

            if (audioRecorder.IsRecording)
            {
                await audioRecorder.StopAsync();
            }
            SetButtonNormalStyle();

            txtLatitud.Text = string.Empty;
            txtLongitud.Text = string.Empty;
        }
    }
}