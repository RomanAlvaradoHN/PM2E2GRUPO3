using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Models
{
    public class Datos{
        private List<string> invalidData = new List<string>();
        //private byte[] firma;
        private byte[] video;
        private byte[] audio;
        private double latitud;
        private double longitud;





        //public Datos(byte[] firma, byte[] audio, double latitud, double longitud) {
        public Datos(byte[] video, byte[] audio, double latitud, double longitud) {
            //this.Firma = firma;
            this.Video = video;
            this.Audio = audio;
            this.Latitud = latitud;
            this.Longitud = longitud;
        }



        public List<string> GetDatosInvalidos() {
            return this.invalidData;
        }












        //public byte[] Firma {
        //    get { return this.firma; }

        //    set {
        //        if (value != null && value.Length > 0) {
        //            this.firma = value;
        //        } else {
        //            this.invalidData.Add("No ha dibujado la firma.");
        //        }
        //    }
        //}




        public byte[] Video {
            get { return this.video; }

            set {
                if (value != null && value.Length > 0) {
                    this.video = value;
                } else {
                    this.invalidData.Add("No hay grabacion de video.");
                }
            }
        }








        public byte[] Audio {
            get { return this.audio; }

            set {
                if (value != null && value.Length > 0) {
                    this.audio = value;
                } else {
                    this.invalidData.Add("No hay grabacion de audio.");
                }
            }
        }









        public double Latitud {
            get { return this.latitud; }

            set {
                if (value != 0.0) {
                    this.latitud = value;
                } else {
                    this.invalidData.Add("No se genero valor de latitud.");
                }
            }
        }









        public double Longitud {
            get { return this.longitud; }

            set {
                if (value != 0.0) {
                    this.longitud = value;
                } else {
                    this.invalidData.Add("No se genero valor de longitud."); ;
                }
            }
        }


        

    }
}
