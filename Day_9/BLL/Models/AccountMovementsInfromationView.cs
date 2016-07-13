using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class AccountMovementsInfromationView
    {
        public ICollection<AccountMovementInfromationView> Movements { get; set; }

        public ICollection<string> IBANS { get; set; }
    }
}
