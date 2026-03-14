using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ChattyClient.Net.IO
{
    class PacketBuilder
    {
        MemoryStream _ms;
        public PacketBuilder()
        {
            _ms = new MemoryStream();
        }
        public void WriteOpCode (byte opcode)
        {
            _ms.WriteByte (opcode);
        }
        public void WriteMessage (string msg)
        {
            // write the byte-length + bytes (use ASCII for compatibility with existing code)
            var payload = Encoding.ASCII.GetBytes(msg ?? string.Empty);
            var lenBytes = BitConverter.GetBytes(payload.Length);
            _ms.Write(lenBytes, 0, lenBytes.Length);
            _ms.Write(payload, 0, payload.Length);
        }
        public byte[] GetPacketBytes()
        {
            return _ms.ToArray();
        } 
    }
}
