using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public enum ClientMessages
    {
        OK = 1,
        LOGINEXISTS = 2,
        LOGINNOTEXIST = 256,
        EMIALEXISTS = 4,
        DBERROR = 8,
        NAMEEXISTS = 16,
        EMAILVALIDATIONERROR = 32,
        CLIENTISFROZEN = 64,
        InsufficientAmount = 128
    }
}
