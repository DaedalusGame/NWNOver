using NWNOver.TwoDA;
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
    public partial class SchemaSelect : Form
    {
        public TwoDASchema SelectedSchema
        {
            get;
            private set;
        }

        public SchemaSelect(TwoDASchemaDatabase database, TwoDASchema current)
        {
            InitializeComponent();
            SelectedSchema = current;
            if(current.Invalid)
                list_schema.Items.Add(current);
            else
                list_schema.Items.Add(new TwoDASchema());
            foreach (var schema in database.GetAllSchemas().OrderBy(x => x.Name))
            {
                list_schema.Items.Add(schema);
            }
            list_schema.SelectedItem = current;
            UpdateDescription();
        }

        private void list_schema_Format(object sender, ListControlConvertEventArgs e)
        {
            var item = (TwoDASchema)e.ListItem;

            if (item.Invalid)
                e.Value = "<none>";
            else
                e.Value = String.Format("{0}.2da",item.Name);
        }

        private void list_schema_SelectedValueChanged(object sender, EventArgs e)
        {
            UpdateDescription();
            SelectedSchema = (TwoDASchema)list_schema.SelectedItem;
        }

        private void UpdateDescription()
        {
            var item = (TwoDASchema)list_schema.SelectedItem;

            if (item.Invalid)
            {
                text_desc.Text = "No schema.";
            }
            else
            {
                StringBuilder desc = new StringBuilder();
                desc.AppendFormat("Schema for '{0}.2da'", item.Name);
                text_desc.Text = desc.ToString();
            }
        }
    }
}
