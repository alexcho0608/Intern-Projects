using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DapperExtensions.Mapper;
namespace DAL
{

    public class ClientMapper : ClassMapper<Client>
    {
        public ClientMapper()
        {
            Table("Client");
            base.Map(m => m.Accounts).Ignore();
            base.Map(m => m.AccountMovements).Ignore();
            base.Map(m => m.Client2Operator).Ignore();
            base.AutoMap();
        }
    }
}
