using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace StatsAnalyzer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();

            Memory memory = new Memory();
            memory.Show();
        }
    }
}
