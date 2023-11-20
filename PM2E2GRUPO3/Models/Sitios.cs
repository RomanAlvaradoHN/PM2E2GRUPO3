using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Models
{
    public class Sitios{
        private List<string> invalidData = new List<string>();
        private byte[] video;
        private byte[] audio;
        //private string video;
        //private string audio;
        private double latitud;
        private double longitud;


        public Sitios() { }


        public Sitios(byte[] video, byte[] audio, double latitud, double longitud) {
            this.Video = video;
            this.Audio = audio;
            this.Latitud = latitud;
            this.Longitud = longitud;
            this.Descripcion = DateTime.Now.ToString("yyyyMMddHHmmss");
        }

        public List<string> GetDatosInvalidos() {
            return this.invalidData;
        }






        public int Id {  get; set; }

        public string Descripcion {  get; set; }


        public byte[] Video {
            get { return this.video; }

            set {
                if (value != null && value.Length > 0) {
                    this.video = value;
                } else {
                    this.invalidData.Add("No hay grabación de vídeo.");
                }
            }
        }

        public byte[] Audio {
            get { return this.audio; }

            set {
                if (value != null && value.Length > 0) {
                    this.audio = value;
                } else {
                    this.invalidData.Add("No hay grabación de audio.");
                }
            }
        }

        //public string Video { get; set; }

        //public string Audio { get; set; }

        public double Latitud {
            get { return this.latitud; }

            set {
                if (value != 0.0) {
                    this.latitud = value;
                } else {
                    this.invalidData.Add("No se generó valor de latitud.");
                }
            }
        }

        public double Longitud {
            get { return this.longitud; }

            set {
                if (value != 0.0) {
                    this.longitud = value;
                } else {
                    this.invalidData.Add("No se generó valor de longitud.");
                }
            }
        }
    }
}
