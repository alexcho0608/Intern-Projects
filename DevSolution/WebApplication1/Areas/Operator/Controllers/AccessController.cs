using BLL;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Areas.Operator.Controllers
{
    public class AccessController : Controller
    {
        // GET: Operator/Access
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Register()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult DoLogin(LoginOperatorRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            OperatorServices service = new OperatorServices();
            var result = service.LoginOperator(model);
            if(result.ErrorMessage == OperatorMessage.OK)
            {
                return View("LoginSuccess");
            }
            else
            {
                return View("LoginError");
            }
        }

        public ActionResult DoRegister(RegisterOperatorModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("Index");
            }

            OperatorServices service = new OperatorServices();
            var result = service.RegisterOperator(model);
            if (result == null)
            {
                return View("RegisterError");
            }
            else
            {
                return View("RegisterSuccess");
            }
        }

        public ActionResult Clients()
        {
            OperatorServices service = new OperatorServices();
            var clients = service.GetClients();

            return View(clients);
        }

        public ActionResult ChangeClientState(int id,bool state)
        {
            OperatorServices service = new OperatorServices();
            var result = service.ChangeClientState(id, 1, !state);
            if (result == OperatorMessage.OK)
            {
                return View("Success");
            }
            else
            {
                return View("Fail");
            }
        }

        public ActionResult CreateAccount()
        {
            return View();
        }

        public ActionResult DoCreateAccount()
        {
            return View();
        }
    }
}