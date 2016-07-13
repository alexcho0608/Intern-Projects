using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class CurrentCurrencies
    {
        public int FromCurrency { get; set; }
        public int ToCurrency { get; set; }
        public DateTime EntryDate { get; set; }
    }
}
