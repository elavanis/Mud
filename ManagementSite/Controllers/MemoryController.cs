using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ManagementSite.Areas.Shared.Model;
using Microsoft.AspNetCore.Mvc;

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
            Chart _chart = new Chart();
            _chart.labels = new string[] { "January", "February", "March", "April", "May", "June", "July", "August", "September", "October", "Novemeber", "December" };
            _chart.datasets = new List<Datasets>();
            List<Datasets> _dataSet = new List<Datasets>();
            _dataSet.Add(new Datasets()
            {
                label = "Current Year",
                data = new int[] { 28, 48, 40, 19, 86, 27, 90, 20, 45, 65, 34, 22 },
                borderColor = new string[] { "#800080" },
                borderWidth = "1"
            });
            _chart.datasets = _dataSet;
            return Json(_chart);
        }
    }
}