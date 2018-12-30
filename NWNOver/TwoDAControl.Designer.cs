namespace NWNOver
{
    partial class TwoDAControl
    {
        /// <summary> 
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary> 
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.grid_table = new System.Windows.Forms.DataGridView();
            this.menu_flags = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_bool = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_bool_true = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_bool_false = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_bool_null = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_strref = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_strref_goto = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_strref_new = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_strref_edit = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_enum = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_column = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_column_histogram = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_2da = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            ((System.ComponentModel.ISupportInitialize)(this.grid_table)).BeginInit();
            this.menu_bool.SuspendLayout();
            this.menu_strref.SuspendLayout();
            this.menu_column.SuspendLayout();
            this.menu_2da.SuspendLayout();
            this.SuspendLayout();
            // 
            // grid_table
            // 
            this.grid_table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_table.Location = new System.Drawing.Point(0, 0);
            this.grid_table.Name = "grid_table";
            this.grid_table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_table.Size = new System.Drawing.Size(769, 480);
            this.grid_table.TabIndex = 0;
            this.grid_table.CellBeginEdit += new System.Windows.Forms.DataGridViewCellCancelEventHandler(this.grid_table_CellBeginEdit);
            this.grid_table.CellContextMenuStripNeeded += new System.Windows.Forms.DataGridViewCellContextMenuStripNeededEventHandler(this.grid_table_CellContextMenuStripNeeded);
            this.grid_table.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_table_CellFormatting);
            this.grid_table.CellMouseClick += new System.Windows.Forms.DataGridViewCellMouseEventHandler(this.grid_table_CellMouseClick);
            this.grid_table.CellToolTipTextNeeded += new System.Windows.Forms.DataGridViewCellToolTipTextNeededEventHandler(this.grid_table_CellToolTipTextNeeded);
            this.grid_table.DataError += new System.Windows.Forms.DataGridViewDataErrorEventHandler(this.grid_table_DataError);
            this.grid_table.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grid_table_RowPostPaint);
            // 
            // menu_flags
            // 
            this.menu_flags.Name = "menu_flags";
            this.menu_flags.Size = new System.Drawing.Size(61, 4);
            this.menu_flags.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.menu_flags_Closing);
            this.menu_flags.Opening += new System.ComponentModel.CancelEventHandler(this.menu_flags_Opening);
            // 
            // menu_bool
            // 
            this.menu_bool.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_bool_true,
            this.menu_bool_false,
            this.menu_bool_null});
            this.menu_bool.Name = "menu_bool";
            this.menu_bool.Size = new System.Drawing.Size(120, 70);
            this.menu_bool.Opening += new System.ComponentModel.CancelEventHandler(this.menu_bool_Opening);
            // 
            // menu_bool_true
            // 
            this.menu_bool_true.Name = "menu_bool_true";
            this.menu_bool_true.Size = new System.Drawing.Size(119, 22);
            this.menu_bool_true.Text = "Set True";
            this.menu_bool_true.Click += new System.EventHandler(this.menu_bool_true_Click);
            // 
            // menu_bool_false
            // 
            this.menu_bool_false.Name = "menu_bool_false";
            this.menu_bool_false.Size = new System.Drawing.Size(119, 22);
            this.menu_bool_false.Text = "Set False";
            this.menu_bool_false.Click += new System.EventHandler(this.menu_bool_false_Click);
            // 
            // menu_bool_null
            // 
            this.menu_bool_null.Name = "menu_bool_null";
            this.menu_bool_null.Size = new System.Drawing.Size(119, 22);
            this.menu_bool_null.Text = "Reset";
            this.menu_bool_null.Click += new System.EventHandler(this.menu_bool_null_Click);
            // 
            // menu_strref
            // 
            this.menu_strref.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_strref_goto,
            this.menu_strref_new,
            this.menu_strref_edit});
            this.menu_strref.Name = "menu_strref";
            this.menu_strref.Size = new System.Drawing.Size(156, 70);
            this.menu_strref.Opening += new System.ComponentModel.CancelEventHandler(this.menu_strref_Opening);
            // 
            // menu_strref_goto
            // 
            this.menu_strref_goto.Name = "menu_strref_goto";
            this.menu_strref_goto.Size = new System.Drawing.Size(155, 22);
            this.menu_strref_goto.Text = "Goto";
            this.menu_strref_goto.Click += new System.EventHandler(this.menu_strref_goto_Click);
            // 
            // menu_strref_new
            // 
            this.menu_strref_new.Name = "menu_strref_new";
            this.menu_strref_new.Size = new System.Drawing.Size(155, 22);
            this.menu_strref_new.Text = "New (User TLK)";
            this.menu_strref_new.Click += new System.EventHandler(this.menu_strref_new_Click);
            // 
            // menu_strref_edit
            // 
            this.menu_strref_edit.Name = "menu_strref_edit";
            this.menu_strref_edit.Size = new System.Drawing.Size(155, 22);
            this.menu_strref_edit.Text = "Edit (User TLK)";
            this.menu_strref_edit.Click += new System.EventHandler(this.menu_strref_edit_Click);
            // 
            // menu_enum
            // 
            this.menu_enum.Name = "menu_flags";
            this.menu_enum.Size = new System.Drawing.Size(61, 4);
            this.menu_enum.Closing += new System.Windows.Forms.ToolStripDropDownClosingEventHandler(this.menu_enum_Closing);
            this.menu_enum.Opening += new System.ComponentModel.CancelEventHandler(this.menu_enum_Opening);
            // 
            // menu_column
            // 
            this.menu_column.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_column_histogram});
            this.menu_column.Name = "menu_column";
            this.menu_column.Size = new System.Drawing.Size(163, 26);
            // 
            // menu_column_histogram
            // 
            this.menu_column_histogram.Name = "menu_column_histogram";
            this.menu_column_histogram.Size = new System.Drawing.Size(162, 22);
            this.menu_column_histogram.Text = "Show Histogram";
            this.menu_column_histogram.Click += new System.EventHandler(this.menu_column_histogram_Click);
            // 
            // menu_2da
            // 
            this.menu_2da.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.closeToolStripMenuItem});
            this.menu_2da.Name = "menu_tlk";
            this.menu_2da.Size = new System.Drawing.Size(153, 48);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(152, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // TwoDAControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.grid_table);
            this.Name = "TwoDAControl";
            this.Size = new System.Drawing.Size(772, 483);
            ((System.ComponentModel.ISupportInitialize)(this.grid_table)).EndInit();
            this.menu_bool.ResumeLayout(false);
            this.menu_strref.ResumeLayout(false);
            this.menu_column.ResumeLayout(false);
            this.menu_2da.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.DataGridView grid_table;
        private System.Windows.Forms.ContextMenuStrip menu_flags;
        private System.Windows.Forms.ContextMenuStrip menu_bool;
        private System.Windows.Forms.ToolStripMenuItem menu_bool_null;
        private System.Windows.Forms.ContextMenuStrip menu_strref;
        private System.Windows.Forms.ToolStripMenuItem menu_bool_true;
        private System.Windows.Forms.ToolStripMenuItem menu_bool_false;
        private System.Windows.Forms.ToolStripMenuItem menu_strref_goto;
        private System.Windows.Forms.ToolStripMenuItem menu_strref_edit;
        private System.Windows.Forms.ToolStripMenuItem menu_strref_new;
        private System.Windows.Forms.ContextMenuStrip menu_enum;
        private System.Windows.Forms.ContextMenuStrip menu_column;
        private System.Windows.Forms.ToolStripMenuItem menu_column_histogram;
        public System.Windows.Forms.ContextMenuStrip menu_2da;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    }
}
