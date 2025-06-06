using System.Collections.Generic;
using VisualTimeTracking.Common.ViewModels;

namespace TrackingReports.Models
{
    public class TableObj
    {
        public string AppName { get; set; }
        public string RefNo { get; set; }
        public int TotalMinutes { get; set; }
        public List<TimeData> Details { get; set; }
        public int Id { get; set; }
    }
}