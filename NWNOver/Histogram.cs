using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace NWNOver
{
    public partial class Histogram : Form
    {
        public IEnumerable<object> Data;

        class HistogramData
        {
            Histogram Parent;

            public HistogramData(Histogram parent, object value, int count)
            {
                Parent = parent;
                Value = value;
                Count = count;
            }

            public object Value
            {
                get;
                private set;
            }

            public int Count
            {
                get;
                private set;
            }

            public double Percentage
            {
                get
                {
                    return Count / (double)Parent.Data.Count();
                }
            }
        }

        public Histogram(IEnumerable<object> data)
        {
            InitializeComponent();
            Data = data;

            BindingList<HistogramData> binding = new BindingList<HistogramData>();
            
            foreach(var histogramData in data.GroupBy(x => x).OrderBy(x => x.Key))
            {
                binding.Add(new HistogramData(this, histogramData.Key, histogramData.Count()));
            }

            grid_histogram.DataSource = binding;
        }
    }
}
