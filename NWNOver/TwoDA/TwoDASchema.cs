using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.TwoDA
{
    public abstract class TwoDAColumn
    {
        protected ContentEnvironment Environment;
        public int ColumnIndex;

        public TwoDAColumn(int column)
        {
            ColumnIndex = column;
        }

        public void SetEnvironment(ContentEnvironment environment)
        {
            Environment = environment;
        }

        public virtual Type GetType()
        {
            return typeof(String);
        }

        public virtual object GetValue(TwoDAFile file, int row)
        {
            var value = file.Get(ColumnIndex, row);
            if (value == "****")
                return null;
            return value;
        }

        public virtual void SetValue(TwoDAFile file, int row, object value)
        {
            if (value == null)
                file.Set(ColumnIndex, row, "****");
            else
                file.Set(ColumnIndex, row, (string)value);
        }

        public virtual object FormatValue(object value)
        {
            return value ?? "****";
        }
    }

    public class StringColumn : TwoDAColumn
    {
        public StringColumn(int column) : base(column)
        {
        }
    }

    public class StrRefColumn : IntColumn
    {
        public StrRefColumn(int column) : base(column)
        {
        }

        public override object FormatValue(object value)
        {
            if (value == null)
                return "****";
            else
            {
                uint strref = (uint)(int)value;
                return String.Format(Environment.StrRefFormatString, strref, FormatStrRefValue(strref), Environment.IsUserStrRef(strref) ? "User" : "Default", Environment.NormalizeStrRef(strref));
            }
        }

        private string FormatStrRefValue(uint strref)
        {
            string str = Environment.GetTLKString(strref);
            if (str == null)
                str = Environment.GetTLKString(0);
            if (str == null)
                str = "?";
            return str;
        }
    }

    public class IntColumn : TwoDAColumn
    {
        public IntColumn(int column) : base(column)
        {
        }

        public override Type GetType()
        {
            return typeof(Int32);
        }

        public override object GetValue(TwoDAFile file, int row)
        {
            string data = file.Get(ColumnIndex, row);
            int value;
            if (int.TryParse(data, out value))
                return value;
            else
                return null;
        }

        public override void SetValue(TwoDAFile file, int row, object value)
        {
            if(value == null)
                file.Set(ColumnIndex, row, "****");
            else
                file.Set(ColumnIndex, row, value.ToString());
        }
    }

    public class FloatColumn : TwoDAColumn
    {
        public FloatColumn(int column) : base(column)
        {
        }

        public override Type GetType()
        {
            return typeof(Single);
        }

        public override object GetValue(TwoDAFile file, int row)
        {
            string data = file.Get(ColumnIndex, row);
            float value;
            if (float.TryParse(data, NumberStyles.Float, NumberFormatInfo.InvariantInfo, out value))
                return value;
            else
                return null;
        }

        public override void SetValue(TwoDAFile file, int row, object value)
        {
            if (value == null)
                file.Set(ColumnIndex, row, "****");
            else
                file.Set(ColumnIndex, row, ((float)value).ToString(NumberFormatInfo.InvariantInfo));
        }
    }

    public class BoolColumn : TwoDAColumn
    {
        public BoolColumn(int column) : base(column)
        {
        }

        public override Type GetType()
        {
            return typeof(Boolean);
        }

        public override object GetValue(TwoDAFile file, int row)
        {
            string data = file.Get(ColumnIndex, row);
            int value;
            if (int.TryParse(data, out value))
                return value > 0;
            else
                return null;
        }

        public override void SetValue(TwoDAFile file, int row, object value)
        {
            if (value == null)
                file.Set(ColumnIndex, row, "****");
            else
            {
                bool boolValue = (bool)value;
                file.Set(ColumnIndex, row, boolValue ? "1" : "0");
            }
        }

        public override object FormatValue(object value)
        {
            if (value == null)
                return "****";
            bool boolValue = (bool)value;
            switch(Environment.BoolFormat)
            {
                case (BoolFormat.Number):
                    return boolValue ? 1 : 0;
                case (BoolFormat.Text):
                    return boolValue.ToString();
                default:
                    return base.FormatValue(value);
            }
        }
    }

    public class EnumColumn : TwoDAColumn
    {
        public List<string> ValidValues;
        Dictionary<string, string> ValueDescription = new Dictionary<string, string>();

        public EnumColumn(int column, IEnumerable<string> validValues, Dictionary<string,string> valueDescription) : this(column, validValues)
        {
            ValueDescription = valueDescription;
        }

        public EnumColumn(int column, IEnumerable<string> validValues) : base(column)
        {
            ValidValues = new List<string>(validValues);
        }

        public override object FormatValue(object value)
        {
            if (value != null && ValueDescription.ContainsKey((string)value))
                return ValueDescription[(string)value];
            return base.FormatValue(value);
        }
    }

    public class TwoDARefColumn : IntColumn
    {
        public string Filename;

        public TwoDARefColumn(int column, string filename) : base(column)
        {
            Filename = filename;
        }

        public override object FormatValue(object value)
        {
            if (value == null)
                return "****";
            else
            {
                TwoDAFile remote = Environment.GetTwoDA(Filename);
                int row = (int)value;
                return string.Format(Environment.TwoDARefFormatString, Filename.ToLower(), Filename.ToUpper(), row, FormatRemoteLabel(remote, row));
            }
        }

        private static string FormatRemoteLabel(TwoDAFile remote, int row)
        {
            string str = row.ToString();
            if(remote != null && row >= 0 && row < remote.Height)
                str = remote.Schema.GetLabel(remote, row);
            return str;
        }
    }

    public class FileRefColumn : TwoDAColumn
    {
        string FilePattern;

        public FileRefColumn(int column, string filePattern) : base(column)
        {
            FilePattern = filePattern;
        }

        public override object FormatValue(object value)
        {
            if (value != null)
            {
                var filename = (string)value;
                switch(Environment.FileRefFormat)
                {
                    case (FileRefFormat.LowerCase):
                        filename = filename.ToLower();
                        break;
                    case (FileRefFormat.UpperCase):
                        filename = filename.ToUpper();
                        break;
                }
                return string.Format(FilePattern, filename);
            }
            else
                return "****";
        }

        public string FormatFilename(string filename)
        {
            return string.Format(FilePattern, filename).ToLower();
        }
    }

    public class FlagsColumn : IntColumn
    {
        public FlagInfo Info;

        public FlagsColumn(int column, FlagInfo info) : base(column)
        {
            Info = info;
        }

        public override object GetValue(TwoDAFile file, int row)
        {
            string data = file.Get(ColumnIndex, row);
            int value;
            if (data != null && data.Length > 2 && int.TryParse(data.Substring(2), NumberStyles.HexNumber, NumberFormatInfo.InvariantInfo, out value))
                return value;
            else
                return "****";
        }

        public override void SetValue(TwoDAFile file, int row, object value)
        {
            if (value == null)
                file.Set(ColumnIndex, row, "****");
            else
                file.Set(ColumnIndex, row, $"0x{Info.Format((int)value)}");
        }

        public override object FormatValue(object value)
        {
            if (value is int)
                return $"0x{Info.Format((int)value)}";
            else
                return "****";
        }
    }

    public abstract class ExtraDataTooltip
    {
        protected ContentEnvironment Environment;

        public void SetEnvironment(ContentEnvironment environment)
        {
            Environment = environment;
        }

        public abstract IEnumerable<int> GetDisplayColumns();

        public abstract string GetTooltipText(TwoDAFile file, int column, int row);
    }

    public class ExtraAlignmentRestrictionData : ExtraDataTooltip
    {
        enum Alignment
        {
            Neutral,
            Lawful,
            Chaotic,
            Good,
            Evil,
        }

        enum AlignmentAxis
        {
            None,
            LawChaos,
            GoodEvil,
            Both,
        }

        public int RestrictionColumn;
        public int RestrictionAxisColumn;
        public int RestrictionInvertColumn;

        public ExtraAlignmentRestrictionData(int restrictionColumn, int axisColumn, int invertColumn)
        {
            RestrictionColumn = restrictionColumn;
            RestrictionAxisColumn = axisColumn;
            RestrictionInvertColumn = invertColumn;
        }

        public override IEnumerable<int> GetDisplayColumns()
        {
            yield return RestrictionColumn;
            yield return RestrictionAxisColumn;
            yield return RestrictionInvertColumn;
        }

        public override string GetTooltipText(TwoDAFile file, int column, int row)
        {
            if (row < 0)
                return null;

            var restriction = file.Schema.GetValue(file, RestrictionColumn, row);
            var axis = file.Schema.GetValue(file, RestrictionAxisColumn, row);
            var invert = file.Schema.GetValue(file, RestrictionInvertColumn, row);

            if (restriction == null || axis == null || invert == null)
                return null;

            bool[,] alignments = new bool[3, 3];
            var restrictFlags = (int)restriction;
            var alignmentAxis = ToAlignmentAxis((int)axis);
            if ((restrictFlags & 0x01) > 0)
                SetAlignments(alignments, Alignment.Neutral, alignmentAxis);

            if ((restrictFlags & 0x02) > 0)
                SetAlignments(alignments, Alignment.Lawful, alignmentAxis);
            if ((restrictFlags & 0x04) > 0)
                SetAlignments(alignments, Alignment.Chaotic, alignmentAxis);
            if ((restrictFlags & 0x08) > 0)
                SetAlignments(alignments, Alignment.Good, alignmentAxis);
            if ((restrictFlags & 0x10) > 0)
                SetAlignments(alignments, Alignment.Evil, alignmentAxis);

            StringBuilder tooltip = new StringBuilder();

            tooltip.AppendLine("Alignments that can take this class:");
            if(restrictFlags != 0x00 && alignmentAxis == AlignmentAxis.None)
                tooltip.AppendLine("(Warning: 0x0 axis setting is untested)");

            for (int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    if(alignments[x,y] == (bool)invert)
                        tooltip.AppendLine(ToAlignment(x,y));
                }

            return tooltip.ToString();
        }

        private AlignmentAxis ToAlignmentAxis(int axis)
        {
            switch(axis)
            {
                case (0x0):
                    return AlignmentAxis.None;
                case (0x1):
                    return AlignmentAxis.LawChaos;
                case (0x2):
                    return AlignmentAxis.GoodEvil;
                case (0x3):
                    return AlignmentAxis.Both;
            }

            throw new Exception();
        }

        private string ToAlignment(int x, int y)
        {
            if (x == 1 && y == 1)
                return "True Neutral";
            return ToLawChaos(x) + " " + ToGoodEvil(y);
        }

        private string ToLawChaos(int x)
        {
            switch(x)
            {
                case (0):
                    return "Lawful";
                case (1):
                    return "Neutral";
                case (2):
                    return "Chaotic";
            }
            throw new Exception();
        }

        private string ToGoodEvil(int y)
        {
            switch (y)
            {
                case (0):
                    return "Good";
                case (1):
                    return "Neutral";
                case (2):
                    return "Evil";
            }
            throw new Exception();
        }

        private void SetAlignments(bool[,] alignments, Alignment alignment, AlignmentAxis axis)
        {
            int axisx = 0;
            int axisy = 0;

            switch(alignment)
            {
                case (Alignment.Neutral):
                    axisx = 1;
                    axisy = 1;
                    break;
                case (Alignment.Lawful):
                    axisx = 0;
                    axisy = 1;
                    axis = AlignmentAxis.LawChaos;
                    break;
                case (Alignment.Chaotic):
                    axisx = 2;
                    axisy = 1;
                    axis = AlignmentAxis.LawChaos;
                    break;
                case (Alignment.Good):
                    axisx = 1;
                    axisy = 0;
                    axis = AlignmentAxis.GoodEvil;
                    break;
                case (Alignment.Evil):
                    axisx = 1;
                    axisy = 2;
                    axis = AlignmentAxis.GoodEvil;
                    break;
            }

            for(int x = 0; x < 3; x++)
                for (int y = 0; y < 3; y++)
                {
                    bool accepted = false;
                    switch(axis)
                    {
                        case (AlignmentAxis.None):
                            accepted = x == axisx && y == axisy;
                            break;
                        case (AlignmentAxis.LawChaos):
                            accepted = x == axisx;
                            break;
                        case (AlignmentAxis.GoodEvil):
                            accepted = y == axisy;
                            break;
                        case (AlignmentAxis.Both):
                            accepted = x == axisx || y == axisy;
                            break;
                    }
                    if (accepted)
                        alignments[x, y] = true;
                }
        }
    }

    public class TwoDASchema
    {
        public string Name;
        public string Description;
        public int LabelColumn;
        public Dictionary<int, TwoDAColumn> Columns = new Dictionary<int, TwoDAColumn>();
        public Dictionary<int, ExtraDataTooltip> Tooltips = new Dictionary<int, ExtraDataTooltip>();

        public bool Invalid
        {
            get
            {
                return Name == null;
            }
        }

        public TwoDASchema()
        {
            AddColumn(new IntColumn(0));
        }

        public object Format(int column, object value)
        {
            if (Columns.ContainsKey(column))
                return Columns[column].FormatValue(value);
            else if (value == null)
                return "****";
            else
                return value;
        }

        public void SetEnvironment(ContentEnvironment environment)
        {
            foreach(var column in Columns.Values)
            {
                column.SetEnvironment(environment);
            }

            foreach(var tooltips in Tooltips.Values)
            {
                tooltips.SetEnvironment(environment);
            }
        }

        public void AddColumn(TwoDAColumn column)
        {
            Columns[column.ColumnIndex] = column;
        }

        public void AddExtraData(ExtraDataTooltip data)
        {
            foreach (int column in data.GetDisplayColumns())
            {
                Tooltips[column] = data;
            }
        }

        public bool IsType<T>(int column)
        {
            if (Columns.ContainsKey(column))
                return Columns[column] is T;
            return false;
        }

        public bool HasTooltip(int column)
        {
            return Tooltips.ContainsKey(column);
        }

        public string GetTooltip(TwoDAFile file, int column, int row)
        {
            return Tooltips[column].GetTooltipText(file, column, row);
        }

        public Type GetType(TwoDAFile file, int column)
        {
            if (Columns.ContainsKey(column))
                return Columns[column].GetType();
            return typeof(String);
        }

        public object GetValue(TwoDAFile file, int column, int row)
        {
            if (Columns.ContainsKey(column))
                return Columns[column].GetValue(file, row);
            return file.Get(column, row);
        }

        public void SetValue(TwoDAFile file, int column, int row, object value)
        {
            if (Columns.ContainsKey(column))
                Columns[column].SetValue(file, row, value);
            else
                file.Set(column, row, (string)value);
        }

        public string GetLabel(TwoDAFile file, int row)
        {
            return file.Get(LabelColumn, row);
        }

        public TwoDASchema Copy()
        {
            TwoDASchema copy = (TwoDASchema)MemberwiseClone();
            copy.Columns = new Dictionary<int, TwoDAColumn>(Columns);
            copy.Tooltips = new Dictionary<int, ExtraDataTooltip>(Tooltips);
            copy.Name = null;
            return copy;
        }

        public IEnumerable<string> GetReferencedTwoDAFiles()
        {
            return Columns.Values.OfType<TwoDARefColumn>().Select(x => x.Filename);
        }
    }
}
