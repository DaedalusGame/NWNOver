using NWNOver.ERF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.BIF
{
    class KEYFile
    {
        string FileType = "KEY ";
        string FileVersion = "V1  ";
        int BIFCount;
        int KeyCount;
        int FileTableOffset;
        int KeyTableOffset;
        int BuildYear;
        int BuildDay;

        public List<FileListEntry> FileList = new List<FileListEntry>();
        public Dictionary<uint, KeyListEntry> KeyList = new Dictionary<uint, KeyListEntry>();

        public char[] TrimNull = new char[] { '\0' };

        public class FileListEntry
        {
            public uint FileSize;
            public uint FilenameOffset;
            public ushort FilenameSize;
            public ushort Drives;

            public string Filename;

            public override string ToString()
            {
                return Filename;
            }
        }

        public class KeyListEntry
        {
            public string ResRef;
            public ushort ResType;
            public uint ResID;

            public uint VariableBIFIndex
            {
                get
                {
                    return ResID >> 20;
                }
            }

            public uint VariableResTableIndex
            {
                get
                {
                    return ResID - (VariableBIFIndex << 20);
                }
            }

            public uint FixedBIFIndex
            {
                get
                {
                    return ResID >> 20;
                }
            }

            public uint FixedResTableIndex
            {
                get
                {
                    return (ResID - (FixedBIFIndex << 20)) >> 14;
                }
            }

            public override string ToString()
            {
                return String.Format("{0}{1}", ResRef, ERFUtils.GetExtension(ResType));
            }
        }

        public DateTime BuildDate
        {
            get
            {
                return new DateTime(1900 + BuildYear, 1, 1).AddDays(BuildDay);
            }
        }

        public void Decode(Stream stream)
        {
            byte[] buffer = new byte[64];

            stream.Read(buffer, 0, buffer.Length);

            FileType = Encoding.ASCII.GetString(buffer, 0, 4);
            FileVersion = Encoding.ASCII.GetString(buffer, 4, 4);
            BIFCount = BitConverter.ToInt32(buffer, 8);
            KeyCount = BitConverter.ToInt32(buffer, 12);
            FileTableOffset = BitConverter.ToInt32(buffer, 16);
            KeyTableOffset = BitConverter.ToInt32(buffer, 20);
            BuildYear = BitConverter.ToInt32(buffer, 24);
            BuildDay = BitConverter.ToInt32(buffer, 28);

            DecodeFileTable(stream);
            DecodeKeyList(stream);
        }

        public void DecodeFileTable(Stream stream)
        {
            byte[] buffer = new byte[12];
            byte[] namebuffer;

            for (int i = 0; i < BIFCount; i++)
            {
                stream.Seek(FileTableOffset + i * 12, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);

                var entry = new FileListEntry();

                entry.FileSize = BitConverter.ToUInt32(buffer, 0);
                entry.FilenameOffset = BitConverter.ToUInt32(buffer, 4);
                entry.FilenameSize = BitConverter.ToUInt16(buffer, 8);
                entry.Drives = BitConverter.ToUInt16(buffer, 10);

                namebuffer = new byte[entry.FilenameSize];
                stream.Seek(entry.FilenameOffset, SeekOrigin.Begin);
                stream.Read(namebuffer, 0, entry.FilenameSize);

                entry.Filename = Encoding.ASCII.GetString(namebuffer, 0, entry.FilenameSize).TrimEnd(TrimNull);
                FileList.Add(entry);
            }
        }

        public void DecodeKeyList(Stream stream)
        {
            byte[] buffer = new byte[22];

            for (int i = 0; i < KeyCount; i++)
            {
                stream.Seek(KeyTableOffset + i * 22, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);

                var entry = new KeyListEntry();

                entry.ResRef = Encoding.ASCII.GetString(buffer, 0, 16).TrimEnd(TrimNull);
                entry.ResType = BitConverter.ToUInt16(buffer, 16);
                entry.ResID = BitConverter.ToUInt32(buffer, 18);

                KeyList.Add(entry.ResID, entry);
            }
        }
    }
}
