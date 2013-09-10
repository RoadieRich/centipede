using System;
using System.IO;
using System.Net.Sockets;

namespace Centipede.Actions
{
    public class SocketStream : Stream
    {
        private Socket _socket;

        public SocketStream(Socket socket)
        {
            this._socket = socket;
        }


        public override void Flush()
        { }

        public override long Seek(long offset, SeekOrigin origin)
        {
            return 0;
        }

        public override void SetLength(long value)
        { }

        public override int Read(byte[] buffer, int offset, int count)
        {
            return this._socket.Receive(buffer, offset, count, SocketFlags.None);
        }

        public override void Write(byte[] buffer, int offset, int count)
        {
            this._socket.Send(buffer, offset, count, SocketFlags.None);
        }

        public override bool CanRead
        {
            get { return this._socket.Connected; }
        }

        public override bool CanSeek
        {
            get { return false; }
        }

        public override bool CanWrite
        {
            get { return this._socket.Connected; }
        }

        public override long Length
        {
            get { throw new InvalidOperationException(); }
        }

        public override long Position
        {
            get { throw new InvalidOperationException(); }
            set { throw new InvalidOperationException(); }
        }
    }
}