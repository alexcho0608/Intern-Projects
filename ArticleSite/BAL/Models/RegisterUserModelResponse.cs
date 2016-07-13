using BAL.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class RegisterUserModelResponse
    {
        public string StringCookie { get; set; }

        public ClientMessage ResponseMessage { get; set; }

        public string Role { get; set; }
    }
}
