using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VisualTimeTracking.Common.ViewModels
{
    public class APIRequest
    {
        public string UserEmail { get; set; }
        public string Data { get; set; }
        public string uuid { get; set; }
    }
    public class APINotification
    {
        public string ClientGUID { get; set; }
        public string InspectorName { get; set; }
    }
    public class APIResponse
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Data { get; set; }
        public int CacheDays { get; set; } = 14;
        public bool SendDebugReport { get; set; } = false;

    }
    public class APIResponseOTP
    {
        public string Message { get; set; }
        public bool Success { get; set; }
        public string Data { get; set; }
        public string containerName { get; set; }
        public string accountName { get; set; }
        public string accountKey { get; set; }
        public int CacheDays { get; set; } = 14;
    }

    public class TimeData
    {
        string refNo = "";
        string appName = "";
        string vASection = "";
        string task = "";
        string comment = "";
        DateTime date = DateTime.Now;
        string startTime = "";
        string endTime = "";
        string totalMinuts = "";

        string domainUsername = "";
        string department = "";
        string procId = "";

        public string AppName { get => appName; set => appName = value; }
        public string RefNo { get => refNo; set => refNo = value; }
        public string VASection { get => vASection; set => vASection = value; }
        public string Task { get => task; set => task = value; }
        public string Comment { get => comment; set => comment = value; }
        public DateTime Date { get => date; set => date = value; }
        public string StartTime { get => startTime; set => startTime = value; }
        public string EndTime { get => endTime; set => endTime = value; }

        public string DomainUsername { get => domainUsername; set => domainUsername = value; }
        public string Department { get => department; set => department = value; }
        public string ProcId { get => procId; set => procId = value; }
        public string TotalMinuts { get => totalMinuts; set => totalMinuts = value; }
        public int totalMinutes { get; set; }
    }

    public class EmpData
    {
        string displayName = "";
        string domainName = "";

        public string DisplayName { get => displayName; set => displayName = value; }
        public string DomainName { get => domainName; set => domainName = value; }
    }
    public class ParamData
    {
        string clientid = "";
        string domainName = "All";
        DateTime startDate = DateTime.Now;
        DateTime endDate = DateTime.Now;

        public string Clientid { get => clientid; set => clientid = value; }
        public DateTime StartDate { get => startDate; set => startDate = value; }
        public DateTime EndDate { get => endDate; set => endDate = value; }
        public string DomainName { get => domainName; set => domainName = value; }
    }
    public class CompanySummary
    {
        public double TotalWorkHours { get; set; }
        public double TotalIdleHours { get; set; }
        public double AvgWorkHoursPerEmployee { get; set; }
        public double AvgIdleHoursPerEmployee { get; set; }
        public string MostActiveDay { get; set; }
        public string LeastActiveDay { get; set; }
    }

    public class EmployeeSummary
    {
        public string UserName { get; set; }
        public double WorkHours { get; set; }
        public double IdleHours { get; set; }
        public double ProductivityPercent { get; set; }
        public int StarsOutOfFive { get; set; }
        public string MostWorkedSection { get; set; }
    }

    public class SectionSummary
    {
        public string Section { get; set; }
        public double TotalHours { get; set; }
        public double WorkPercentage { get; set; }
    }
    public class TimeDataSummery
    {
        public CompanySummary summary { get; set; } = new CompanySummary();
        public List<EmployeeSummary> employees { get; set; } = new List<EmployeeSummary>();
        public List<SectionSummary> sections { get; set; } = new List<SectionSummary>();
        public string StartDate { get; set; }
        public string EndDate { get; set; }
    }
    public class LoadBusinessTimeDataSummery
    {
        public TimeDataSummery LastWeekSummery { get; set; } = new TimeDataSummery();
        public TimeDataSummery CurrentWeekSummery { get; set; } = new TimeDataSummery();
    }

}
