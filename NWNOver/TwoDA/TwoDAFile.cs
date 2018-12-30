using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace NWNOver.TwoDA
{
    public class TwoDAFile : IListSource
    {
        private const string DEFAULT_VERSION_STRING = "2DA V2.0";
        static Regex ColumnVerify = new Regex("^[\\S_]+$");
        static Regex StringVerify = new Regex("^(?:\".+?\"|\\S+)$");
        static Regex DefaultHeader = new Regex($"DEFAULT: ({StringVerify.ToString()})");

        public string Name;
        public TwoDASchema Schema;
        string Version;
        public string Default;
        public string[] IndexNames;
        Dictionary<string, int> Index;
        string[,] Data;

        public TwoDAFile(string name)
        {
            Name = name;
        }

        public string this[int x, int y]
        {
            get
            {
                if (x < 0 || y < 0 || x >= Width || y >= Height)
                    return Default;
                return Data[x, y];
            }
        }

        public int Width
        {
            get
            {
                return Data.GetLength(0);
            }
        }

        public int Height
        {
            get
            {
                return Data.GetLength(1);
            }
        }

        public int IndexColumns
        {
            get
            {
                return IndexNames.Length;
            }
        }

        public bool ContainsListCollection
        {
            get
            {
                return true;
            }
        }

        public void Read(Stream stream)
        {
            StreamReader reader = new StreamReader(stream, ASCIIEncoding.ASCII);
            
            var headerVersion = reader.ReadLine();
            Version = headerVersion;
            var headerDefault = reader.ReadLine();
            SetDefault(headerDefault);
            var headerIndex = reader.ReadLine();
            IndexNames = SplitIndex(headerIndex).ToArray();
            CreateIndex();

            List<string> data = new List<string>();
            while (!reader.EndOfStream)
            {
                var line = reader.ReadLine();
                if(!String.IsNullOrWhiteSpace(line))
                    data.Add(line);
            }
            FillData(data);

            stream.Close();
        }

        public void Write(Stream stream)
        {
            StreamWriter writer = new StreamWriter(stream, ASCIIEncoding.ASCII);
            writer.WriteLine(Version ?? DEFAULT_VERSION_STRING);
            writer.WriteLine(Default ?? String.Empty);

            List<int> ColumnWidths = new List<int>(IndexNames.Select(x => ValueToString(x).Length));
            for(int i = 0; i < ColumnWidths.Count; i++)
            {
                ColumnWidths[i] = Math.Max(ColumnWidths[i],GetColumn(i).Max(x => x.Length));
            }

            string padSpacing = new string(' ',3);
            writer.WriteLine(string.Join(padSpacing, IndexNames.Select((name,i) => name.PadRight(ColumnWidths[i]))));
            for (int y = 0; y < Height; y++)
            {
                var line = String.Join(padSpacing, GetRow(y).Select((value, i) => ValueToString(value).PadRight(ColumnWidths[i])));
                writer.WriteLine(line);
            }

            writer.Flush();
            stream.Close();
        }

        public static string ValueToString(string data)
        {
            if (StringVerify.IsMatch(data))
                return data;
            else
                return $"\"{data}\"";
        }

        private void SetDefault(string headerDefault)
        {
            Match defaultHeaderMatch = DefaultHeader.Match(headerDefault);
            if (defaultHeaderMatch.Success)
            {
                Default = defaultHeaderMatch.Groups[1].Value;
            }
        }

        private void CreateIndex()
        {
            Index = new Dictionary<string, int>();
            for(int i = 0; i < IndexNames.Length; i++)
            {
                Index.Add(IndexNames[i], i);
            }
        }

        private void FillData(List<string> data)
        {
            Data = new string[IndexNames.Length, data.Count];
            for (int row = 0; row < data.Count; row++)
            {
                int column = 0;
                foreach (string val in SplitData(data[row]))
                {
                    Data[column, row] = val;
                    column++;
                }
            }
        }

        public int GetRowNumber(int row)
        {
            return int.Parse(Get(0,row));
        }

        public string Get(int column, int row)
        {
            return Data[column, row]; 
        }

        public string Get(string column, int row)
        {
            return Get(Index[column], row);
        }

        public void Set(int column, int row, string value)
        {
            Data[column, row] = value;
            CurrentBindingList.ResetItem(row);
        }

        public void Set(string column, int row, string value)
        {
            Set(Index[column], row, value);
        }

        public void InsertColumn(string column, int index) //Insert an empty column with a respective header
        {
            Resize(Width + 1, Height);
            Permute(x => {
                if (x > index)
                    return x - 1;
                else if (x == index)
                    return -1;
                else
                    return x;
            }, y => y);
        }

        public void InsertRow(int index) //Insert an empty row
        {
            Resize(Width, Height + 1);
            Permute(x => x, y =>
            {
                if (y > index)
                    return y - 1;
                else if (y == index)
                    return -1;
                else
                    return y;
            });
        }

        public void Resize(int newWidth, int newHeight)
        {
            int width = Data.GetLength(0);
            int height = Data.GetLength(1);
            var newData = new string[newWidth, newHeight];
            var newIndexNames = new string[newWidth];

            for (int y = 0; y < Math.Min(height, newHeight); y++)
                for (int x = 0; x < Math.Min(width,newWidth); x++)
                {
                    newData[x, y] = Data[x, y];
                    newIndexNames[x] = IndexNames[x];
                }

            Data = newData;
            IndexNames = newIndexNames;
            UpdateBinding();
        }

        public void Permute(Func<int, int> columnPermutation, Func<int, int> rowPermutation)
        {
            int width = Data.GetLength(0);
            int height = Data.GetLength(1);
            var newData = new string[width, height];
            var newIndexNames = new string[width];

            for (int y = 0; y < height; y++)
                for (int x = 0; x < width; x++)
                {
                    newIndexNames[x] = IndexNames[columnPermutation(x)];
                    newData[x, y] = this[columnPermutation(x), rowPermutation(y)];
                }

            Data = newData;
            IndexNames = newIndexNames;
        }

        public void Renumber()
        {
            for (int y = 0; y < Height; y++)
            {
                Set(0, y, y.ToString());
            }
        }

        public IEnumerable<string> GetRow(int row)
        {
            for (int i = 0; i < Data.GetLength(0); i++)
            {
                yield return Data[i, row];
            }
        }

        public IEnumerable<string> GetColumn(string column)
        {
            return GetColumn(Index[column]);
        }

        public IEnumerable<string> GetColumn(int column)
        {
            for (int i = 0; i < Data.GetLength(1); i++)
            {
                yield return Data[column, i];
            }
        }

        public static IEnumerable<string> SplitIndex(string line)
        {
            StringBuilder builder = new StringBuilder();

            yield return String.Empty;

            foreach (char c in line + " ")
            {
                if (char.IsWhiteSpace(c))
                {
                    if (builder.Length > 0)
                        yield return builder.ToString();
                    builder = builder.Clear();
                }
                else
                    builder.Append(c);
            }
        }

        public static IEnumerable<string> SplitData(string line)
        {
            StringBuilder builder = new StringBuilder();
            bool escaped = false;

            foreach(char c in line+" ")
            {
                if (char.IsWhiteSpace(c) && !escaped)
                {
                    if(builder.Length > 0)
                        yield return builder.ToString();
                    builder = builder.Clear();
                }
                else if (c == '"')
                    escaped = !escaped;
                else
                    builder.Append(c);
            }
        }

        BindingList<TwoDALine> CurrentBindingList;
        bool DirtyBinding;

        public IList GetList()
        {
            if (CurrentBindingList == null)
            {
                CurrentBindingList = new BindingList<TwoDALine>();
                CurrentBindingList.ListChanged += List_ListChanged;
                UpdateBinding();
            }
            return CurrentBindingList;
        }

        private void UpdateBinding()
        {
            CurrentBindingList.RaiseListChangedEvents = false;
            CurrentBindingList.Clear();
            for (int y = 0; y < Height; y++)
            {
                CurrentBindingList.Add(new TwoDALine(this, y));
            }
            CurrentBindingList.RaiseListChangedEvents = true;
            CurrentBindingList.ResetBindings();
        }

        public void SetSchema(TwoDASchema selectedSchema)
        {
            Schema = selectedSchema;
            CurrentBindingList.ResetBindings();
        }

        private void List_ListChanged(object sender, ListChangedEventArgs e)
        {
            var list = (BindingList<TwoDALine>)sender;

            switch (e.ListChangedType)
            {
            }
        }

        internal void SetRow(int row, IEnumerable<string> data)
        {
            int column = 0;
            foreach(var value in data)
            {
                Set(column,row,value);
                column++;
            }
        }

        internal void ResetRow(int row)
        {
            for(int column = 0; column < Width; column++)
            {
                Set(column, row, "****");
            }
        }
    }
}
