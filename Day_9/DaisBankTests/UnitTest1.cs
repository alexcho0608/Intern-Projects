using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using BLL;
using BLL.Models;
using System.Collections.Generic;
using DAL;

namespace DaisBankTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void CheckWithdrawFromAccountOperation()
        {
            ClientService service = new ClientService();

            WithdrawRequestModel withdraw = new WithdrawRequestModel()
            {
                Amount = 10,
                AccountId = 9469,
                ClientId = 154651
            };

            ClientMessages message = service.Withdraw(withdraw);

            Assert.AreEqual(message, ClientMessages.OK);
        }

        [TestMethod]
        public void CheckDepositIntoAccountOperation()
        {
            ClientService service = new ClientService();

            DepositRequestModel deposit = new DepositRequestModel()
            {
                Amount = 10,
                AccountId = 9469,
                ClientId = 154651
            };

            ClientMessages message = service.DepositIntoAccount(deposit);

            Assert.AreEqual(message, ClientMessages.OK);
        }

        [TestMethod]
        public void CheckAccountMovementQuery()
        {
            ClientService service = new ClientService();

            DepositRequestModel deposit = new DepositRequestModel()
            {
                Amount = 10,
                AccountId = 9469,
                ClientId = 154651
            };

            ClientMessages message = service.DepositIntoAccount(deposit);

            Assert.AreEqual(message, ClientMessages.OK);
        }

        [TestMethod]
        public void GetAccountMovementsTest()
        {
            ClientService service = new ClientService();

            ClientViewModel cl = new ClientViewModel()
            {
                ID = 154651
            };

            ICollection<AccountMovementInfromationView> message = service.GetAccountMovements(cl);

            

            Assert.AreEqual(message.Count, 4);
        }

    }
}
