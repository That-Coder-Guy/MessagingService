using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketUtilities
{
    public interface IPacket
    {
        public PacketId Id { get; }
    }
}
