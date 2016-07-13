using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using Dapper;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class DapperGenericRepository<T> where T : EntityBase
    {

        private IDbConnection db;//= new SqlConnection("BankDBv2Entities");
        private string _tableName;

        internal IDbConnection Connection
        {
            get
            {
                return new SqlConnection("Data Source=.;Initial Catalog=BankDBv2;Integrated Security=True");
            }
        }

        public DapperGenericRepository(string tablename)
        {
            this._tableName = tablename;
        }

        internal virtual dynamic Mapping(T item)
        {
            return item;
        }


        public virtual void Update(T item)
        {
            using (IDbConnection cn = Connection)
            {
                var parameters = (object)Mapping(item);
                var properties = item.GetType().GetProperties();
                foreach (var property in properties)
                {
                    if (property.GetGetMethod().IsVirtual)
                    {

                    };
                }
                cn.Open();
                cn.Update(_tableName, parameters);
            }
        }

        public virtual T FindByID(int id)
        {
            T item = default(T);

            using (this.db = Connection)
            {
                db.Open();
                item = db.Query<T>("SELECT * FROM " + _tableName + " WHERE ID=@ID", new { ID = id }).SingleOrDefault();
            }

            return item;
        }

        public T Insert(T elem)
        {
            using (this.db = Connection)
            {
                this.db.Open();
                var result = this.db.Insert<T>(_tableName, (object)Mapping(elem));
                return result;
            }
        }

        public int Delete(T elem)
        {
            using (this.db = Connection)
            {
                db.Open();
                var result = db.Execute("DELETE FROM " + _tableName + " WHERE ID=@ID", new { ID = elem.ID });
                return result;
            }
        }

        public IEnumerable<T> GetAll()
        {
            IQueryable<T> items;
            using (db = Connection)
            {
                db.Open();
                items = db.Query<T>("SELECT * FROM " + _tableName).AsQueryable();
            }
            return items;
        }

    }
}
