using BLL;
using BLL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BLLApplication
{
    class Program
    {
        static void Main(string[] args)
        {
            try
            {


                //ClientService cs = new ClientService();
                //var reuslt = cs.RegisterClient(new BLL.Models.RegisterClientModel()
                //{
                //    Login = "hahaee",
                //    Names = "hahaee",
                //    Password = "hahaee",
                //    Email = "haha@haha.bg",
                //    Address = "hahaee",
                //    EGN = 10033300030
                //});

                //if (reuslt.ErrorMessage == ClientMessages.OK)
                //{
                //    var guid = cs.LoginClient(new LoginClientRequestModel()
                //    {
                //        LoginName = "hahaee",
                //        LoginPassword = "hahaee"
                //    });
                //    Console.WriteLine(guid);
                //}

                //ClientService service = new ClientService();

                //LoginClientRequestModel model = new LoginClientRequestModel()
                //{
                //    LoginName = "bond0",
                //    LoginPassword = "jamsesbond0"
                //};
                //service.LoginClient(model);
                OperatorServices opServices = new OperatorServices();

                //RegisterOperatorModel modelReg = new RegisterOperatorModel()
                //{
                //    Login = "operatorTest",
                //    Names = "operatorTest",
                //    Password = "007"
                //};

                //opServices.RegisterOperator(modelReg);
                //IEnumerable<CurrentCurrencies> rates = opServices.GetRates();

                //Account acc = opServices.GetAccountBalance("IBAN1");


                //opServices.ChangeClientState(154651, 929, true);

                AccountCreateRequestModel model = new AccountCreateRequestModel()
                {
                    AccountTypeId = 1,
                    BankOfficeId = 1,
                    ClientId = 154651,
                    OperatorId = 921,
                    IBAN = "testtesttest",
                    CurrencyId = 2,
                };

                opServices.CreateAccount(model);

                //LoginOperatorRequestModel model2 = new LoginOperatorRequestModel()
                //{
                //    LoginName = "operatorbond1",
                //    LoginPassword = "007"
                //};
                //opServices.LoginOperator(model2);

            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Console.WriteLine("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:",
                        eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Console.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            
        }
    }
}
