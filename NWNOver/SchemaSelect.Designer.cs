namespace NWNOver
{
    partial class SchemaSelect
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

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.list_schema = new System.Windows.Forms.ListBox();
            this.text_desc = new System.Windows.Forms.TextBox();
            this.btn_ok = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // list_schema
            // 
            this.list_schema.FormattingEnabled = true;
            this.list_schema.Location = new System.Drawing.Point(12, 12);
            this.list_schema.Name = "list_schema";
            this.list_schema.ScrollAlwaysVisible = true;
            this.list_schema.Size = new System.Drawing.Size(395, 160);
            this.list_schema.TabIndex = 0;
            this.list_schema.Format += new System.Windows.Forms.ListControlConvertEventHandler(this.list_schema_Format);
            this.list_schema.SelectedValueChanged += new System.EventHandler(this.list_schema_SelectedValueChanged);
            // 
            // text_desc
            // 
            this.text_desc.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.text_desc.Location = new System.Drawing.Point(12, 178);
            this.text_desc.Multiline = true;
            this.text_desc.Name = "text_desc";
            this.text_desc.ReadOnly = true;
            this.text_desc.Size = new System.Drawing.Size(395, 53);
            this.text_desc.TabIndex = 1;
            // 
            // btn_ok
            // 
            this.btn_ok.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btn_ok.Location = new System.Drawing.Point(172, 237);
            this.btn_ok.Name = "btn_ok";
            this.btn_ok.Size = new System.Drawing.Size(75, 23);
            this.btn_ok.TabIndex = 2;
            this.btn_ok.Text = "OK";
            this.btn_ok.UseVisualStyleBackColor = true;
            // 
            // SchemaSelect
            // 
            this.AcceptButton = this.btn_ok;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(419, 268);
            this.Controls.Add(this.btn_ok);
            this.Controls.Add(this.text_desc);
            this.Controls.Add(this.list_schema);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "SchemaSelect";
            this.Text = "Set 2DA Schema";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ListBox list_schema;
        private System.Windows.Forms.TextBox text_desc;
        private System.Windows.Forms.Button btn_ok;
    }
}