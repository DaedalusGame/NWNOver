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
    public partial class NumberInputDialog : Form
    {
        public int RowsPicked
        {
            get
            {
                return (int)numericUpDown1.Value;
            }
        }

        public NumberInputDialog(int rows)
        {
            InitializeComponent();
            numericUpDown1.Value = Math.Max(1,rows);
        }
    }
}
