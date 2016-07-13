using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL.Models
{
    public class AccountCreateRequestModel
    {
        public int CurrencyId { get; set; }

        public string IBAN { get; set; }

        public int BankOfficeId { get; set; }

        public int AccountTypeId { get; set; }

        public int OperatorId { get; set; }

        public int ClientId { get; set; }

    }
}
