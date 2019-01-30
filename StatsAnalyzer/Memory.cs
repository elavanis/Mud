using Shared.PerformanceCounters;
using Shared.PerformanceCounters.Interface;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.DataVisualization.Charting;

namespace StatsAnalyzer
{
    public partial class Memory : Form
    {
        private FileInfo lastFile = null;
        public Memory()
        {
            InitializeComponent();
            timer1.Start();
        }

        public List<Counters> LoadChartData(FileInfo fileToLoadFrom)
        {
            JsonSerialization jsonSerialization = new JsonSerialization();
            List<Counters> counters = jsonSerialization.Deserialize<List<Counters>>(File.ReadAllText(fileToLoadFrom.FullName));

            return counters;
        }
        public void DrawChart(List<Counters> counters)
        {
            List<string> times = new List<string>();
            List<int> memory = new List<int>();

            foreach (Counters counter in counters)
            {
                times.Add(counter.CounterDateTime.ToString());
                memory.Add(counter.Memory);
            }

            Series series = chart1.Series[0];
            series.Name = "Megabytes";
            series.ChartType = SeriesChartType.Line;
            series.Points.DataBindXY(times, memory);


        }

        private void TimerTick(object sender, EventArgs eventArgs)
        {
            DirectoryInfo directoryInfo = new DirectoryInfo(Settings.StatsDirectory);
            FileInfo fileInfo = directoryInfo.GetFiles("*.*", SearchOption.AllDirectories).OrderByDescending(e => e.CreationTimeUtc).First();

            if (lastFile == null
                || lastFile.FullName != fileInfo.FullName)
            {
                lastFile = fileInfo;

                List<Counters> counters = LoadChartData(fileInfo);

                DrawChart(counters);
            }




        }
    }
}
