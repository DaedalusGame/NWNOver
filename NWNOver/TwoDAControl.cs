using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using NWNOver.TwoDA;
using System.IO;

namespace NWNOver
{
    public partial class TwoDAControl : UserControl
    {
        public TwoDAFile File;
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

        public TwoDAControl(TwoDAFile file, NWNOver form)
        {
            InitializeComponent();
            File = file;
            MainForm = form;

            Environment.OnTLKChanged += (env) => grid_table.Refresh();
            Environment.OnStrRefFormatChanged += (env) => grid_table.Refresh();
            Environment.OnFileRefFormatChanged += (env) => grid_table.Refresh();
            Environment.OnTwoDARefFormatChanged += (env) => grid_table.Refresh();
            Environment.OnBoolFormatChanged += (env) => grid_table.Refresh();
            BindingSource source = new BindingSource();
            source.DataSource = file;

            grid_table.AutoGenerateColumns = false;


            for (int i = 0; i < File.IndexColumns; i++)
            {
                string columnName = File.IndexNames[i];
                grid_table.Columns.Add(new DataGridViewTextBoxColumn() { Name = columnName, DataPropertyName = "column" + i });
            }

            grid_table.DataSource = source;
        }

        public void SetPath(string path)
        {
            FilePath = path;
            Tab.Name = path;
        }

        internal void Open2DALine(int line)
        {
            MainForm.tabControl1.SelectedTab = Tab;
            MainForm.tabControl1.Select();
            grid_table.CurrentCell = grid_table.Rows[line].Cells[0];
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
            if (grid.RowHeadersWidth < textSize.Width + 40)
            {
                grid.RowHeadersWidth = textSize.Width + 40;
            }
            var headerBounds = new Rectangle(e.RowBounds.Left, e.RowBounds.Top, grid.RowHeadersWidth, e.RowBounds.Height);
            e.Graphics.DrawString(rowIdx, this.Font, SystemBrushes.ControlText, headerBounds, centerFormat);

        }

        private void grid_table_CellToolTipTextNeeded(object sender, DataGridViewCellToolTipTextNeededEventArgs e)
        {
            if (File.Schema.HasTooltip(e.ColumnIndex))
                e.ToolTipText = File.Schema.GetTooltip(File, e.ColumnIndex, e.RowIndex);
        }

        private void grid_table_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var grid = sender as DataGridView;

            var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
            if (!cell.IsInEditMode)
            {
                if (File.Schema.IsType<BoolColumn>(e.ColumnIndex))
                {
                    if (e.Value == null)
                        e.CellStyle.BackColor = Color.White;
                    else
                    {
                        bool boolValue = (bool)e.Value;
                        e.CellStyle.BackColor = boolValue ? Color.FromArgb(128, 255, 128) : Color.FromArgb(255, 128, 128);
                    }
                }
                e.Value = File.Schema.Format(e.ColumnIndex, e.Value);
                //e.FormattingApplied = true;
            }
            /*if (grid.Columns[e.ColumnIndex].Name == "Name" && e.Value != null)
            {
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (!cell.IsInEditMode)
                {
                    uint strref;
                    UInt32.TryParse(((string)e.Value), out strref);
                    e.Value = String.Format("({0}) {1}", strref, FormatStrRefValue(strref), Environment.IsUserStrRef(strref) ? "User" : "Default", Environment.NormalizeStrRef(strref));
                    e.FormattingApplied = true;
                }  
            }*/
        }

        bool beginEdit = false;

        private void grid_table_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            var grid = sender as DataGridView;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (File.Schema.IsType<BoolColumn>(e.ColumnIndex))
                {
                    if (cell.Value != null)
                        cell.Value = !(bool)cell.Value;
                    e.Cancel = true;
                }
            }
        }

        private void grid_table_CellContextMenuStripNeeded(object sender, DataGridViewCellContextMenuStripNeededEventArgs e)
        {
            var grid = sender as DataGridView;

            if (grid.IsCurrentCellInEditMode)
                return;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
                if (File.Schema.IsType<FlagsColumn>(e.ColumnIndex))
                {
                    grid.CurrentCell = cell;
                    CurrentFlagInfo = ((FlagsColumn)File.Schema.Columns[e.ColumnIndex]).Info;
                    e.ContextMenuStrip = menu_flags;
                }
                if (File.Schema.IsType<BoolColumn>(e.ColumnIndex))
                {
                    grid.CurrentCell = cell;
                    e.ContextMenuStrip = menu_bool;
                }
                if (File.Schema.IsType<StrRefColumn>(e.ColumnIndex))
                {
                    grid.CurrentCell = cell;
                    e.ContextMenuStrip = menu_strref;
                }
                if (File.Schema.IsType<EnumColumn>(e.ColumnIndex))
                {
                    grid.CurrentCell = cell;
                    CurrentValidValues = ((EnumColumn)File.Schema.Columns[e.ColumnIndex]).ValidValues;
                    e.ContextMenuStrip = menu_enum;
                }
            }

            if(e.RowIndex < 0 && e.ColumnIndex >= 0)
            {
                var cell = grid.Rows[grid.CurrentCell.RowIndex].Cells[e.ColumnIndex];
                grid.CurrentCell = cell;
                e.ContextMenuStrip = menu_column;
            }
        }

        private void grid_table_CellMouseClick(object sender, DataGridViewCellMouseEventArgs e)
        {
            var grid = sender as DataGridView;

            if (e.RowIndex >= 0 && e.ColumnIndex >= 0)
            {
                var cell = grid.Rows[e.RowIndex].Cells[e.ColumnIndex];
              
                if (e.Button == MouseButtons.Middle && !grid.IsCurrentCellInEditMode)
                {
                    grid.CurrentCell = cell;
                    if (cell.Value != null && File.Schema.IsType<StrRefColumn>(e.ColumnIndex))
                    {
                        uint strref = (uint)(int)cell.Value;
                        GotoTLKLine(strref, false);
                    }
                    if (cell.Value != null && File.Schema.IsType<TwoDARefColumn>(e.ColumnIndex))
                    {
                        int row = (int)cell.Value;
                        string filename = ((TwoDARefColumn)File.Schema.Columns[e.ColumnIndex]).Filename;
                        GotoTwoDALine(Path.GetFileNameWithoutExtension(filename),row);
                    }
                }
            }
        }

        private void GotoTwoDALine(string filename)
        {
            Environment.OpenTwoDALine(filename);
        }

        private void GotoTwoDALine(string filename,int row)
        {
            Environment.OpenTwoDALine(filename, row);
        }

        private void GotoTLKLine(uint strref, bool edit)
        {
            if (Environment.IsUserStrRef(strref))
            {
                if (Environment.UserTLK != null)
                    Environment.OpenTLKLine(Environment.UserTLK.Name, Environment.NormalizeStrRef(strref), edit);
            }
            else
            {
                if (Environment.DefaultTLK != null)
                    Environment.OpenTLKLine(Environment.DefaultTLK.Name, Environment.NormalizeStrRef(strref), edit);
            }
        }

        private void grid_table_DataError(object sender, DataGridViewDataErrorEventArgs e)
        {
            e.Cancel = true;
        }

        FlagInfo CurrentFlagInfo;

        private void menu_flags_Opening(object sender, CancelEventArgs e)
        {
            var menu = sender as ContextMenuStrip;

            int flagValue = 0;
            if (grid_table.CurrentCell.Value is int)
                flagValue = (int)grid_table.CurrentCell.Value;
            menu.Items.Clear();
            List<Action> updateActions = new List<Action>();

            foreach (string flagName in CurrentFlagInfo.FlagNames)
            {
                var bits = CurrentFlagInfo.FlagBits[flagName];
                var button = new ToolStripButton(flagName, null, (senderItem, eItem) =>
                {
                    flagValue ^= bits;
                    updateActions.ForEach(x => x());
                });
                button.Checked = (flagValue & bits) == bits;
                updateActions.Add(() => { button.Checked = (flagValue & bits) == bits; });
                menu.Items.Add(button);
            }

            menu.Items.Add(new ToolStripSeparator());
            menu.Items.Add("Commit", global::NWNOver.Properties.Resources.icons8_ok_50, (senderItem, eItem) =>
            {
                grid_table.CurrentCell.Value = flagValue;
                menu.Close();
            });

            e.Cancel = false;
        }

        private void menu_flags_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {
            if (e.CloseReason == ToolStripDropDownCloseReason.ItemClicked)
                e.Cancel = true;
        }

        List<string> CurrentValidValues;

        private void menu_enum_Opening(object sender, CancelEventArgs e)
        {
            var menu = sender as ContextMenuStrip;

            menu.Items.Clear();

            foreach (string validValue in CurrentValidValues)
            {
                var name = validValue == null ? "Reset" : validValue;
                var button = new ToolStripButton(name, null, (senderItem, eItem) =>
                {
                    grid_table.CurrentCell.Value = validValue;
                });
                button.Checked = (string)grid_table.CurrentCell.Value == validValue;
                menu.Items.Add(button);
            }

            e.Cancel = false;
        }

        private void menu_enum_Closing(object sender, ToolStripDropDownClosingEventArgs e)
        {

        }

        private void menu_bool_Opening(object sender, CancelEventArgs e)
        {
            var cell = grid_table.CurrentCell;

            menu_bool_true.Checked = cell.Value != null && (bool)cell.Value;
            menu_bool_false.Checked = cell.Value != null && !(bool)cell.Value;
            menu_bool_null.Checked = cell.Value == null;
        }

        private bool CanGotoStrRef(uint strref)
        {
            if (Environment.IsUserStrRef(strref))
                return Environment.UserTLK != null;
            else
                return Environment.DefaultTLK != null;
        }

        private void menu_strref_Opening(object sender, CancelEventArgs e)
        {
            var cell = grid_table.CurrentCell;

            menu_strref_goto.Enabled = cell.Value != null && CanGotoStrRef((uint)(int)cell.Value);
            menu_strref_new.Enabled = Environment.UserTLK != null;
            menu_strref_edit.Enabled = Environment.UserTLK != null;
        }

        private void menu_bool_true_Click(object sender, EventArgs e)
        {
            grid_table.CurrentCell.Value = true;
        }

        private void menu_bool_false_Click(object sender, EventArgs e)
        {
            grid_table.CurrentCell.Value = false;
        }

        private void menu_bool_null_Click(object sender, EventArgs e)
        {
            grid_table.CurrentCell.Value = null;
        }

        private void menu_strref_goto_Click(object sender, EventArgs e)
        {
            var cell = grid_table.CurrentCell;
            GotoTLKLine((uint)(int)cell.Value, false);
        }

        private void menu_strref_new_Click(object sender, EventArgs e)
        {
            var cell = grid_table.CurrentCell;
            uint addedIndex = AddTLKLine(String.Empty);
            cell.Value = (int)addedIndex;
            GotoTLKLine(addedIndex, false);
        }

        private void menu_strref_edit_Click(object sender, EventArgs e)
        {
            var cell = grid_table.CurrentCell;
            uint editIndex = 0;
            if(cell.Value != null)
                editIndex = (uint)(int)cell.Value;
            if (!Environment.IsUserStrRef(editIndex))
            {
                var originalLine = Environment.GetTLKLine(editIndex);
                editIndex = AddTLKLine(originalLine?.String);
                cell.Value = (int)editIndex;
            }
            GotoTLKLine(editIndex, true);
        }

        private void menu_column_histogram_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;

            Histogram histogram = new Histogram(File.GetColumn(column));
            histogram.ShowDialog();
        }

        private uint AddTLKLine(string text)
        {
            uint addedIndex = Environment.GetUserStrRef(Environment.UserTLK.StringCount);
            Environment.UserTLK.AddTextLine(text);
            return addedIndex;
        }

        private void closeToolStripMenuItem_Click(object sender, EventArgs e)
        {
            MainForm.CloseTab((TabPage)Parent);
        }

        internal string ClipCut()
        {
            int row = grid_table.CurrentCell.RowIndex;
            if (row >= 0)
            {
                var data = File.GetRow(row).Select(x => TwoDAFile.ValueToString(x)).ToArray();
                File.ResetRow(row);
                return string.Join(" ", data);
            }
            return null;
        }

        internal string ClipCopy()
        {
            int row = grid_table.CurrentCell.RowIndex;
            if (row >= 0)
            {
                var data = File.GetRow(row).Select(x => TwoDAFile.ValueToString(x)).ToArray();
                return string.Join(" ", data);
            }
            return null;
        }

        internal void ClipPaste(string v)
        {
            int row = grid_table.CurrentCell.RowIndex;
            if(row >= 0)
            {
                var split = TwoDAFile.SplitData(v);
                if(split.Count() == File.Width)
                {
                    File.SetRow(row, split);
                }
            }
        }

        private void SetColumnType(TwoDAColumn columnType)
        {
            columnType.SetEnvironment(Environment);
            File.Schema = File.Schema.Copy();
            File.Schema.AddColumn(columnType);
            grid_table.Refresh();
        }

        private void menu_column_type_string_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            SetColumnType(new StringColumn(column));
        }

        private void menu_column_type_bool_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            SetColumnType(new BoolColumn(column));
        }

        private void menu_column_type_int_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            SetColumnType(new IntColumn(column));
        }

        private void menu_column_type_float_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            SetColumnType(new FloatColumn(column));
        }

        private void menu_column_type_strref_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            SetColumnType(new StrRefColumn(column));
        }

        private void menu_column_type_2daref_Click(object sender, EventArgs e)
        {
            var column = grid_table.CurrentCell.ColumnIndex;
            string filename = Prompt.ShowTextDialog("Which 2da file should this column reference?","Question");
            if (filename != null)
            {
                filename = Path.GetFileNameWithoutExtension(filename).ToLower() + ".2da";
                SetColumnType(new TwoDARefColumn(column, filename));
            }
        }
    }
}
