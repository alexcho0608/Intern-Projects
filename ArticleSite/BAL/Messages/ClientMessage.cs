using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Messages
{
    public enum ClientMessage
    {
        OK = 1,
        LOGINEXISTS = 2,
        EMIALEXISTS = 4,
        DBERROR = 8,
        NAMEEXISTS = 16,
        EMAILVALIDATIONERROR = 32,
        ITEMNOTFOUND = 128,
        LOGINNOTEXIST = 256,
        CATEGORYDOESNOTEXIST = 1024,
        INVALIDROLE = 2048
    }
}
