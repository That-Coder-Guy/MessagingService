using Microsoft.AspNetCore.Authentication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSocketUtilities
{
    public enum PacketId
    {
        PublicKeyHandshake,
        CreateAccount
    }
}
