using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLL
{
    public class ClientOperationsRepository<T> : GenericRepository<T>
        where T : class
    {
        public ClientOperationsRepository(DbContext context)
            :base(context)
        {

        }

        
    }
}
