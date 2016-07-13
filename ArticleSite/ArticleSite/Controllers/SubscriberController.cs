using BAL;
using BAL.Messages;
using BAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleSite.Controllers
{
    public class SubscriberController : Controller
    {
        SubscriberService service;

        public SubscriberController()
        {
            this.service = new SubscriberService();
        }

        // GET: Subscriber
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Search(string search)
        {
            var articlesResult = service.GetArticlesByName(search).ToList();

            return PartialView("ArticleSearchPartialView",articlesResult);
        }

        public FileResult Download(string articleName)
        {
            string article = service.GetArticlePathByName(articleName);
            if(article != null)
            {
                byte[] fileBytes = System.IO.File.ReadAllBytes(article);
                string fileName = article.Split('\\').Last();
                return File(fileBytes, System.Net.Mime.MediaTypeNames.Application.Octet, fileName);
            }
            return null;
        }

        public ActionResult Order(string articleName)
        {
            var model = new PurchaseModelRequest()
            {
                ArticleName = articleName,
                Username = "alex1"
            };

            var flag = service.PurchaseArticle(model);
            if (flag.Message != SubscriberMessage.OK) ViewBag.Message = "Purchase was unsuccessful";
            return View("Index");
        }
    }
}