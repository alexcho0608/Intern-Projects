using BLL;
using BLL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;

namespace WebApplication1.Controllers
{
    public class AccountController : Controller
    {
        // GET: Account
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult Movements(int id = -1)
        {
            ClientService service = new ClientService();
            ClientViewModel model = new ClientViewModel()
            {
                ID = id
            };

            var ibans = service.GetIBANS(id);
            var movements = service.GetAccountMovements(model);

            AccountMovementsInfromationView responseModel = new AccountMovementsInfromationView()
            {
                Movements = movements,
                IBANS = ibans
            };

            return View(responseModel);
        }

        public ActionResult MovementsByIban(string IBAN)
        {
            ClientService service = new ClientService();
            var movements = service.GetAccountMovementsByIBAN(IBAN);

            return PartialView("AccountMovementTablePartial",movements);
        }

        public ActionResult Deposit(DepositRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("DepositFail");
            }

            ClientService service = new ClientService();
            var result = service.DepositIntoAccount(model);
            if (result == ClientMessages.OK)
                return View("DepositSuccess");
            else
                return View("DepositFail");
        }

        public ActionResult Withdraw(WithdrawRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("DepositFail");
            }

            ClientService service = new ClientService();
            var result = service.Withdraw(model);
            if (result == ClientMessages.OK)
                return View("WithdrawSuccess");
            else
                return View("WithdrawFail");
        }

        public ActionResult TransferBetweenAccounts(TransferRequestModel model)
        {
            if (!ModelState.IsValid)
            {
                return View("TransferFail");
            }

            ClientService service = new ClientService();
            var result = service.Transfer(model);

            if (result == ClientMessages.OK)
                return View("TransferSuccess");
            else
                return View("TransferFail");
        }
    }
}