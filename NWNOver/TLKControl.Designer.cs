namespace NWNOver
{
    partial class TLKControl
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
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.grid_table = new System.Windows.Forms.DataGridView();
            this.textBox1 = new System.Windows.Forms.TextBox();
            this.menu_tlk = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.menu_tlk_setdefault = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_tlk_setuser = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.closeToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.menu_flags = new System.Windows.Forms.ContextMenuStrip(this.components);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.grid_table)).BeginInit();
            this.menu_tlk.SuspendLayout();
            this.SuspendLayout();
            // 
            // splitContainer1
            // 
            this.splitContainer1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.AutoScroll = true;
            this.splitContainer1.Panel1.Controls.Add(this.grid_table);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.textBox1);
            this.splitContainer1.Size = new System.Drawing.Size(919, 644);
            this.splitContainer1.SplitterDistance = 579;
            this.splitContainer1.TabIndex = 2;
            // 
            // grid_table
            // 
            this.grid_table.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.grid_table.ClipboardCopyMode = System.Windows.Forms.DataGridViewClipboardCopyMode.Disable;
            this.grid_table.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.grid_table.Location = new System.Drawing.Point(3, 3);
            this.grid_table.Name = "grid_table";
            this.grid_table.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect;
            this.grid_table.Size = new System.Drawing.Size(573, 638);
            this.grid_table.TabIndex = 0;
            this.grid_table.CellFormatting += new System.Windows.Forms.DataGridViewCellFormattingEventHandler(this.grid_table_CellFormatting);
            this.grid_table.CellValueChanged += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_table_CellValueChanged);
            this.grid_table.RowEnter += new System.Windows.Forms.DataGridViewCellEventHandler(this.grid_table_RowEnter);
            this.grid_table.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grid_table_RowPostPaint);
            this.grid_table.SelectionChanged += new System.EventHandler(this.grid_table_SelectionChanged);
            // 
            // textBox1
            // 
            this.textBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.textBox1.Enabled = false;
            this.textBox1.Location = new System.Drawing.Point(3, 3);
            this.textBox1.Multiline = true;
            this.textBox1.Name = "textBox1";
            this.textBox1.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.textBox1.Size = new System.Drawing.Size(330, 638);
            this.textBox1.TabIndex = 0;
            this.textBox1.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            this.textBox1.Leave += new System.EventHandler(this.textBox1_Leave);
            // 
            // menu_tlk
            // 
            this.menu_tlk.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.menu_tlk_setdefault,
            this.menu_tlk_setuser,
            this.toolStripSeparator1,
            this.closeToolStripMenuItem});
            this.menu_tlk.Name = "menu_tlk";
            this.menu_tlk.Size = new System.Drawing.Size(155, 76);
            this.menu_tlk.Opening += new System.ComponentModel.CancelEventHandler(this.menu_tlk_Opening);
            this.menu_tlk.ItemClicked += new System.Windows.Forms.ToolStripItemClickedEventHandler(this.menu_tlk_ItemClicked);
            // 
            // menu_tlk_setdefault
            // 
            this.menu_tlk_setdefault.Name = "menu_tlk_setdefault";
            this.menu_tlk_setdefault.Size = new System.Drawing.Size(154, 22);
            this.menu_tlk_setdefault.Text = "Set Default TLK";
            // 
            // menu_tlk_setuser
            // 
            this.menu_tlk_setuser.Name = "menu_tlk_setuser";
            this.menu_tlk_setuser.Size = new System.Drawing.Size(154, 22);
            this.menu_tlk_setuser.Text = "Set User TLK";
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(151, 6);
            // 
            // closeToolStripMenuItem
            // 
            this.closeToolStripMenuItem.Name = "closeToolStripMenuItem";
            this.closeToolStripMenuItem.Size = new System.Drawing.Size(154, 22);
            this.closeToolStripMenuItem.Text = "Close";
            this.closeToolStripMenuItem.Click += new System.EventHandler(this.closeToolStripMenuItem_Click);
            // 
            // menu_flags
            // 
            this.menu_flags.Name = "menu_flags";
            this.menu_flags.Size = new System.Drawing.Size(61, 4);
            // 
            // TLKControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.BackColor = System.Drawing.SystemColors.Control;
            this.Controls.Add(this.splitContainer1);
            this.Name = "TLKControl";
            this.Size = new System.Drawing.Size(925, 647);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            this.splitContainer1.Panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.grid_table)).EndInit();
            this.menu_tlk.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.DataGridView grid_table;
        private System.Windows.Forms.TextBox textBox1;
        private System.Windows.Forms.ToolStripMenuItem menu_tlk_setdefault;
        private System.Windows.Forms.ToolStripMenuItem menu_tlk_setuser;
        public System.Windows.Forms.ContextMenuStrip menu_tlk;
        private System.Windows.Forms.ContextMenuStrip menu_flags;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripMenuItem closeToolStripMenuItem;
    }
}
