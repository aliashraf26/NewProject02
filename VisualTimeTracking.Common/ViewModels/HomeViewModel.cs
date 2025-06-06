using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TrackingReports.Models;

namespace VisualTimeTracking.Common.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<PieChartObj> PieChart { get; set; }
        public LineChartObj LineChart { get; set; }
        public List<TableObj> TableData { get;  set; }
        public int UserCount { get;  set; }
        public decimal TotalTime { get;  set; }
        public int AppCount { get;  set; }
        public List<EmpData> StaffList { get;  set; }
        public int IdleTime { get;  set; }
    }
}
