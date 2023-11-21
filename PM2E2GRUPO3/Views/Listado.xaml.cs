using PM2E2GRUPO3.Controllers;
using PM2E2GRUPO3.Models;
using System.Data;
using System.Windows.Input;

namespace PM2E2GRUPO3.Views;

public partial class Listado : ContentPage {
    Api api;




    public Listado() {
        InitializeComponent();
        api = new Api();
    }







    protected async override void OnAppearing() {
        base.OnAppearing();
        viewListado.ItemsSource = await api.SelectAll();
    }



    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args) {
        Sitios sitio = args.SelectedItem as Sitios;
        await Navigation.PushAsync(new PlayMedia(sitio));
    }
}