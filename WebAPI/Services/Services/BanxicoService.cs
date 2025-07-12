using System.Text.Json;

namespace WebAPI.Services.Services
{
    public class BanxicoService
    {
        private readonly IConfiguration _configuration;
        private readonly HttpClient _httpClient;

        public BanxicoService(IConfiguration configuration)
        {
            _configuration = configuration;
            _httpClient = new HttpClient();
        }

        public async Task<decimal> GetExchangeRate(string serieId)
        {
            var token = _configuration["Banxico:Token"];
            var url = $"https://www.banxico.org.mx/SieAPIRest/service/v1/series/{serieId}/datos/oportuno";

            var request = new HttpRequestMessage(HttpMethod.Get, url);
            request.Headers.Add("Bmx-Token", token);

            var response = await _httpClient.SendAsync(request);

            if (!response.IsSuccessStatusCode)
                throw new Exception("Error consultando Banxico");

            var json = await response.Content.ReadAsStringAsync();

            using var doc = JsonDocument.Parse(json);
            var root = doc.RootElement;

            if (!root.TryGetProperty("bmx", out var bmx))
                throw new Exception("No se encontró el objeto 'bmx' en la respuesta");

            if (!bmx.TryGetProperty("series", out var seriesArray))
                throw new Exception("No se encontró el array 'series' en la respuesta");

            if (seriesArray.GetArrayLength() == 0)
                throw new Exception("No hay series en la respuesta");

            var series = seriesArray[0];

            if (!series.TryGetProperty("datos", out var datosArray))
                throw new Exception("No se encontró el array 'datos' en la respuesta");

            if (datosArray.GetArrayLength() == 0)
                throw new Exception("La serie no tiene datos disponibles.");

            var datoStr = datosArray[0].GetProperty("dato").GetString();

            if (string.IsNullOrWhiteSpace(datoStr))
                throw new Exception("Dato vacío o nulo en la respuesta");

            return decimal.Parse(datoStr);
        }
    }
}
