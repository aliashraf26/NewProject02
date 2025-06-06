using VisualTimeTracking.BL.Response;
using VisualTimeTracking.Common.ResponseModel;
using VisualTimeTracking.Common.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Security.Claims;
using System.Net.Http;
using VisualTimeTracking.Common;
using Newtonsoft.Json;

namespace VisualTimeTracking.Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class AuthController : ControllerBase
    {
        public AuthController( )
        {
            
        }

        [HttpPost("Login")]
        [AllowAnonymous]
        public async Task<ServiceResponse<APIResponse>> Login(LoginViewModel request)
        {
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                try
                {
                    var apiRequest = new APIRequest
                    {
                        UserEmail = request.Username,
                        Data = request.Password,
                        uuid = Guid.NewGuid().ToString()
                    };
                    using var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(APIURL.apiUrl + "api/TimeTracking/LoginTimeTrackingPortal", apiRequest);

                    if (response.IsSuccessStatusCode)
                    {
                        APIResponse apiResponse = await response.Content.ReadFromJsonAsync<APIResponse>();

                        return new ServiceResponse<APIResponse>
                        {
                            Data = apiResponse
                        };
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return new ServiceResponse<APIResponse>
                        {
                            Data = null,
                            Message = result
                        };
                    } 
                } 
                catch (Exception ex)
                {
                    return GenericResponse<APIResponse>.ReturnError("Exception :" + ex.Message.ToString());
                }
            }
            return GenericResponse<APIResponse>.ReturnError("User Name & Password Required");
        }

        [HttpGet("GetStaffList")]
        [AllowAnonymous]
        public async Task<ServiceResponse<List<EmpData>>> GetStaffList(string clientID)
        {
            if (!string.IsNullOrWhiteSpace(clientID))
            {
                try
                {
                     
                    using var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(APIURL.apiUrl + $"api/quoteAccess/GetStaffList?clientid={clientID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponsedata = await response.Content.ReadAsStringAsync();
                     var apiResponse =   JsonConvert.DeserializeObject<List<EmpData>>(apiResponsedata);
                        return new ServiceResponse<List<EmpData>>
                        {
                            Data = apiResponse
                        };
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return new ServiceResponse<List<EmpData>>
                        {
                            Data = null,
                            Message = result
                        };
                    }
                }
                catch (Exception ex)
                {
                    return GenericResponse<List<EmpData>>.ReturnError("Exception :" + ex.Message.ToString());
                }
            }
            return GenericResponse<List<EmpData>>.ReturnError("User Name & Password Required");
        }

        [HttpGet("GetBAList")]
        [AllowAnonymous]
        public async Task<ServiceResponse<List<string>>> GetBAList(string clientID)
        {
            if (!string.IsNullOrWhiteSpace(clientID))
            {
                try
                {

                    using var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(APIURL.apiUrl + $"api/TimeTracking/GetBAList?clientid={clientID}");

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponsedata = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<List<string>>(apiResponsedata);
                        return new ServiceResponse<List<string>>
                        {
                            Data = apiResponse
                        };
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return new ServiceResponse<List<string>>
                        {
                            Data = null,
                            Message = result
                        };
                    }
                }
                catch (Exception ex)
                {
                    return GenericResponse<List<string>>.ReturnError("Exception :" + ex.Message.ToString());
                }
            }
            return GenericResponse<List<string>>.ReturnError("No Data Found");
        }
        [HttpGet("LoadBusinessTimeDataSummery")]
        [AllowAnonymous]
        public async Task<ServiceResponse<LoadBusinessTimeDataSummery>> LoadBusinessTimeDataSummery(string clientID)
        {
            if (!string.IsNullOrWhiteSpace(clientID))
            {
                try
                { 
                    using var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.GetAsync(APIURL.apiUrl + $"api/TimeTracking/LoadBusinessTimeDataSummery?clientid={clientID}");
                     
                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponsedata = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<LoadBusinessTimeDataSummery>(apiResponsedata);
                        return new ServiceResponse<LoadBusinessTimeDataSummery>
                        {
                            Data = apiResponse
                        };
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return new ServiceResponse<LoadBusinessTimeDataSummery>
                        {
                            Data = null,
                            Message = result
                        };
                    }
                }
                catch (Exception ex)
                {
                    return GenericResponse<LoadBusinessTimeDataSummery>.ReturnError("Exception :" + ex.Message.ToString());
                }
            }
            return GenericResponse<LoadBusinessTimeDataSummery>.ReturnError("User Name & Password Required");
        }
        [HttpPost("LoadDataByParams")]
        [AllowAnonymous]
        public async Task<ServiceResponse<LoadBusinessTimeDataSummery>> LoadDataByParams(ParamData data)
        {
            if (!string.IsNullOrWhiteSpace(data.Clientid) && !string.IsNullOrWhiteSpace(data.Clientid))
            {
                try
                {
                    using var httpClient = new HttpClient();
                    HttpResponseMessage response = await httpClient.PostAsJsonAsync(APIURL.apiUrl + "api/TimeTracking/LoadDataByParams", data);

                    if (response.IsSuccessStatusCode)
                    {
                        var apiResponsedata = await response.Content.ReadAsStringAsync();
                        var apiResponse = JsonConvert.DeserializeObject<LoadBusinessTimeDataSummery>(apiResponsedata);
                        return new ServiceResponse<LoadBusinessTimeDataSummery>
                        {
                            Data = apiResponse
                        };
                    }
                    else
                    {
                        string result = await response.Content.ReadAsStringAsync();
                        return new ServiceResponse<LoadBusinessTimeDataSummery>
                        {
                            Data = null,
                            Message = result
                        };
                    }
                }
                catch (Exception ex)
                {
                    return GenericResponse<LoadBusinessTimeDataSummery>.ReturnError("Exception :" + ex.Message.ToString());
                }
            }
            return GenericResponse<LoadBusinessTimeDataSummery>.ReturnError("Missing Required Data.");
        }
    }
}
