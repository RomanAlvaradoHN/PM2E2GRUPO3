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
        private PermissionStatus permiso;
        private CancellationTokenSource _cancelTokenSource;
        private bool _isCheckingLocation;

        public LocationRecorder() {
        }





        //Obtiene las coordenadas ===================================================================================
        public async Task<Location> GetLocacion() {
            try {
                _isCheckingLocation = true;
                GeolocationRequest request = new GeolocationRequest(GeolocationAccuracy.Low, TimeSpan.FromSeconds(5));
                _cancelTokenSource = new CancellationTokenSource();

                return await Geolocation.Default.GetLocationAsync(request, _cancelTokenSource.Token);


            } catch (Exception ex) {
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









        private async void ValidarPermisoLocacion() {
            this.permiso = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();

            if (this.permiso == PermissionStatus.Granted) {
                return;

            } else {
                this.permiso = await Permissions.CheckStatusAsync<Permissions.LocationAlways>();
            }
        }


    }
}
