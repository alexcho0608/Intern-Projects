using BAL.Messages;
using BAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class SubscriberService
    {
        GenericRepository<PurchaseHistory> historyRepo;
        GenericRepository<User> userRepo;
        GenericRepository<Article> articleRepo;
        GenericRepository<Category> categoryRepo;

        PublisherEntities context;

        public SubscriberService()
        {
            this.context = new PublisherEntities();
            this.historyRepo = new GenericRepository<PurchaseHistory>(this.context);
            this.userRepo = new GenericRepository<User>(this.context);
            this.articleRepo = new GenericRepository<Article>(this.context);
            this.categoryRepo = new GenericRepository<Category>(this.context);
        }

        public bool DownloadArticle(string articleName,int userId)
        {
            var result = historyRepo.GetAll().Where(h => h.Article.Name == articleName && h.ClientId == userId);
            return result.Count() > 0;
        }

        public IQueryable<ArticleSearchItem> GetArticlesByName(string name)
        {
            return articleRepo.GetAll().Where(a => a.Name.Contains(name)).Select(e => new ArticleSearchItem() {
                Author = e.User.Login,
                Name = e.Name,
                Price = e.Category.Tax,
                Status = false,
                Category = e.Category.Name
            });
        }

        public string GetArticlePathByName(string name)
        {
            var article = articleRepo.GetAll().Where(a => a.Name == name).FirstOrDefault();
            //Validate right user
            if (article == null)
            {
                return null;
            }
            return article.ArticleText;
        }

        public PurchaseModelResponse PurchaseArticle(PurchaseModelRequest request)
        {
            var article = articleRepo.GetAll().Where(a => a.Name == request.ArticleName).FirstOrDefault();
            var user = userRepo.GetAll().Where(u => u.Login == request.Username).FirstOrDefault();

            PurchaseModelResponse response = new PurchaseModelResponse();

            if (article == null || user == null || article.Category.Tax > user.AccountMoney)
            {
                response.Message = SubscriberMessage.PurchaseFailed;
                return response;
            }

            PurchaseHistory logElement = new PurchaseHistory()
            {
                ArticleId = article.Id,
                ClientId = user.Id,
                Date = DateTime.Now
            };

            historyRepo.Insert(logElement);
            historyRepo.SaveChanges();
            user.AccountMoney -= article.Category.Tax;
            userRepo.SaveChanges();

            response.Message = SubscriberMessage.OK;
            response.Date = DateTime.Now;
            return response;
        }

    }
}
