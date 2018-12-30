using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NWNOver
{
    class LimitedStream : Stream
    { 
        Stream InnerStream;
        long Offset;
        public override long Length
        {
            get;
        }

        public LimitedStream(Stream inner, long offset, long length)
        {
            InnerStream = inner;
            Offset = offset;
            Length = length;
        }

        public override bool CanRead
        {
            get
            {
                return InnerStream.CanRead;
            }
        }

        public override bool CanSeek
        {
            get
            {
                return InnerStream.CanSeek;
            }
        }

        public override bool CanWrite
        {
            get
            {
                return InnerStream.CanWrite;
            }
        }

        public override long Position
        {
            get
            {
                return InnerStream.Position - Offset;
            }

            set
            {
                InnerStream.Position = value + Offset;
            }
        }

        public override void Flush()
        {
            InnerStream.Flush();
        }

        public override int Read(byte[] buffer, int offset, int count)
        {
            if (Position + count > Length)
                count -= (int)(Position + count - Length);
            return InnerStream.Read(buffer, offset, count);
        }

        public override long Seek(long offset, SeekOrigin origin)
        {
            switch(origin)
            {
                case (SeekOrigin.Begin):
                    return InnerStream.Seek(Offset + offset, SeekOrigin.Begin);
                case (SeekOrigin.Current):
                    return InnerStream.Seek(Offset + Length - offset, SeekOrigin.Begin);
                case (SeekOrigin.End):
                    return InnerStream.Seek(offset, SeekOrigin.Begin);
            }

            throw new ArgumentException();
        }

        public override void SetLength(long value)
        {
            throw new NotImplementedException();
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            if (Position + count > Length)
                count -= (int)(Position + count - Length);
            InnerStream.Write(buffer, offset, count);
        }
    }
}
