using PM2E2GRUPO3.Controllers;
using System.Data;

namespace PM2E2GRUPO3.Views;

public partial class Listado : ContentPage
{
    Api api;




	public Listado(){
		InitializeComponent();
        api = new Api();
    }







    protected async override void OnAppearing() {
        base.OnAppearing();
        viewListado.ItemsSource = await api.SelectAll();
    }



    private async void OnItemSelected(object sender, SelectedItemChangedEventArgs args) {
        //Pais p = args.SelectedItem as Pais;
        //await Navigation.PushAsync(new MapaView(p));
    }
}