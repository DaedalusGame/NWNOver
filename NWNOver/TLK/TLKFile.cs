using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver.TLK
{
    public class TLKLine
    {
        public TLKLine()
        {
            String = String.Empty;
            SoundResRef = String.Empty;
        }

        public uint Flags
        {
            get;
            set;
        }

        public uint VolumeVariance
        {
            get;
            set;
        }

        public uint PitchVariance
        {
            get;
            set;
        }

        public float SoundLength
        {
            get;
            set;
        }

        public string SoundResRef
        {
            get;
            set;
        }

        public string String
        {
            get;
            set;
        }

        public bool HasString
        {
            get
            {
                return (Flags & 0x0001) > 0;
            }
            set
            {
                if (HasString != value)
                    Flags ^= 0x0001;
            }
        }

        public bool HasSoundResRef
        {
            get
            {
                return (Flags & 0x0002) > 0;
            }
            set
            {
                if(HasSoundResRef != value)
                    Flags ^= 0x0002;
            }
        }

        public bool HasSoundLength
        {
            get
            {
                return (Flags & 0x0004) > 0;
            }
            set
            {
                if (HasSoundLength != value)
                    Flags ^= 0x0004;
            }
        }
    }

    public class TLKFile : IListSource
    {
        public const uint UserStrRefFlag = 0x01000000;

        public string Name;
        string FileType = "TLK ";
        string FileVersion = "V3.0";
        uint LanguageId;     
        uint StringEntriesOffset;

        public uint StringCount
        {
            get
            {
                return (uint)Lines.Count;
            }
        }

        BindingList<TLKLine> Lines = new BindingList<TLKLine>();

        public bool ContainsListCollection
        {
            get
            {
                return true;
            }
        }

        public TLKFile(string name)
        {
            Name = name;
        }

        public TLKLine Get(uint strref)
        {
            if(strref >= Lines.Count)
                return null;
            return Lines[(int)strref];
        }

        public void Read(Stream stream)
        {
            BinaryReader reader = new BinaryReader(stream, Encoding.ASCII);
            FileType = new String(reader.ReadChars(4));
            FileVersion = new String(reader.ReadChars(4));
            LanguageId = reader.ReadUInt32();
            uint strCount = reader.ReadUInt32();
            StringEntriesOffset = reader.ReadUInt32();

            for(int i = 0; i < strCount; i++)
            {
                TLKLine line = new TLKLine();
                line.Flags = reader.ReadUInt32();
                line.SoundResRef = new String(reader.ReadChars(16));
                line.VolumeVariance = reader.ReadUInt32();
                line.PitchVariance = reader.ReadUInt32();
                uint stringOffset = reader.ReadUInt32();
                uint stringSize = reader.ReadUInt32();
                line.SoundLength = reader.ReadSingle();
                if (line.HasString)
                {
                    long resetPos = stream.Position;
                    stream.Position = StringEntriesOffset + stringOffset;
                    line.String = new String(reader.ReadChars((int)stringSize)); //This line will crash if a string is several million characters long.
                    stream.Position = resetPos;
                }
                else
                {
                    line.String = String.Empty;
                }
                Lines.Add(line);
            }

            stream.Close();
        }

        public void Write(Stream stream)
        {
            BinaryWriter writer = new BinaryWriter(stream, Encoding.ASCII);
            writer.Write(FileType.ToCharArray(0,4));
            writer.Write(FileVersion.ToCharArray(0, 4));
            writer.Write(LanguageId);
            writer.Write(StringCount);
            long stringEntriesOffset = stream.Position;
            writer.Write((uint)0);

            Queue<long> offsetFields = new Queue<long>();

            foreach(TLKLine line in Lines)
            {
                writer.Write(line.Flags);
                writer.Write(line.SoundResRef.PadRight(16, (char)0).ToCharArray(0, 16));
                writer.Write(line.VolumeVariance);
                writer.Write(line.PitchVariance);
                offsetFields.Enqueue(stream.Position);
                writer.Write((uint)0);
                writer.Write((uint)line.String.Length);
                writer.Write(line.SoundLength);
            }

            StringEntriesOffset = (uint)stream.Position;
            long resetPos = stream.Position;
            stream.Position = stringEntriesOffset;
            writer.Write(StringEntriesOffset);
            stream.Position = resetPos;

            for (int i = 0; i < Lines.Count; i++)
            {
                TLKLine line = Lines[i];
                resetPos = stream.Position;
                stream.Position = offsetFields.Dequeue();
                if (line.HasString)
                    writer.Write((uint)(resetPos - StringEntriesOffset));
                stream.Position = resetPos;
                if (line.HasString)
                    writer.Write(line.String.ToCharArray());
            }

            writer.Flush();
            stream.Close();
        }

        public void AddLine()
        {
            Lines.Add(new TLKLine());
        }

        public void AddTextLine(string text)
        {
            var line = new TLKLine();
            line.String = text;
            line.HasString = true;
            Lines.Add(line);
        }

        public IList GetList()
        {
            return Lines;
        }

        public void Resize(int newRows)
        {
            Lines.RaiseListChangedEvents = false;
            while(StringCount < newRows)
            {
                Lines.Add(new TLKLine());
            }
            while (StringCount > newRows)
            {
                Lines.RemoveAt(Lines.Count - 1);
            }
            Lines.RaiseListChangedEvents = true;
            Lines.ResetBindings();
        }
    }
}
