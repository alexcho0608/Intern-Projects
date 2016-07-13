using BAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BAL
{
    public class PublisherService
    {
        PublisherEntities context;
        GenericRepository<Article> articleRepo;
        GenericRepository<Category> categoryRepo;

        public PublisherService()
        {
            this.context = new PublisherEntities();
            this.articleRepo = new GenericRepository<Article>(context);
            this.categoryRepo = new GenericRepository<Category>(context);
        }

        public bool AddArticle(PublishArticleModel model, string path)
        {
            var category = categoryRepo.GetAll().Where(c => c.Name == model.Category).FirstOrDefault();
            if (category == null) return false;
            var article = new Article()
            {
                CategoryId = category.Id,
                Name = model.Name,
                Date = DateTime.Now,
                Description = model.Description,
                ArticleText = path,
                AuthorId = 1
            };
            articleRepo.Insert(article);
            articleRepo.SaveChanges();
            return true;
        }
    }
}
