using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using CentroUnas.Web.Core;
using CentroUnas.Web.Dtos;

namespace CentroUnas.Web.Services
{
    public class ApiService
    {
        private readonly HttpClient _http;
        private readonly JsonSerializerOptions _opciones;

        public ApiService(HttpClient http)
        {
            _http = http;
            _opciones = new JsonSerializerOptions(JsonSerializerDefaults.Web);
            _opciones.Converters.Add(new JsonStringEnumConverter());
        }

        public async Task<ApiResult<T>> GetAsync<T>(string ruta)
        {
            return await EnviarAsync<T>(() => _http.GetAsync(ruta));
        }

        public async Task<ApiResult<T>> GetAsync<T>(string ruta, int id)
        {
            return await GetAsync<T>($"{ruta}/{id}");
        }

        public async Task<List<T>> GetListAsync<T>(string ruta)
        {
            var result = await GetAsync<List<T>>(ruta);
            return result.Data ?? new List<T>();
        }

        public async Task<ApiResult<T>> PostAsync<T>(string ruta, object datos)
        {
            return await EnviarAsync<T>(() => _http.PostAsJsonAsync(ruta, datos, _opciones));
        }

        public async Task<ApiResult<bool>> PutAsync(string ruta, object datos)
        {
            return await EnviarAsync<bool>(() => _http.PutAsJsonAsync(ruta, datos, _opciones));
        }

        public async Task<ApiResult<bool>> PutAsync(string ruta, int id, object datos)
        {
            return await PutAsync($"{ruta}/{id}", datos);
        }

        public async Task<ApiResult<bool>> GuardarAsync(string ruta, DtoBase dto)
        {
            if (dto.Id != 0)
            {
                return await PutAsync(ruta, dto.Id, dto);
            }

            var result = await PostAsync<JsonElement>(ruta, dto);
            return result.Success
                ? ApiResult<bool>.Ok(true)
                : ApiResult<bool>.Fail(result.Message ?? "No se pudo guardar");
        }

        public async Task<ApiResult<bool>> DeleteAsync(string ruta, int id)
        {
            return await EnviarAsync<bool>(() => _http.DeleteAsync($"{ruta}/{id}"));
        }

        private async Task<ApiResult<T>> EnviarAsync<T>(Func<Task<HttpResponseMessage>> peticion)
        {
            try
            {
                var respuesta = await peticion();

                if (!respuesta.IsSuccessStatusCode)
                {
                    return ApiResult<T>.Fail(await LeerErrorAsync(respuesta));
                }

                if (respuesta.StatusCode == HttpStatusCode.NoContent)
                {
                    return ApiResult<T>.Ok(default);
                }

                var datos = await respuesta.Content.ReadFromJsonAsync<T>(_opciones);
                return ApiResult<T>.Ok(datos);
            }
            catch (HttpRequestException)
            {
                return ApiResult<T>.Fail("No se pudo conectar con la API. Verifica que esté en ejecución.");
            }
        }

        private static async Task<string> LeerErrorAsync(HttpResponseMessage respuesta)
        {
            var contenido = await respuesta.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(contenido))
            {
                return $"La API respondió con el error {(int)respuesta.StatusCode}";
            }

            try
            {
                using var documento = JsonDocument.Parse(contenido);
                var raiz = documento.RootElement;

                if (raiz.ValueKind == JsonValueKind.String)
                {
                    return raiz.GetString() ?? contenido;
                }

                if (raiz.TryGetProperty("errors", out var errores))
                {
                    var mensajes = errores.EnumerateObject()
                        .SelectMany(e => e.Value.EnumerateArray().Select(m => m.GetString()));
                    return string.Join(" ", mensajes);
                }

                if (raiz.TryGetProperty("title", out var titulo))
                {
                    return titulo.GetString() ?? contenido;
                }

                return contenido;
            }
            catch (JsonException)
            {
                return contenido;
            }
        }
    }
}
