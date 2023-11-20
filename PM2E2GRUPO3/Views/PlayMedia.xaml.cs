using Plugin.Maui.Audio;
using CommunityToolkit.Maui.Views;
using PM2E2GRUPO3.Models;
using PM2E2GRUPO3.Controllers;

namespace PM2E2GRUPO3.Views;

public partial class PlayMedia : ContentPage
{
	Sitios sitio;
    Api api;
    IAudioPlayer player;




    public PlayMedia(Sitios sitio){
		InitializeComponent();
		this.sitio = sitio;
        this.api = new Api();
	}




    protected async override void OnAppearing() {
        base.OnAppearing();

        // save the file into local storage
        string localFilePath = Path.Combine(FileSystem.CacheDirectory, sitio.Descripcion);
        using (FileStream videoFile = File.OpenWrite(localFilePath)) {
            Stream st = new MemoryStream(sitio.Video);
            await st.CopyToAsync(videoFile);
        }

        videoElement.Source = MediaSource.FromFile(localFilePath);
    }


    protected override void OnDisappearing() {
        base.OnDisappearing();
        if (player.IsPlaying) {
            player.Stop();
        }
    }



    private async void OnBtnPlayClicked(object sender, EventArgs args) {
        try {
            Stream stream = new MemoryStream(sitio.Audio);
            player = AudioManager.Current.CreatePlayer(stream);
            player.Play();

        } catch (Exception ex) {
            await DisplayAlert("Error", ex.Message, "Aceptar");
        }
    }




    private async void OnBtnMapaClicked(object sender, EventArgs args) {
        try {
            await Navigation.PushAsync(new MapaView(sitio));
        }catch(Exception ex) {
            Console.WriteLine("###################################\n" + "BOTON MAPA\n" +ex.Message + "\n###################################");
        }
    }


    private async void OnBtnEditarClicked(object sender, EventArgs args) {
        await DisplayAlert("Editar", "No nos alcanzó el tiempo", "Aceptar");
    }


    private async void OnBtnEliminarClicked(object sender, EventArgs args) {
        if(await api.Delete(sitio.Id)) {
            await DisplayAlert("Eliminar", "Registro eliminado.", "Aceptar");
            await Navigation.PopAsync();

        } else {
            await DisplayAlert("Eliminar", "Algo salió mal", "Aceptar");
        }
    }
}