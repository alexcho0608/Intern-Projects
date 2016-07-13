using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class AccountMovementInfromationView
    {
        public AccountMovement movements { get; set; }
        public int clientId { get; set; }
        public int accountId { get; set; }
    }
}
