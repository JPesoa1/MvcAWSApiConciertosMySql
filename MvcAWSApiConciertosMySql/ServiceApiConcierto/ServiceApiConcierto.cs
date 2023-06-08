using MvcAWSApiConciertosMySql.Helpers;
using MvcAWSApiConciertosMySql.Models;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using System.Text;

namespace MvcAWSApiConciertosMySql.ServiceApiConcierto
{
    public class ServiceApiConcierto
    {
        private MediaTypeWithQualityHeaderValue Header;
        private string UrlApi;
       





        public ServiceApiConcierto(IConfiguration configuration, KeysModel model)
        {
            this.Header =
                new MediaTypeWithQualityHeaderValue("application/json");


            
           
            this.UrlApi = model.ApiConcierto;
               
        }

        private async Task<T> CallApiAsync<T>(string request)
        {
            using (HttpClient client = new HttpClient())
            {
               
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);
                string url = this.UrlApi + request;
                HttpResponseMessage response =
                    await client.GetAsync(url);
                if (response.IsSuccessStatusCode)
                {
                    T data = await response.Content.ReadAsAsync<T>();
                    return data;
                }
                else
                {
                    return default(T);
                }
            }
        }



        public async Task CreateConcierto(string nombre, string artista, int categoria, string imagen)
        {
            using (HttpClient client = new HttpClient())
            {
                string request = "api/Concierto/InsertarEvento";
                client.DefaultRequestHeaders.Clear();
                client.DefaultRequestHeaders.Accept.Add(this.Header);


                Eventos eventos = new Eventos();
                eventos.IdEvento = 0;
                eventos.Nombre = nombre;
                eventos.Artista = artista;
                eventos.IdCategoria = categoria;
                eventos.Imagen = imagen;

                string jsonComic = JsonConvert.SerializeObject(eventos);
                StringContent content =
                    new StringContent(jsonComic, Encoding.UTF8, "application/json");
                HttpResponseMessage response =
                    await client.PostAsync(this.UrlApi + request, content);
            }
        }

        public async Task<List<CategoriaEvento>> GetCategoriasAsync()
        {
            string request = "api/Concierto/GetCategorias";
            List<CategoriaEvento> categoriaEventos = await this.CallApiAsync<List<CategoriaEvento>>(request);
            return categoriaEventos;
        }


        public async Task<List<Eventos>> GetEventos()
        {
            string request = "api/Concierto/GetEventos";
            List<Eventos> eventos = await this.CallApiAsync<List<Eventos>>(request);
            return eventos;
        }


        public async Task<List<Eventos>> GetEventosPorCategoria(int idcategoria)
        {
            string request = "api/Concierto/GetEventosPorCategoria/"+idcategoria;
            List<Eventos> eventos = await this.CallApiAsync<List<Eventos>>(request);
            return eventos;
        }


    }
}
