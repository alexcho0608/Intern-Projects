using BAL;
using BAL.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ArticleSite.Controllers
{
    public class PublisherController : Controller
    {
        PublisherService service;

        public PublisherController()
        {
            //this.service = new PublisherService();
        }

        // GET: Publisher
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Add(PublishArticleModel request,HttpPostedFileBase file)
        {
            service = new PublisherService();
            if (!ModelState.IsValid)
            {
                ViewBag.Message = "Inaccurate request";
                return View("Index");
            }
            
            string path = "";
            if (file.ContentLength > 0)
            {
                var fileName = Path.GetFileName(file.FileName);
                path = Path.Combine(Server.MapPath("~/App_Data/uploads"), fileName);
                file.SaveAs(path);
            }

            service.AddArticle(request, path);

            ViewBag.Message = "Successful Import";
            return RedirectToAction("Index");
        }
    }
}