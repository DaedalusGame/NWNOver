using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    [JsonObject]
    public class Configuration
    {
        public DirectoryInfo Directory = new DirectoryInfo(".");
        [JsonProperty]
        public string DirectoryString
        {
            get
            {
                return Directory.FullName;
            }
            set
            {
                Directory = new DirectoryInfo(value);
            }
        }

        public event Action<object> OnConfigChanged;

        [JsonProperty]
        public StrRefFormat StrRefFormat = StrRefFormat.StringOnly;
        [JsonProperty]
        public TwoDARefFormat TwoDARefFormat = TwoDARefFormat.Label;
        [JsonProperty]
        public FileRefFormat FileRefFormat = FileRefFormat.LowerCase;
        [JsonProperty]
        public BoolFormat BoolFormat = BoolFormat.Text;
        [JsonProperty]
        public ColumnFormat ColumnFormat = ColumnFormat.Name;

        public void MarkDirty()
        {
            OnConfigChanged?.Invoke(this);
        }
    }
}
