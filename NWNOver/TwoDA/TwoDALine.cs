using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.TwoDA
{
    class TwoDAProperty : PropertyDescriptor
    {
        int ColumnIndex;
        Type Type;

        public TwoDAProperty(int column, Type type) : base("column"+column,null)
        {
            ColumnIndex = column;
            Type = type;
        }

        public override Type ComponentType
        {
            get
            {
                return typeof(TwoDALine);
            }
        }

        public override bool IsReadOnly
        {
            get
            {
                return false;
            }
        }

        public override Type PropertyType
        {
            get
            {
                return Type;
            }
        }

        public override bool CanResetValue(object component)
        {
            return true;
        }

        public override object GetValue(object component)
        {
            TwoDALine line = (TwoDALine)component;

            return line.File.Schema.GetValue(line.File, ColumnIndex, line.RowIndex);
        }

        public override void ResetValue(object component)
        {
            TwoDALine line = (TwoDALine)component;

            line.File.Set(ColumnIndex, line.RowIndex, "****");
        }

        public override void SetValue(object component, object value)
        {
            TwoDALine line = (TwoDALine)component;

            line.File.Schema.SetValue(line.File, ColumnIndex, line.RowIndex, value);
        }

        public override bool ShouldSerializeValue(object component)
        {
            return false;
        }
    }

    class TwoDALine : ICustomTypeDescriptor
    {
        public TwoDAFile File;
        public int RowIndex;

        public TwoDALine(TwoDAFile file, int row)
        {
            File = file;
            RowIndex = row;
        }

        public PropertyDescriptorCollection GetProperties(Attribute[] attributes)
        {
            List<PropertyDescriptor> properties = new List<PropertyDescriptor>();

            //Add property descriptors for each entry in the dictionary
            for (int i = 0; i < File.IndexColumns; i++)
            {
                properties.Add(new TwoDAProperty(i,File.Schema.GetType(File,i)));
            }

            //Get properties also belonging to this class also
            PropertyDescriptorCollection pdc = TypeDescriptor.GetProperties(this.GetType(), attributes);

            foreach (PropertyDescriptor oPropertyDescriptor in pdc)
            {
                properties.Add(oPropertyDescriptor);
            }

            return new PropertyDescriptorCollection(properties.ToArray());
        }

        #region ICustomTypeDescriptor
        public AttributeCollection GetAttributes()
        {
            return new AttributeCollection(null);
        }

        public string GetClassName()
        {
            return null;
        }

        public string GetComponentName()
        {
            return null;
        }

        public TypeConverter GetConverter()
        {
            return null;
        }

        public EventDescriptor GetDefaultEvent()
        {
            return null;
        }

        public PropertyDescriptor GetDefaultProperty()
        {
            return null;
        }

        public object GetEditor(Type editorBaseType)
        {
            return null;
        }

        public EventDescriptorCollection GetEvents()
        {
            return new EventDescriptorCollection(null);
        }

        public EventDescriptorCollection GetEvents(Attribute[] attributes)
        {
            return new EventDescriptorCollection(null);
        }

        public PropertyDescriptorCollection GetProperties()
        {
            return ((ICustomTypeDescriptor)this).GetProperties(null);
        }

        public object GetPropertyOwner(PropertyDescriptor pd)
        {
            return this;
        }
        #endregion
    }
}
