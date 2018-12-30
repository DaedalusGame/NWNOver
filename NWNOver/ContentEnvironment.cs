using NWNOver.TLK;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using NWNOver.TwoDA;

namespace NWNOver
{
    public enum StrRefFormat
    {
        StrRefOnly,
        StringOnly,
        StrRefAndString,
        Full
    }

    public enum TwoDARefFormat
    {
        Label,
        LowerCaseNumber,
        UpperCaseNumber,
    }

    public enum FileRefFormat
    {
        UpperCase,
        LowerCase,
        Original,
    }

    public enum BoolFormat
    {
        Text,
        Number,
    }

    public class ContentEnvironment
    {
        Dictionary<StrRefFormat, string> StrRefFormatStrings = new Dictionary<StrRefFormat, string>()
        {
            { StrRefFormat.StrRefOnly, "{0}" },
            { StrRefFormat.StringOnly, "{1}" },
            { StrRefFormat.StrRefAndString, "({0}) {1}" },
            { StrRefFormat.Full, "({2}:{3}) {1}" },
        };
        Dictionary<TwoDARefFormat, string> TwoDARefFormatStrings = new Dictionary<TwoDARefFormat, string>()
        {
            { TwoDARefFormat.Label, "{3}" },
            { TwoDARefFormat.LowerCaseNumber, "{2} ({0})" },
            { TwoDARefFormat.UpperCaseNumber, "{2} ({1})" },
        };

        public Configuration Config;

        public TwoDASchemaDatabase SchemaDatabase;
        public DirectoryInfo Directory
        {
            get
            {
                return Config.Directory;
            }
            set
            {
                Config.Directory = value;
                Config.MarkDirty();
            }
        }

        public StrRefFormat StrRefFormat
        {
            get
            {
                return Config.StrRefFormat;
            }
            set
            {
                Config.StrRefFormat = value;
                Config.MarkDirty();
            }
        }
        public TwoDARefFormat TwoDARefFormat
        {
            get
            {
                return Config.TwoDARefFormat;
            }
            set
            {
                Config.TwoDARefFormat = value;
                Config.MarkDirty();
            }
        }
        public FileRefFormat FileRefFormat
        {
            get
            {
                return Config.FileRefFormat;
            }
            set
            {
                Config.FileRefFormat = value;
                Config.MarkDirty();
            }
        }
        public BoolFormat BoolFormat
        {
            get
            {
                return Config.BoolFormat;
            }
            set
            {
                Config.BoolFormat = value;
                Config.MarkDirty();
            }
        }

        public TLKFile DefaultTLK;
        public TLKFile UserTLK;
        public Dictionary<string, TwoDAFile> TwoDAFiles = new Dictionary<string, TwoDAFile>();

        public event Action<ContentEnvironment> OnStrRefFormatChanged;
        public event Action<ContentEnvironment> OnFileRefFormatChanged;
        public event Action<ContentEnvironment> OnTwoDARefFormatChanged;
        public event Action<ContentEnvironment> OnBoolFormatChanged;
        public event Action<ContentEnvironment> OnTLKChanged;

        public event Action<string, int> OnOpenTwoDALine;
        public event Action<string, uint, bool> OnOpenTLKLine;

        public void AddTwoDA(TwoDAFile file)
        {
            TwoDAFiles[file.Name.ToLower()] = file;
        }

        public void RemoveTwoDA(TwoDAFile file)
        {
            TwoDAFiles.Remove(file.Name.ToLower());
        }

        public TwoDAFile GetTwoDA(string filename)
        {
            filename = Path.GetFileNameWithoutExtension(filename.ToLower());
            if (TwoDAFiles.ContainsKey(filename))
                return TwoDAFiles[filename];
            return null;
        }

        public ContentEnvironment()
        {
            SchemaDatabase = new TwoDASchemaDatabase(this);
            SchemaDatabase.Setup();
        }

        public void OpenTwoDALine(string filename)
        {
            OpenTwoDALine(filename,0);
        }

        public string GetFullPath(string filename)
        {
            return Path.Combine(Directory.FullName, filename);
        }

        public bool HasFile(string filename)
        {
            return File.Exists(GetFullPath(filename));
        }

        public void OpenTwoDALine(string filename, int line)
        {
            OnOpenTwoDALine?.Invoke(filename,line);
        }

        public void OpenTLKLine(string filename, uint line, bool edit)
        {
            OnOpenTLKLine?.Invoke(filename, line, edit);
        }

        public string TwoDARefFormatString
        {
            get
            {
                return TwoDARefFormatStrings[TwoDARefFormat];
            }
        }

        public string StrRefFormatString
        {
            get
            {
                return StrRefFormatStrings[StrRefFormat];
            }
        }

        public void SetStrRefFormat(StrRefFormat format)
        {
            StrRefFormat = format;
            OnStrRefFormatChanged?.Invoke(this);
        }

        internal void SetFileRefFormat(FileRefFormat format)
        {
            FileRefFormat = format;
            OnFileRefFormatChanged?.Invoke(this);
        }

        internal void SetTwoDARefFormat(TwoDARefFormat format)
        {
            TwoDARefFormat = format;
            OnTwoDARefFormatChanged?.Invoke(this);
        }

        internal void SetBoolFormat(BoolFormat format)
        {
            BoolFormat = format;
            OnBoolFormatChanged?.Invoke(this);
        }

        public void SetDefaultTLK(TLKFile file)
        {
            DefaultTLK = file;
            if (UserTLK == DefaultTLK)
                UserTLK = null;
            OnTLKChanged?.Invoke(this);
        }

        public void SetUserTLK(TLKFile file)
        {
            UserTLK = file;
            if (DefaultTLK == UserTLK)
                DefaultTLK = null;
            OnTLKChanged?.Invoke(this);
        }

        public void UnsetTLK(TLKFile file)
        {
            if (DefaultTLK == file)
                DefaultTLK = null;
            if (UserTLK == file)
                UserTLK = null;
            OnTLKChanged?.Invoke(this);
        }

        public TLKLine GetTLKLine(uint strref)
        {
            if(IsUserStrRef(strref))
            {
                return UserTLK?.Get(NormalizeStrRef(strref));
            }
            else
            {
                return DefaultTLK?.Get(NormalizeStrRef(strref));
            }
        }

        public uint NormalizeStrRef(uint strref)
        {
            return strref ^ (IsUserStrRef(strref) ? TLKFile.UserStrRefFlag : 0);
        }

        public string GetTLKString(uint strref)
        {
            return GetTLKLine(strref)?.String;
        }

        public bool IsUserStrRef(uint strref)
        {
            return (strref & TLKFile.UserStrRefFlag) > 0;
        }

        public uint GetUserStrRef(uint strref)
        {
            return strref | TLKFile.UserStrRefFlag;
        }
    }
}
