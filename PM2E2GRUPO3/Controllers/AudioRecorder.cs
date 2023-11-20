//Grabar audio:=========================================================================================================
//https://www.google.com/search?q=.net+maui+record+audio&oq=.net+maui+record+audio&gs_lcrp=EgZjaHJvbWUyBggAEEUYOTIGCAEQLhhA0gEINTMxOWowajSoAgCwAgA&sourceid=chrome&ie=UTF-8#fpstate=ive&vld=cid:d924060a,vid:KaHyRSy5sAs,st:0

using Plugin.Maui.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Controllers {
    public class AudioRecorder {
        private List<string> errorString = new List<string>();
        private IAudioRecorder audioRecorder;
        private PermissionStatus permiso;
        private byte[] audioArray;


        public AudioRecorder() {
            ValidarPermiso();

            //objeto grabador de audio
            audioRecorder = AudioManager.Current.CreateRecorder();
        }




        public List<string> GetErrors() {
            return this.errorString;
        }


        public bool IsRecording() {
            return audioRecorder.IsRecording;
        }


        public void Detener() {
            if(audioRecorder.IsRecording) {
                audioRecorder.StopAsync();
            } 
        }


        public async Task<byte[]> Grabar_o_Detener() {
            errorString = new List<string>();

            if (permiso == PermissionStatus.Granted) {
                if (!audioRecorder.IsRecording) {
                    await audioRecorder.StartAsync();
                    return new byte[0];

                } else {
                    var audio = await audioRecorder.StopAsync();
                    using (MemoryStream ms = new MemoryStream()) {
                        Stream st = audio.GetAudioStream();
                        await st.CopyToAsync(ms);
                        audioArray = ms.ToArray();
                    }
                    return audioArray;
                }
            } else {
                errorString.Add("No se otorgaron permisos de microfono");
                return new byte[0];
            }
        }






        private async void ValidarPermiso() {
            permiso = await Permissions.CheckStatusAsync<Permissions.Microphone>();

            if (permiso == PermissionStatus.Granted) {
                return;

            } else {
                permiso = await Permissions.RequestAsync<Permissions.Microphone>();
            }
        }





    }
}
