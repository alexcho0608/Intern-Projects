using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL
{
    public class GenericRepository<T> where T : class
    {

        private DbContext context;
        private DbSet<T> Set;

        public GenericRepository(DbContext db)
        {
            this.context = db;
            this.Set = context.Set<T>();
        }

        public T Insert(T elem)
        {
            var result = this.context.Set<T>().Add(elem);
            return result;
        }

        public T GetById(object id)
        {
            return this.Set.Find(id);
        }

        public void Delete(T elem)
        {
            this.context.Set<T>().Remove(elem);
        }

        public IQueryable<T> GetAll()
        {
            return this.context.Set<T>().Select(e => e);
        }


        public void SaveChanges()
        {
            this.context.SaveChanges();
        }
    }
}
