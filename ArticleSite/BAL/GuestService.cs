using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class GuestService
    {
        GenericRepository<PurchaseHistory> historyRepo;
        GenericRepository<User> userRepo;
        GenericRepository<Article> articleRepo;
        GenericRepository<Category> categoryRepo;

        PublisherEntities context;

        public GuestService()
        {
            this.context = new PublisherEntities();
            this.historyRepo = new GenericRepository<PurchaseHistory>(this.context);
            this.userRepo = new GenericRepository<User>(this.context);
            this.articleRepo = new GenericRepository<Article>(this.context);
            this.categoryRepo = new GenericRepository<Category>(this.context);
        }

        public IQueryable<Article> GetArticles()
        {
            return articleRepo.GetAll();
        }

        public Article GetArticleById(int id)
        {
            return articleRepo.GetById(id);
        }
    }
}
