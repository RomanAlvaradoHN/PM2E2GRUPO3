using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PM2E2GRUPO3.Models
{
    public class Datos{
        private List<string> invalidData = new List<string>();
        private byte[] firma;
        private byte[] audio;
        private double latitud;
        private double longitud;







        public byte[] Firma {
            get { return this.firma; }

            set {
                if (value != null && value.Length > 0) {
                    this.firma = value;
                } else {
                    this.invalidData.Add("No ha dibujado la firma");
                }
            }
        }





        public byte[] Audio {
            get { return this.audio; }

            set {
                if (value != null && value.Length > 0) {
                    this.audio = value;
                } else {
                    this.invalidData.Add("No hay grabacion de audio");
                }
            }
        }
















    }
}
