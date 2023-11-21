//Geolocacion=========================================================================================================
//https://learn.microsoft.com/en-us/dotnet/maui/platform-integration/device/geolocation?view=net-maui-7.0&tabs=android

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Controllers
{
    public class LocationRecorder{
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        
        public LocationRecorder() {}




        //Obtiene las coordenadas ===================================================================================
        public async Task<Location> GetLocacion() {
            try {
                if (await PermisoLocacion()) {
                    _isCheckingLocation = true;
                    GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(5));
                    _cancelTokenSource = new CancellationTokenSource();

                    return await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);
                
                } else {
                    Console.WriteLine("No hay permisos de locacion");
                    return new Location();
                }

                


            } catch (Exception ex) {
                Console.WriteLine(ex.Message);
                return new Location();

            } finally {
                    _isCheckingLocation = false;
            }
        }

        public void CancelRequest() {
            if (_isCheckingLocation && _cancelTokenSource != null && _cancelTokenSource.IsCancellationRequested == false) {
                _cancelTokenSource.Cancel();
            }
        }









        private async Task<bool> PermisoLocacion() {
            PermissionStatus locationAlways = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            PermissionStatus locationWhenIUse = await Permissions.CheckStatusAsync<Permissions.LocationWhenInUse>();

            if (locationAlways != PermissionStatus.Granted) {
                locationAlways = await Permissions.RequestAsync<Permissions.LocationAlways>();
            }

            if (locationWhenIUse != PermissionStatus.Granted) {
                locationWhenIUse = await Permissions.RequestAsync<Permissions.LocationWhenInUse>();
            }

            if(locationAlways == PermissionStatus.Granted && locationWhenIUse == PermissionStatus.Granted) {
                return true;
            } else {
                return false;
            }
        }


    }
}
