using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class RegisterClientModel
    {
        public string Names { get; set; }

        public long EGN { get; set; }
        
        public string Email { get; set; }
        
        public string Login { get; set; }

        public string Password { get; set; }

        public string Address { get; set; }
    }
}
