//Comunicacion con API ====================================================================================
//https://learn.microsoft.com/en-us/dotnet/maui/data-cloud/rest?view=net-maui-7.0

using PM2E2GRUPO3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;


namespace PM2E2GRUPO3.Controllers {
    public class Api {
        private HttpClient _client;
        private JsonSerializerOptions _serializerOptions;
        private List<Sitios> Items;


        public Api() {
            _client = new HttpClient();
            _serializerOptions = new JsonSerializerOptions {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
                WriteIndented = true
            };
        }








        public async Task<List<Sitios>> SelectAll() {
            Items = new List<Sitios>();

            Uri uri = new Uri("http://www.grupo3.somee.com/api/Persona");

            try {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode) {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Sitios>>(content, _serializerOptions);
                }

            } catch (Exception ex) {
                Console.WriteLine("#################################\n" + ex.Message + "\n#################################");
            }

            return Items;
        }







        public async Task<List<Sitios>> SelectById(int id) {
            Items = new List<Sitios>();

            Uri uri = new Uri("http://www.grupo3.somee.com/api/Persona/" + id.ToString());

            try {
                HttpResponseMessage response = await _client.GetAsync(uri);
                if (response.IsSuccessStatusCode) {
                    string content = await response.Content.ReadAsStringAsync();
                    Items = JsonSerializer.Deserialize<List<Sitios>>(content, _serializerOptions);
                }

            } catch (Exception ex) {
                Console.WriteLine("#################################\n" + ex.Message + "\n#################################");
            }

            return Items;
        }











        public async Task<bool> Insert(Sitios item) {
            Uri uri = new Uri("http://www.grupo3.somee.com/api/Persona");

            try {
                string json = JsonSerializer.Serialize<Sitios>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await _client.PostAsync(uri, content);

                if (response.IsSuccessStatusCode) {
                    Console.WriteLine("#################################\n" + "Datos Guardados" + "\n#################################");
                    return true;
                } else {
                    return false;
                }
                    
            } catch (Exception ex) {
                Console.WriteLine("#################################\n" + ex.Message + "\n#################################");
                return false;
            }
        }





        public async Task<bool> Update(Sitios item) {
            Uri uri = new Uri("http://www.grupo3.somee.com/api/Persona");

            try {
                string json = JsonSerializer.Serialize<Sitios>(item, _serializerOptions);
                StringContent content = new StringContent(json, Encoding.UTF8, "application/json");

                HttpResponseMessage response = null;
                response = await _client.PatchAsync(uri, content);

                if (response.IsSuccessStatusCode) {
                    Console.WriteLine("#################################\n" + "Datos Guardados" + "\n#################################");
                    return true;
                } else {
                    return false;
                }

            } catch (Exception ex) {
                Console.WriteLine("#################################\n" + ex.Message + "\n#################################");
                return false;
            }
        }






        public async Task<bool> Delete(int id) {
            Uri uri = new Uri("http://www.grupo3.somee.com/api/Persona/" + id.ToString());
            try {
                HttpResponseMessage response = await _client.DeleteAsync(uri);
                if (response.IsSuccessStatusCode) {
                    Console.WriteLine("#################################\n" + "Datos Guardados" + "\n#################################");
                    return true;
                } else {
                    return false;
                }
            } catch (Exception ex) {
                Console.WriteLine("#################################\n" + ex.Message + "\n#################################");
                return false;
            }
        }

    }
}
