using NWNOver.ERF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.BIF
{
    class BIFFile
    {
        public FileInfo FilePath;

        public KEYFile KeyFile;
        public uint KeyFileIndex;

        public string FileType;
        public string FileVersion;
        public uint VariableResCount;
        public uint FixedResCount;
        public uint VariableTableOffset;

        public List<VariableTableEntry> VariableResources = new List<VariableTableEntry>();

        public BIFFile(FileInfo file)
        {
            FilePath = file;
        }

        public class VariableTableEntry
        {
            public BIFFile ParentBIF;
            public uint ResID;
            public uint Offset;
            public uint Size;
            public uint ResType;

            public VariableTableEntry(BIFFile parent)
            {
                ParentBIF = parent;
            }

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

            public bool IsPatched
            {
                get
                {
                    return VariableBIFIndex != VariableResTableIndex;
                }
            }

            public string GetFilename()
            {
                var trueid = (ParentBIF.KeyFileIndex << 20) + VariableResTableIndex;
                var cool = ParentBIF.KeyFile.KeyList[trueid]; //ParentBIF.KeyFile.KeyList.Find(x => x.VariableResTableIndex == VariableResTableIndex && x.VariableBIFIndex == ParentBIF.KeyFileIndex);

                if (cool == null)
                {
                    return null;
                }

                return cool.ResRef;
            }

            public override string ToString()
            {
                return String.Format("{0}{1} | {2}", GetFilename(), ERFUtils.GetExtension((ushort)ResType), IsPatched ? "Patched" : "Main Game");
            }
        }

        public void Decode(Stream stream)
        {
            byte[] buffer = new byte[20];

            stream.Read(buffer, 0, buffer.Length);

            FileType = Encoding.ASCII.GetString(buffer, 0, 4);
            FileVersion = Encoding.ASCII.GetString(buffer, 4, 4);
            VariableResCount = BitConverter.ToUInt32(buffer, 8);
            FixedResCount = BitConverter.ToUInt32(buffer, 12);
            VariableTableOffset = BitConverter.ToUInt32(buffer, 16);

            DecodeVariableTable(stream);
        }

        public void DecodeVariableTable(Stream stream)
        {
            for (int i = 0; i < VariableResCount; i++)
            {
                VariableTableEntry entry = new VariableTableEntry(this);
                byte[] buffer = new byte[16];

                stream.Seek(VariableTableOffset + i * 16, SeekOrigin.Begin);
                stream.Read(buffer, 0, buffer.Length);

                entry.ResID = BitConverter.ToUInt32(buffer, 0);
                entry.Offset = BitConverter.ToUInt32(buffer, 4);
                entry.Size = BitConverter.ToUInt32(buffer, 8);
                entry.ResType = BitConverter.ToUInt32(buffer, 12);

                VariableResources.Add(entry);

                var blah = entry.ToString();
            }
        }
    }
}
