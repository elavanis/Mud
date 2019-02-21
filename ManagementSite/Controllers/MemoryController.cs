using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementSite.Areas.Shared.Model;
using ManagementSite.BL;
using Microsoft.AspNetCore.Mvc;
using Shared.PerformanceCounters.Interface;

namespace ManagementSite.Areas.Memory.Controllers
{
    public class MemoryController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult LineChartData()
        {
            List<ICounters> counters = StatsReader.Stats;
            List<string> xAxis = new List<string>();
            List<int> yAxis = new List<int>();
            foreach (ICounters item in counters)
            {
                xAxis.Add(item.CounterDateTime.ToString());
                yAxis.Add((int)item.Memory);
            }

            Chart chart = new Chart();
            chart.labels = xAxis.ToArray();
            Datasets dataSet = new Datasets();
            dataSet.label = "Megs";
            dataSet.data = yAxis.ToArray();
            dataSet.borderColor = new string[] { "#800080" };
            dataSet.borderWidth = "1";
            chart.datasets = new List<Datasets>() { dataSet };



            //Chart _chart = new Chart();
            //_chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            //_chart.datasets = new List<Datasets>();
            //List<Datasets> _dataSet = new List<Datasets>();
            //_dataSet.Add(new Datasets()
            //{
            //    label = "Current Year",
            //    data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
            //    borderColor = new string[] { "#800080" },
            //    borderWidth = "1"
            //});
            //_chart.datasets = _dataSet;
            return Json(chart);
        }
    }
}