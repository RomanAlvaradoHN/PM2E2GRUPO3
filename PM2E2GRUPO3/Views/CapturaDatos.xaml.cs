using Microsoft.Maui.Controls.Compatibility;
using Microsoft.Maui.Devices.Sensors;
using Plugin.Maui.Audio;

namespace PM2E2GRUPO3.Views;

public partial class CapturaDatos : ContentPage
{
    private CancellationTokenSource _cancelTokenSource;
    private bool _isCheckingLocation;
    private readonly IAudioRecorder audioRecorder;
    private PermissionStatus permisoMicrofono;
    private PermissionStatus permisoGPS;


    private Location locacion = new Location();
    private byte[] firmaImageArray;
    private byte[] audioArray;
    
    
    
    





    public CapturaDatos(IAudioManager audioManager){
		InitializeComponent();
        audioRecorder = audioManager.CreateRecorder();
	}







    protected override async void OnAppearing() {
        base.OnAppearing();

        //Validar permisos para usar microfono
        permisoMicrofono = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (permisoMicrofono == PermissionStatus.Granted) {
            return;
        } else {
            permisoMicrofono = await Permissions.RequestAsync<Permissions.Microphone>();
        }

        //Validar permisos para usar gps
        permisoGPS = await Permissions.CheckStatusAsync<Permissions.Microphone>();
        if (permisoGPS == PermissionStatus.Granted) {
            return;
        } else {
            permisoGPS = await Permissions.RequestAsync<Permissions.Microphone>();
        }
    }










    public async void OnPadSignedEvent(object sender, EventArgs args) {
        //Guardar el byte[] de la imagen de la firma:
        using (MemoryStream ms = new MemoryStream()) {
            Stream st = await signPad.GetImageStream(signPad.Width, signPad.Height);
            await st.CopyToAsync(ms);
            firmaImageArray = ms.ToArray();
        }

        await ObtenerCoordenadas();
    }








    //Geolocacion=========================================================================================================
    //https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?view=net-maui-7.0&tabs=android
    private async Task ObtenerCoordenadas() {
        try {
            _isCheckingLocation = true;
            GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(5));
            _cancelTokenSource = new CancellationTokenSource();

            locacion = await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
            if (locacion != null) {
                txtLatitud.Text = $"{locacion.Latitude}";
                txtLongitud.Text = $"{locacion.Longitude}";
            }

        }

        catch (Exception ex) {
            await DisplayAlert("Atencion", ex.Message, "Aceptar");

        } finally {
            _isCheckingLocation = false;
        }
    }
    public void CancelRequest() {
        if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false) {
            _cancelTokenSource.Cancel();
        }
    }









    //Grabar audio:=========================================================================================================
    //https://www.google.com/search?q=.net+maui+record+audio&oq=.net+maui+record+audio&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQLhhA0gEINTMxOWowajSoAgCwAgA&sourceid=chrome&ie=UTF-8#fpstate=ive&vld=cid:d924060a,vid:KaHyRSy5sAs,st:0
    private async void OnBtnStartRecordingClicked(object sender, EventArgs args) {
        if (permisoMicrofono == PermissionStatus.Granted) {
            if (!audioRecorder.IsRecording) {
                await audioRecorder.StartAsync();
                SetButtonRecordingStyle();

            } else {
                var audio = await audioRecorder.StopAsync();
                using (MemoryStream ms = new MemoryStream()) {
                    Stream st = audio.GetAudioStream();
                    await st.CopyToAsync(ms);
                    audioArray = ms.ToArray();
                }

                SetButtonRecordedStyle();
            }
        } else {
            await DisplayAlert("Grabar", "No se concedieron permisos para grabar", "Aceptar");
        }
    }












    public async void OnBtnListaClicked(object sender, EventArgs args) {
        //await Navigation.PushAsync(new Listado());
    }








    public void OnBtnLimpiarClicked(object sender, EventArgs args) {
        LimpiarCampos();
    }


    private void SetButtonNormalStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.Black;
        btnBtnStartRecording.BorderColor = Colors.YellowGreen;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "microphone_ico.png";
        //btnBtnStartRecording.Text = "Grabar";
    }

    private void SetButtonRecordingStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.Red;
        btnBtnStartRecording.BorderColor = Colors.Red;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "stop_ico.png";
        //btnBtnStartRecording.Text = "Detener";
    }

    private void SetButtonRecordedStyle() {
        btnBtnStartRecording.BackgroundColor = Colors.DeepSkyBlue;
        btnBtnStartRecording.BorderColor = Colors.Cyan;
        btnBtnStartRecording.ImageSource = new FileImageSource().File = "done_ico.png";
        //btnBtnStartRecording.Text = "Detener";
    }



    private async void LimpiarCampos() {
        firmaImageArray = new byte[0];
        audioArray = new byte[0];
        signPad.Clear();
        if (audioRecorder.IsRecording) {
            await audioRecorder.StopAsync();
        }
        SetButtonNormalStyle();
        
        txtLatitud.Text = string.Empty;
        txtLongitud.Text = string.Empty;
    }
}