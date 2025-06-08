using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using VisualTimeTracking.Common;
using System.Reflection;
using TrackingReports.Models;
using MudBlazor;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Syncfusion.Blazor.Calendars;

namespace VisualTimeTracking.UI.ApiServices
{
    public class TimeTrackingApi
    {
        private IHttpService _httpService;
        private ILocalStorageServices _localStorageService;
        private NavigationManager _navigationManager;
        private IToastService _toastService;
        private readonly HttpClient _http;

        public TimeTrackingApi(ILocalStorageServices localStorageService, NavigationManager navigationManager, IToastService toastService, IHttpService httpService, HttpClient http)
        {
            _localStorageService = localStorageService;
            _navigationManager = navigationManager;
            _toastService = toastService;

            _httpService = httpService;
            _http = http;

        }


        public async Task<HomeViewModel> Getdata(ParamData data1)
        {

            List<TimeData> data = new List<TimeData>();

            try
            {



                HttpContent httpContent;
                var jsonData = JsonConvert.SerializeObject(data1);

                httpContent = new StringContent(jsonData, Encoding.UTF8, "application/json");
                _http.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
                _http.DefaultRequestHeaders.Add("Connection", "keep-alive");

                var responseToken = await _http.PostAsync("https://visualblazorapi-staging.azurewebsites.net/api/quoteAccess/LoadTimeDataSummery", httpContent);



                string ApiResponseToken = await responseToken.Content.ReadAsStringAsync();


                if (responseToken.IsSuccessStatusCode && !string.IsNullOrEmpty(ApiResponseToken))
                {
                    data = JsonConvert.DeserializeObject<List<TimeData>>(ApiResponseToken);

                }
                else
                    data = new List<TimeData>();
                var totalMinutes = 0;

                foreach (var item in data)
                {
                    if (string.IsNullOrEmpty(item.RefNo))
                    {
                        item.RefNo = item.AppName;
                    }
                    var timeUnit = item.TotalMinuts.Split(':');
                    var hour = Convert.ToInt16(timeUnit[0]);
                    var min = Convert.ToInt16(timeUnit[1]);
                    var sec = Convert.ToInt16(timeUnit[2].Split('.')[0]);

                    var totalMin = (hour * 60) + min + (sec / 60);
                    item.totalMinutes = totalMin;
                    totalMinutes += totalMin;
                }

                var tableDataGroups = data.GroupBy(x => x.RefNo).ToList();

                var tableData = new List<TableObj>();

                foreach (var item in tableDataGroups)
                {
                    var obj = new TableObj
                    {
                        Id = tableData.Count,
                        AppName = item.FirstOrDefault().AppName,
                        RefNo = item.FirstOrDefault().RefNo,
                        TotalMinutes = item.Sum(x => x.totalMinutes),
                        Details = item.ToList()
                    };

                    tableData.Add(obj);
                }
                var appGroups = data.GroupBy(x => x.RefNo)
                    .Select(x => new PieChartObj { name = x.Key, y = x.Sum(y => y.totalMinutes) }).ToList();

                var dateGroups = data.GroupBy(x => x.Date.Date)
                   .Select(x => x.Sum(y => y.totalMinutes)).ToList();

                var dateValGroups = data.GroupBy(x => x.Date.Date)
                   .Select(x => x.Key.ToString("dd MMMM")).ToList();

                var lineChart = new LineChartObj { category = dateValGroups, values = dateGroups };
                var result = tableData.SingleOrDefault(x => x.RefNo.ToLower().Contains("idle"));
                int idlminuts = 0;
                if (result != null)
                {
                    idlminuts = result.TotalMinutes;
                }
                var viewModel = new HomeViewModel
                {
                    PieChart = appGroups,
                    LineChart = lineChart,

                    TableData = tableData,

                    UserCount = data.GroupBy(x => x.DomainUsername).Count(),
                    AppCount = data.GroupBy(x => x.RefNo).Count(),
                    IdleTime = idlminuts,
                };
                decimal sum = data.Sum(y => y.totalMinutes);
                viewModel.TotalTime = Math.Round(Convert.ToDecimal(sum / 60), 2);

                return viewModel;

            }
            catch (Exception ex)
            {


            }

            return null;

        }
        public async Task<LoadBusinessTimeDataSummery> LoadBusinessTimeDataSummery()
        {

            LoadBusinessTimeDataSummery data = new LoadBusinessTimeDataSummery();

            try
            {
                string clientID = await _localStorageService.GetItem<string>("companyid");
                var apiResponse = await _httpService.Get<ServiceResponse<LoadBusinessTimeDataSummery>>("/api/auth/LoadBusinessTimeDataSummery?clientID=" + clientID);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    data = apiResponse.Data;
                }
                return data;
            }
            catch (Exception ex)
            {


            }

            return null;

        }
        //public async Task<LoadBusinessTimeDataSummery> LoadDataByDateRange(DateRange date, string employee)
        //{

        //    LoadBusinessTimeDataSummery data = new LoadBusinessTimeDataSummery();
        //    if (string.IsNullOrEmpty(employee))
        //    {
        //        _toastService.ShowWarning("Please Select an Employee from the List.");
        //        return data;
        //    }
        //    else if (date.Start == DateTime.MinValue || date.End == DateTime.MinValue)
        //    {
        //        _toastService.ShowWarning("Please Select a Valid Date Range.");
        //        return data;
        //    }

        //    try
        //    {
        //        ParamData pdata = new ParamData();
        //        pdata.Clientid = await _localStorageService.GetItem<string>("companyid");
        //        pdata.StartDate = date.Start.GetValueOrDefault();
        //        pdata.EndDate = date.End.GetValueOrDefault();
        //        pdata.DomainName = employee;
        //        var apiResponse = await _httpService.Post<ServiceResponse<LoadBusinessTimeDataSummery>>("/api/auth/LoadDataByParams", pdata);
        //        if (apiResponse.Success && apiResponse.Data != null)
        //        {
        //            data = apiResponse.Data;
        //        }
        //        return data;
        //    }
        //    catch (Exception ex)
        //    {


        //    }

        //    return null;

        //}
        public async Task<LoadBusinessTimeDataSummery> LoadDataByDateRange(SfDateRangePicker<DateTime?> date, string employee)
        {

            LoadBusinessTimeDataSummery data = new LoadBusinessTimeDataSummery();
            if (string.IsNullOrEmpty(employee))
            {
                _toastService.ShowWarning("Please Select an Employee from the List.");
                return data;
            }
            else if (date.StartDate == DateTime.MinValue || date.EndDate == DateTime.MinValue)
            {
                _toastService.ShowWarning("Please Select a Valid Date Range.");
                return data;
            }

            try
            {
                ParamData pdata = new ParamData();
                pdata.Clientid = await _localStorageService.GetItem<string>("companyid");
                pdata.StartDate = date.StartDate.GetValueOrDefault();
                pdata.EndDate = date.EndDate.GetValueOrDefault();
                pdata.DomainName = employee;
                var apiResponse = await _httpService.Post<ServiceResponse<LoadBusinessTimeDataSummery>>("/api/auth/LoadDataByParams", pdata);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    data = apiResponse.Data;
                }
                return data;
            }
            catch (Exception ex)
            {


            }

            return null;

        }
        public async Task<LoadBusinessTimeDataSummery> LoadDataBySelection(int forThis, string employee)
        {

            LoadBusinessTimeDataSummery data = new LoadBusinessTimeDataSummery();
            if (string.IsNullOrEmpty(employee))
            {
                _toastService.ShowWarning("Please Select an Employee from the List.");
                return data;
            }


            try
            {

                ParamData pdata = new ParamData();
                pdata.Clientid = await _localStorageService.GetItem<string>("companyid");
                var (startdate, enddate) = GetLastOrCurrentWorkWeekRange();
                switch (forThis)
                {
                    case 1: //this week
                        (startdate, enddate) = GetLastOrCurrentWorkWeekRange();
                        break;

                    case 2: //last week
                        (startdate, enddate) = GetOnlyLastWorkWeekRange();
                        break;

                    case 3://this month
                        (startdate, enddate) = GetCurrentMonthRange();
                        break;
                    case 4: //last month
                        (startdate, enddate) = GetLastMonthRange();
                        break;
                    default:
                        (startdate, enddate) = GetLastOrCurrentWorkWeekRange();
                        break;
                }
                pdata.StartDate = startdate;
                pdata.EndDate = enddate;
                pdata.DomainName = employee;
                var apiResponse = await _httpService.Post<ServiceResponse<LoadBusinessTimeDataSummery>>("/api/auth/LoadDataByParams", pdata);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    data = apiResponse.Data;
                }
                return data;
            }
            catch (Exception ex)
            {


            }

            return null;

        }
        public async Task<List<EmpData>> GetStaffList()
        {

            try
            {


                string clientID = await _localStorageService.GetItem<string>("companyid");


                var apiResponse = await _httpService.Get<ServiceResponse<List<EmpData>>>("/api/auth/GetStaffList?clientID=" + clientID);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    var data = apiResponse.Data;
                    return data;
                }


                return new List<EmpData>();

            }
            catch (Exception ex)
            {
                return new List<EmpData>();
            }


        }
        public async Task<List<string>> GetBAList()
        {

            try
            {


                string clientID = await _localStorageService.GetItem<string>("companyid");


                var apiResponse = await _httpService.Get<ServiceResponse<List<string>>>("/api/auth/GetBAList?clientID=" + clientID);
                if (apiResponse.Success && apiResponse.Data != null)
                {
                    var data = apiResponse.Data;
                    return data;
                }


                return new List<string>();

            }
            catch (Exception ex)
            {
                return new List<string>();
            }


        }
        public static (DateTime StartDate, DateTime EndDate) GetLastOrCurrentWorkWeekRange()
        {
            var today = DateTime.Today;
            int daysSinceMonday = (int)today.DayOfWeek - (int)DayOfWeek.Monday;
            if (daysSinceMonday < 0) daysSinceMonday += 7;

            DateTime thisWeekMonday = today.AddDays(-daysSinceMonday);

            if (today == thisWeekMonday)
            {
                DateTime lastMonday = thisWeekMonday.AddDays(-7);
                DateTime lastFriday = lastMonday.AddDays(4);
                return (lastMonday, lastFriday);
            }
            else
            {
                DateTime start = thisWeekMonday;
                DateTime end = today.DayOfWeek > DayOfWeek.Friday
                    ? thisWeekMonday.AddDays(4)
                    : today;
                return (start, end);
            }
        }
        public static (DateTime StartDate, DateTime EndDate) GetOnlyLastWorkWeekRange()
        {
            var today = DateTime.Today;

            // Get this week's Monday
            int daysSinceMonday = ((int)today.DayOfWeek + 6) % 7; // Sunday = 6, Monday = 0
            var thisWeekMonday = today.AddDays(-daysSinceMonday);

            // Go back to last week's Monday and Friday
            var lastWeekMonday = thisWeekMonday.AddDays(-7);
            var lastWeekFriday = lastWeekMonday.AddDays(4);

            return (lastWeekMonday, lastWeekFriday);
        }
        public static (DateTime StartDate, DateTime EndDate) GetCurrentMonthRange()
        {
            var today = DateTime.Today;
            var start = new DateTime(today.Year, today.Month, 1);
            var end = start.AddMonths(1).AddDays(-1); // Last day of this month
            return (start, end);
        }

        public static (DateTime StartDate, DateTime EndDate) GetLastMonthRange()
        {
            var today = DateTime.Today;
            var lastMonth = today.AddMonths(-1);
            var start = new DateTime(lastMonth.Year, lastMonth.Month, 1);
            var end = start.AddMonths(1).AddDays(-1); // Last day of last month
            return (start, end);
        }

    }



}

