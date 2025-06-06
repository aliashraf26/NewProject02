using Blazored.Toast.Services;
using VisualTimeTracking.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text.Json;
using VisualTimeTracking.Common.ViewModels;
using System.Text;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using TrackingReports.Models;


namespace VisualTimeTracking.UI.ApiServices
{
    public class AuthService : AuthenticationStateProvider
    {
        private IHttpService _httpService;
        private ILocalStorageServices _localStorageService;
        private NavigationManager _navigationManager;
        private IToastService _toastService;
        private readonly HttpClient _http;

        public AuthService(ILocalStorageServices localStorageService, NavigationManager navigationManager, IToastService toastService, IHttpService httpService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
            _toastService = toastService;

            _httpService = httpService;
            _http = http;
        }
       
        #region user Auth
        public string token { get; private set; } = "";
        public UserViewModel? User { get; private set; }


        public async Task<bool> Login(LoginViewModel model)
        {
            try
            {
                var apiResponse = await _httpService.Post<ServiceResponse<APIResponse>>("/api/auth/Login", model);
                if (apiResponse.Success)
                {
                    if (apiResponse.Data != null)
                    {
                        if (apiResponse.Data.Success && apiResponse.Data.Message.Equals("Please Check Your Email and  enter 6 digit OTP with in 15 mins"))
                        {
                            _toastService.ShowWarning("Please Check Your Email and  enter 6 digit OTP with in 15 mins.");
                            return apiResponse.Data.Success;
                        }
                        else
                        {
                            _toastService.ShowWarning(apiResponse.Data.Message);
                            return apiResponse.Data.Success;

                        }
                    }
                    else
                    {
                        _toastService.ShowWarning(apiResponse.Message);
                        return false;
                    }


                }
                else
                {
                    _toastService.ShowWarning("Invalid login details");
                    return false;
                }
            }
            catch (Exception ex)
            {
                _toastService.ShowError("error");
                return false;
            }
        }
        public async Task<bool> LoginVerify(LoginViewModel model)
        {
            try
            {
                var apiResponse = await _httpService.Post<ServiceResponse<APIResponse>>("/api/auth/Login", model);
                if (apiResponse.Success && apiResponse.Data != null)
                {

                    if (apiResponse.Data.Success)
                    {
                        token = CreateToken(apiResponse.Data.Data, model.Username);

                        await _localStorageService.SetItem<string>("token", token);
                        await _localStorageService.SetItem<string>("company", apiResponse.Data.Message);
                        await _localStorageService.SetItem<string>("companyid", apiResponse.Data.Data);
                        await _localStorageService.SetItem<string>("username", model.Username);
                        await GenerateAuthenticationState(token);
                        _navigationManager.NavigateTo("/BusinessSummaries");

                    }
                    else
                    {
                        _toastService.ShowWarning(apiResponse.Data.Message);
                    }

                }
                else if (apiResponse.Data == null)
                {
                    _toastService.ShowWarning(apiResponse.Message);
                    return false;
                }
                else
                {
                    _toastService.ShowWarning("Invalid login details");
                }
                return apiResponse.Success;

            }
            catch (Exception ex)
            {
                _toastService.ShowError("Invalid login details");
                return false;
            }
        }
       
        public async Task Logout()
        {
            try
            {
                User = null;
                await _localStorageService.RemoveItem("token");

                await GenerateAuthenticationState(null);

                _navigationManager.NavigateTo("/Login");
            }
            catch (Exception ex)
            {
                _toastService.ShowError("error");
            }
        }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            token = await _localStorageService.GetItem<string>("token");

            var state = await GenerateAuthenticationState(token);

            return state;
        }

        private async Task<AuthenticationState> GenerateAuthenticationState(string? tokenParam)
        {
            var identity = new ClaimsIdentity();
            _http.DefaultRequestHeaders.Authorization = null;

            if (!string.IsNullOrEmpty(tokenParam))
            {
                try
                {
                    identity = new ClaimsIdentity(ParseClaimsFromJwt(tokenParam), "jwt");
                    _http.DefaultRequestHeaders.Authorization =
                           new AuthenticationHeaderValue("Bearer", tokenParam.Replace("\"", ""));
                }
                catch
                {
                    await _localStorageService.RemoveItem("token");
                    identity = new ClaimsIdentity();
                }
            }

            var user = new ClaimsPrincipal(identity);

            var state = new AuthenticationState(user);

            NotifyAuthenticationStateChanged(Task.FromResult(state));

            return state;
        }

        public static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
        {
            var payload = jwt.Split('.')[1];
            var jsonBytes = ParseBase64WithoutPadding(payload);
            var keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
            return keyValuePairs.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()));
        }

        private static byte[] ParseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
        private string CreateToken(string Id, string UserName)
        {
            List<Claim> claims;

            var Role = "Owner";

            if (Role != null)
            {
                claims = new List<Claim>
                {
                    new Claim(ClaimTypes.NameIdentifier, Id.ToString()),
                    new Claim(ClaimTypes.Name, UserName),
                    new Claim(ClaimTypes.Role, Convert.ToString(Role))
                };
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("JWTAuthenticationHIGHsecuredPasswordVVVp1OH7Xzyr"));

                var token = new JwtSecurityToken(
                    //issuer: _configuration["JWT:ValidIssuer"],
                    //audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddDays(1),
                    notBefore: DateTime.Now,
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                    );
                var jwt = new JwtSecurityTokenHandler().WriteToken(token);
                return jwt;
            }
            else
            {
                return "Role Missing!";
            }
        }
        #endregion


    }
}
