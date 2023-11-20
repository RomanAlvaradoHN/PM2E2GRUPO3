using Microsoft.Maui.Controls.Maps;
using Microsoft.Maui.Maps;
using PM2E2GRUPO3.Models;

namespace PM2E2GRUPO3.Views;

public partial class MapaView : ContentPage
{
	private Sitios sitio;

	public MapaView(Sitios sitio){
		InitializeComponent();
		this.sitio = sitio;
	}


    protected override void OnAppearing() {
        base.OnAppearing();

        //Location locacion = new Location(pais.latlng[0], pais.latlng[1]);

        //mapa.Pins.Add(new Pin {
        //    Label = pais.name.official,
        //    Address = "Area: " + pais.area + " km^2",
        //    Location = locacion,
        //    Type = PinType.Place
        //}); ;

        ////mapa.MapType = MapType.Satellite;
        //mapa.MoveToRegion(new MapSpan(locacion, 0.1, 0.1));
    }
}