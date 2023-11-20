using CommunityToolkit.Maui.Views;
using PM2E2GRUPO3.Models;
using PM2E2GRUPO3.Controllers;
using PM2E2GRUPO3.Views;


namespace PM2E2GRUPO3 {
    public partial class MainPage : ContentPage 
    {
        private Location locacion = new Location();
        private AudioRecorder ar = new AudioRecorder();
        private byte[] videoArray = new byte[0];
        private byte[] audioArray = new byte[0];






        public MainPage() {
            InitializeComponent();
        }




        protected override void OnDisappearing() {
            base.OnDisappearing();
            videoElement.Handler?.DisconnectHandler();
        }





        public void OnBtnVideoClicked(object sender, EventArgs args){
            GrabarVideo();
            MostrarCoordenadas();
        }






        private async void GrabarVideo() {
            VideoRecorder vr = new VideoRecorder();
            this.videoArray = await vr.GrabarVideo();
            videoElement.Source = MediaSource.FromFile(vr.GetLocalFilePath());
        }



        private async void MostrarCoordenadas() {
            LocationRecorder lr = new LocationRecorder();
            this.locacion = await lr.GetLocacion();
            txtLatitud.Text = $"{this.locacion.Latitude}";
            txtLongitud.Text = $"{this.locacion.Longitude}";
        }





        private async void OnBtnStartRecordingClicked(object sender, EventArgs args) {
            audioArray = await ar.Grabar_o_Detener();

            if (!ar.GetErrors().Any()) {
                if (ar.IsRecording()) {
                    SetButtonStyle(Color.FromArgb("#F0394D"), "stop_ico.png");

                } else {
                    SetButtonStyle(Color.FromArgb("#8BC34A"), "done_ico.png");
                }
            
            } else {
                string msj = string.Join("\n", ar.GetErrors());
                await DisplayAlert("Atencion:", msj, "Aceptar");
            }
        }






        





        






        //Proceso de guardado: ======================================================================================================
        public async void OnBtnGuardarClicked(object sender, EventArgs args){
            try{
                Sitios datos = new Sitios(
                    videoArray,
                    audioArray,
                    locacion.Latitude,
                    locacion.Longitude
                );

                if (!datos.GetDatosInvalidos().Any()){
                    await DisplayAlert("Guardar", "Datos guardados!", "Aceptar");
                    LimpiarCampos();
                
                }else {
                    string msj = string.Join("\n", datos.GetDatosInvalidos());
                    await DisplayAlert("Atencion:", msj, "Aceptar");
                }
            
            } catch (Exception ex) {
                await DisplayAlert("Guardar", ex.Message, "Aceptar");
            }
        }








        public async void OnBtnListaClicked(object sender, EventArgs args){
            await Navigation.PushAsync(new Listado());
        }




        




        private void SetButtonStyle(Color color, string imageName) {
            btnBtnStartRecording.BackgroundColor = color;
            btnBtnStartRecording.ImageSource = new FileImageSource().File = imageName;
        }

        public void OnBtnLimpiarClicked(object sender, EventArgs args) {
            LimpiarCampos();
        }

        private void LimpiarCampos(){
            locacion = new Location();
            videoArray = new byte[0];
            audioArray = new byte[0];
            videoElement.Source = null;
            txtLatitud.Text = string.Empty;
            txtLongitud.Text = string.Empty;
            ar.Detener();
            SetButtonStyle(Color.FromArgb("#8BC34A"), "microphone_ico.png");
        }
    }
}