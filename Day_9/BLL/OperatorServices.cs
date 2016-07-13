using BLL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace BLL
{
    public class OperatorServices
    {

        DapperGenericRepository<Operator> operatorRepo;
        DapperGenericRepository<Client> clientRepo;
        DapperGenericRepository<Account> accountRepo;
        DapperGenericRepository<AccountMovement> accountMovementRepo;
        DapperGenericRepository<Client2Operator> clientToOperatorRepo;
        DapperGenericRepository<CurrenciesExchangeRate> ratesRepo;

        public OperatorServices()
        {
            operatorRepo = new DapperGenericRepository<Operator>("Operators");
            accountRepo = new DapperGenericRepository<Account>("Account");
            clientRepo = new DapperGenericRepository<Client>("Clients");
            clientToOperatorRepo = new DapperGenericRepository<Client2Operator>("Client2Operator");
            accountMovementRepo = new DapperGenericRepository<AccountMovement>("AccountMovement");
            ratesRepo = new DapperGenericRepository<CurrenciesExchangeRate>("CurrenciesExchangeRate");
        }

        private string Encode(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash).ToLower();

            return encoded;
        }

        private OperatorMessage ValidatOperatorRegisterRequest(RegisterOperatorModel model)
        {

            var clients = operatorRepo.GetAll();
            var loginResults = clients.Where(client => client.Login == model.Login);
            if (loginResults.Count() > 0) return  OperatorMessage.LOGINEXISTS;

            return OperatorMessage.OK;
        }

        public LoginOperatorResponseModel RegisterOperator(RegisterOperatorModel model)
        {
            string hashed = Encode(model.Password).Substring(0, 18);


            var response = new LoginOperatorResponseModel()
            {
                AccessToken = Guid.NewGuid().ToString()
            };

            response.ErrorMessage = ValidatOperatorRegisterRequest(model);

            Operator  newOperator = new Operator()
            {
                Names = model.Names,
                Login = model.Login,
                Password = hashed,
            };

            var result = operatorRepo.Insert(newOperator);
            if (result == null) return null;

            return response;
        }

        public IList<Client> GetClients()
        {
            return clientRepo.GetAll().ToList();
        }

        public LoginOperatorResponseModel LoginOperator(LoginOperatorRequestModel model)
        {
            var encoded = Encode(model.LoginPassword).Substring(0, 18);
            var response = new LoginOperatorResponseModel()
            {
                AccessToken = Guid.NewGuid().ToString()
            };

            var result = operatorRepo.GetAll().Where(client => client.Login == model.LoginName && client.Password == encoded);
            if (result.Count() > 0)
            {
                var operatorResult = result.First();

                response.ErrorMessage = OperatorMessage.OK;
                return response;
            }

            response.ErrorMessage = OperatorMessage.LOGINEXISTS;
            return response;
        }

        public OperatorMessage ChangeClientState(int clientId,int operatorId, bool state)
        {
            Client cl = clientRepo.FindByID(clientId);

            cl.State = state;
            clientRepo.Update(cl);

            Client2Operator cl2Op = new Client2Operator()
            {
                ClientID = clientId,
                OperatorID = operatorId,
                EntryDate = DateTime.Today,
                NewStatus = state
            };

            clientToOperatorRepo.Insert(cl2Op);

            return OperatorMessage.OK;
        }

        public IEnumerable<CurrentCurrencies> GetRates()
        {
            return ratesRepo.GetAll().GroupBy(p => new { p.FromCurrencyID, p.ToCurrencyID })
                               .Select(cl =>
                                   new CurrentCurrencies()
                                   {
                                       FromCurrency = cl.First().FromCurrencyID,
                                       ToCurrency = cl.First().ToCurrencyID,
                                       EntryDate = cl.Max(e => e.EntryDate)
                                   }
                                );

        }

        public OperatorMessage CreateAccount(AccountCreateRequestModel model)
        {

            Account newAccount = new Account()
            {
                AccountTypeID = model.AccountTypeId,
                BankOfficeID = model.BankOfficeId,
                IBAN = model.IBAN,
                CreatedDate = DateTime.Now,
                CreateByOperatorID = model.OperatorId,
                CurrencyID = model.CurrencyId,
                ClientID = model.ClientId
            };

            CultureInfo standardizedCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            standardizedCulture.DateTimeFormat.DateSeparator = "-";
            standardizedCulture.DateTimeFormat.LongDatePattern = "MM-dd-yyyy hh:mm:ss";
            standardizedCulture.DateTimeFormat.FullDateTimePattern = "MM-dd-yyyy hh:mm:ss";
            standardizedCulture.DateTimeFormat.ShortDatePattern = "MM-dd-yyyy";
            Thread.CurrentThread.CurrentCulture = standardizedCulture;
            Thread.CurrentThread.CurrentUICulture = standardizedCulture;

            accountRepo.Insert(newAccount);

            standardizedCulture = (CultureInfo)CultureInfo.CurrentCulture.Clone();
            standardizedCulture.DateTimeFormat.DateSeparator = "-";
            standardizedCulture.DateTimeFormat.LongDatePattern = "d/M/yyyy hh:mm:ss";
            standardizedCulture.DateTimeFormat.FullDateTimePattern = "d/M/yyyy hh:mm:ss";
            standardizedCulture.DateTimeFormat.ShortDatePattern = "d/M/yyyy";
            Thread.CurrentThread.CurrentCulture = standardizedCulture;
            Thread.CurrentThread.CurrentUICulture = standardizedCulture;

            return OperatorMessage.OK;
        }

        public Account GetAccountBalance(string IBAN)
        {
            IEnumerable<Account> accounts = accountRepo.GetAll();
            var account = accounts.FirstOrDefault(e => e.IBAN.Trim() == IBAN);
            return account;
        }
    }
}
