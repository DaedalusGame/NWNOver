using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    class FileReference
    {
        public FileInfo FilePath;
        public uint BeginOffset;
        public uint Length;
        Stream CurrentReadStream;

        public uint EndOffset
        {
            get
            {
                return BeginOffset + Length;
            }
        }

        public Stream GetReadStream()
        {
            if(CurrentReadStream == null)
            {
                CurrentReadStream = FilePath.OpenRead();
            }

            CurrentReadStream.Seek(BeginOffset, SeekOrigin.Begin);

            return new LimitedStream(CurrentReadStream,BeginOffset,Length);
        }
    }
}
