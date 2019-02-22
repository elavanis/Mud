using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementSite.Areas.Shared.Model;
using ManagementSite.BL;
using Microsoft.AspNetCore.Mvc;
using Shared.PerformanceCounters.Interface;

namespace ManagementSite.Controllers
{
    public class LineChartController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public JsonResult Player()
        {
            List<ICounters> counters = StatsReader.Stats;
            List<string> xAxis = new List<string>();
            List<decimal> yAxis = new List<decimal>();
            foreach (ICounters item in counters)
            {
                xAxis.Add(item.CounterDateTime.ToString());
                yAxis.Add(item.ConnnectedPlayers);
            }

            Chart chart = CreateChart(xAxis, yAxis, "Players");

            return Json(chart);
        }

        public JsonResult Cpu()
        {
            List<ICounters> counters = StatsReader.Stats;
            List<string> xAxis = new List<string>();
            List<decimal> yAxis = new List<decimal>();
            foreach (ICounters item in counters)
            {
                xAxis.Add(item.CounterDateTime.ToString());
                yAxis.Add(item.CPU);
            }

            Chart chart = CreateChart(xAxis, yAxis, "Cpu %");

            return Json(chart);
        }

        public JsonResult TurnTime()
        {
            List<ICounters> counters = StatsReader.Stats;
            List<string> xAxis = new List<string>();
            List<decimal> yAxis = new List<decimal>();
            foreach (ICounters item in counters)
            {
                xAxis.Add(item.CounterDateTime.ToString());
                yAxis.Add(item.MaxTickTimeInMs);
            }

            Chart chart = CreateChart(xAxis, yAxis, "Turn In Ms");

            return Json(chart);
        }

        public JsonResult Memory()
        {
            List<ICounters> counters = StatsReader.Stats;
            List<string> xAxis = new List<string>();
            List<decimal> yAxis = new List<decimal>();
            foreach (ICounters item in counters)
            {
                xAxis.Add(item.CounterDateTime.ToString());
                yAxis.Add(item.Memory);
            }

            Chart chart = CreateChart(xAxis, yAxis, "Megs");

            return Json(chart);
        }

        private static Chart CreateChart(List<string> xAxis, List<decimal> yAxis, string label)
        {
            Chart chart = new Chart();
            chart.labels = xAxis.ToArray();

            Datasets dataSet = new Datasets();
            dataSet.label = label;
            dataSet.data = yAxis.ToArray();
            dataSet.borderColor = new string[] { "#004080" };
            dataSet.borderWidth = "1";
            dataSet.backgroundColor = new string[] { "rgba(0,64,128,0.4)" };

            chart.datasets = new List<Datasets>() { dataSet };
            return chart;
        }
    }
}