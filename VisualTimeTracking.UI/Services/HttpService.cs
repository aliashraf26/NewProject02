using Microsoft.AspNetCore.Components;
using System.ComponentModel;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Net;
using System.Text.Json;
using System.Text;
using System.Text.Json.Serialization;

namespace VisualTimeTracking.UI.Services
{
    public interface IHttpService
    {
        Task<T> Get<T>(string uri);
        Task Post(string uri, object value);
        Task<T> Post<T>(string uri, object value);
        Task<T> PostFile<T>(string uri, MultipartFormDataContent value);
        Task<T> PutFile<T>(string uri, MultipartFormDataContent value);
        Task Put(string uri, object value);
        Task<T> Put<T>(string uri, object value);
        Task Delete(string uri);
        Task<T> Delete<T>(string uri);
    }

    public class HttpService : IHttpService
    {
        private HttpClient _httpClient;
        private NavigationManager _navigationManager;
        private ILocalStorageServices _localStorageService;
        private IConfiguration _configuration;

        public HttpService(
            HttpClient httpClient,
            NavigationManager navigationManager,
            ILocalStorageServices localStorageService,
            IConfiguration configuration
        )
        {
            _httpClient = httpClient;
            _navigationManager = navigationManager;
            _localStorageService = localStorageService;
            _configuration = configuration;
        }

        public async Task<T> Get<T>(string uri)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, uri);
            return await sendRequest<T>(request);
        }

        public async Task Post(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Post<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Post, uri, value);
            return await sendRequest<T>(request);
        }
        public async Task<T> PostFile<T>(string uri, MultipartFormDataContent value)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, uri);

            await addJwtHeader(request);

            using var response = await _httpClient.PostAsync(uri, value);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!_navigationManager.Uri.ToLower().Trim().Contains("login"))
                {
                    _navigationManager.NavigateTo("/Logout");
                }

                return default;
            }

            await handleErrors(response);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new StringConverter());
            var res = await response.Content.ReadFromJsonAsync<T>(options);
            return res;
        }

        public async Task<T> PutFile<T>(string uri, MultipartFormDataContent value)
        {
            var request = new HttpRequestMessage(HttpMethod.Put, uri);

            await addJwtHeader(request);

            using var response = await _httpClient.PutAsync(uri, value);

            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!_navigationManager.Uri.ToLower().Trim().Contains("login"))
                {
                    _navigationManager.NavigateTo("/Logout");
                }
                return default;
            }

            await handleErrors(response);
            var options = new JsonSerializerOptions();
            options.PropertyNameCaseInsensitive = true;
            options.Converters.Add(new StringConverter());
            var res = await response.Content.ReadFromJsonAsync<T>(options);
            return res;
        }

        public async Task Put(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            await sendRequest(request);
        }

        public async Task<T> Put<T>(string uri, object value)
        {
            var request = createRequest(HttpMethod.Put, uri, value);
            return await sendRequest<T>(request);
        }

        public async Task Delete(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            await sendRequest(request);
        }

        public async Task<T> Delete<T>(string uri)
        {
            var request = createRequest(HttpMethod.Delete, uri);
            return await sendRequest<T>(request);
        }

        // helper methods

        private HttpRequestMessage createRequest(HttpMethod method, string uri, object value = null)
        {
            var request = new HttpRequestMessage(method, uri);
            if (value != null)
                request.Content = new StringContent(JsonSerializer.Serialize(value), Encoding.UTF8, "application/json");
            return request;
        }

        private async Task sendRequest(HttpRequestMessage request)
        {
            await addJwtHeader(request);

            // send request
            using var response = await _httpClient.SendAsync(request);

            // auto logout on 401 response
            if (response.StatusCode == HttpStatusCode.Unauthorized)
            {
                if (!_navigationManager.Uri.ToLower().Trim().Contains("login"))
                {
                    _navigationManager.NavigateTo("/Logout");
                }
                return;
            }

            await handleErrors(response);
        }

        private async Task<T> sendRequest<T>(HttpRequestMessage request)
        {
            await addJwtHeader(request);
            try
            {
                // send request
                using var response = await _httpClient.SendAsync(request);

                // auto logout on 401 response
                if (response.StatusCode == HttpStatusCode.Unauthorized)
                {
                    if (!_navigationManager.Uri.ToLower().Trim().Contains("login"))
                    {
                        _navigationManager.NavigateTo("/Logout");
                    }
                    return default;
                }

                await handleErrors(response);

                var options = new JsonSerializerOptions();
                options.PropertyNameCaseInsensitive = true;
                options.Converters.Add(new StringConverter());
                var res = await response.Content.ReadFromJsonAsync<T>(options);
                return res;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task addJwtHeader(HttpRequestMessage request)
        {
            // add jwt auth header if user is logged in and request is to the api url
            string token = await _localStorageService.GetItem<string>("token");
            var isApiUrl = !request.RequestUri.IsAbsoluteUri;
            if (token != null && isApiUrl)
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private async Task handleErrors(HttpResponseMessage response)
        {
            // throw exception on error response
            if (!response.IsSuccessStatusCode)
            {
                try
                {
                    var msg = await response.Content.ReadAsStringAsync();

                    var error = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
                    throw new Exception(error["message"]);
                }
                catch (Exception e)
                {
                    throw;
                }
            }
        }
    }
    public class StringConverter : JsonConverter<string>
    {
        public override string Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            // deserialize numbers as strings.
            if (reader.TokenType == JsonTokenType.Number)
            {
                return reader.GetInt32().ToString();
            }
            else if (reader.TokenType == JsonTokenType.String)
            {
                return reader.GetString();
            }

            throw new JsonException();
        }

        public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value);
        }
    }
}
