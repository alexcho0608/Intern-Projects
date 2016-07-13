using BAL.Messages;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL.Models
{
    public class PurchaseModelResponse
    {
        public SubscriberMessage Message { get; set; }

        public DateTime Date { get; set; }
    }
}
