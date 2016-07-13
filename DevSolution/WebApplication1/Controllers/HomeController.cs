using BLL;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class HomeController : Controller
    {
        private void SetCookie(string guid)
        {
            //HttpCookie cookie = new HttpCookie("Authorization");
        }

        // GET: Home
        public ActionResult Index()
        {
            IndexResponse response = new IndexResponse();
            return View(response);
        }


        public ActionResult Login()
        { 
            return View();
        }

        [HttpPost]
        public JsonResult DoLogin(LoginClientRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return Json("Error with model");
            }
            ClientService clService = new ClientService();
            
            var result = clService.LoginClient(model);
            IndexResponse res = new IndexResponse();
            res.Response = result.ErrorMessage.ToString();
            SetCookie(result.AccessToken);

            return Json(res.Response);
        }

        public ActionResult Register()
        {
            return View();
        }

        [HttpGet]
        public JsonResult CheckUser()
        {
            var username = this.Request.Params["username"];
            ClientService service = new ClientService();
            var result = service.GetClient(username) ? "exists" : "ok";
            return Json(result, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult DoRegister(RegisterClientModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            ClientService clService = new ClientService();
            var result = clService.RegisterClient(model);
            IndexResponse res = new IndexResponse()
            {
                Response = result.ErrorMessage.ToString()
            };
            return View("Index", res);
        }
    }
}