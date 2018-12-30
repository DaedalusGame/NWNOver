using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWNOver.TLK;

namespace NWNOver
{
    public partial class TLKControl : UserControl
    {
        public TLKFile File;
        NWNOver MainForm;

        public string FilePath
        {
            get;
            private set;
        }

        ContentEnvironment Environment
        {
            get
            {
                return MainForm.Environment;
            }
        }

        TabPage Tab
        {
            get
            {
                return (TabPage)this.Parent;
            }
        }


        public TLKControl(TLKFile file, NWNOver form)
        {
            InitializeComponent();
            File = file;
            MainForm = form;
            BindingSource source = new BindingSource();
            source.DataSource = file;

            grid_table.AutoGenerateColumns = false;
            grid_table.Columns.Add(new DataGridViewTextBoxColumn()
            {
                Name = "String",
                Width = 300,
                DataPropertyName = "String",
                DefaultCellStyle = new DataGridViewCellStyle()
                {
                    WrapMode = DataGridViewTriState.True,
                }
            });
            grid_table.AutoSizeRowsMode = DataGridViewAutoSizeRowsMode.DisplayedCellsExceptHeaders;
            var FlagsColumn = new DataGridViewTextBoxColumn() { Name = "Flags", DataPropertyName = "Flags", Tag = "HEXADECIMAL", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells };
            grid_table.Columns.Add(FlagsColumn);
            grid_table.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Text", DataPropertyName = "HasString", MinimumWidth = 24, AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader });
            grid_table.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Sound", DataPropertyName = "HasSoundResRef", MinimumWidth = 24, AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader });
            grid_table.Columns.Add(new DataGridViewCheckBoxColumn() { Name = "Sound Length", DataPropertyName = "HasSoundLength", MinimumWidth = 24, AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCellsExceptHeader });
            grid_table.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SoundResRef", DataPropertyName = "SoundResRef", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });
            grid_table.Columns.Add(new DataGridViewTextBoxColumn() { Name = "VolumeVariance", DataPropertyName = "VolumeVariance", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });
            grid_table.Columns.Add(new DataGridViewTextBoxColumn() { Name = "PitchVariance", DataPropertyName = "PitchVariance", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });
            grid_table.Columns.Add(new DataGridViewTextBoxColumn() { Name = "SoundLength", DataPropertyName = "SoundLength", AutoSizeMode = DataGridViewAutoSizeColumnMode.DisplayedCells });

            grid_table.DataSource = source;
        }


        public void SetPath(string path)
        {
            FilePath = path;
            Tab.Name = path;
        }

        internal void OpenTLKLine(uint line, bool edit)
        {
            MainForm.tabControl1.SelectedTab = Tab;
            MainForm.tabControl1.Select();
            grid_table.CurrentCell = grid_table.Rows[(int)line].Cells["String"];
            if(edit)
                grid_table.BeginEdit(false);
        }

        private void grid_table_RowPostPaint(object sender, DataGridViewRowPostPaintEventArgs e)
        {
            var grid = sender as DataGridView;
            var rowIdx = (e.RowIndex).ToString();

            var centerFormat = new StringFormat()
            {
                // right alignment might actually make more sense for numbers
                Alignment = StringAlignment.Center,

                LineAlignment = StringAlignment.Center
            };
            //get the size of the string
            Size textSize = TextRenderer.MeasureText(rowIdx, this.Font);
            //if header width lower then string width then resize
            if (grid.RowHeadersWidth < textSize.Width + 20)
            {
                grid.RowHeadersWidth = textSize.Width + 20;
            }
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

        }

        private void grid_table_RowEnter(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTextbox();
        }

        private void grid_table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;
            if (grid.Columns[e.ColumnIndex].Name == "String" && e.Value != null)
            {
                e.Value = ((string)e.Value).Replace("\n", "\r\n");
                e.FormattingApplied = true;
            }
        }

        private bool IgnoreTextChanged = false;

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            if (IgnoreTextChanged)
                return;
            if (grid_table.SelectedRows.Count != 1)
                return;
        }

        private void textBox1_Leave(object sender, EventArgs e)
        {
            var cell = grid_table.CurrentRow.Cells["String"];

            if (cell.Value != null)
            {
                string realText = (textBox1.Text).Replace("\r\n", "\n");
                if(realText != (string)cell.Value)
                cell.Value = realText;
            }
        }

        private void grid_table_SelectionChanged(object sender, EventArgs e)
        {
            UpdateTextbox();
        }

        private void UpdateTextbox()
        {
            if (grid_table.SelectedRows.Count == 1)
            {
                var cell = grid_table.SelectedRows[0].Cells["String"];
                textBox1.Enabled = true;
                IgnoreTextChanged = true;
                if (cell.Value != null)
                    textBox1.Text = ((string)cell.Value).Replace("\n", "\r\n");
                IgnoreTextChanged = false;
            }
            else
            {
                textBox1.Enabled = false;
                IgnoreTextChanged = true;
                textBox1.Text = String.Empty;
                IgnoreTextChanged = false;
            }
        }

        private void grid_table_CellValueChanged(object sender, DataGridViewCellEventArgs e)
        {
            UpdateTextbox();
        }

        private void menu_tlk_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            ContextMenuStrip contextMenu = sender as ContextMenuStrip;
            if(e.ClickedItem == menu_tlk_setdefault)
            {
                Environment.SetDefaultTLK(File);
            }
            if (e.ClickedItem == menu_tlk_setuser)
            {
                Environment.SetUserTLK(File);
            }
        }

        private void menu_tlk_Opening(object sender, CancelEventArgs e)
        {
            ContextMenuStrip contextMenu = sender as ContextMenuStrip;
            menu_tlk_setdefault.Checked = File == Environment.DefaultTLK;
            menu_tlk_setuser.Checked = File == Environment.UserTLK;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.CloseTab((TabPage)Parent);
        }

        internal string ClipCut()
        {
            return null;
        }

        internal string ClipCopy()
        {
            return null;
        }

        internal void ClipPaste(string v)
        {
            //NOOP;
        }
    }
}
