using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class WithdrawRequestModel
    {
        public decimal Amount { get; set; }

        public int ClientId { get; set; }

        public int AccountId { get; set; }
    }
}
