using BLL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Transactions;

namespace BLL
{
    public class ClientService
    {
        GenericRepository<Client> clientRepo;
        GenericRepository<Account> accountRepo;
        GenericRepository<AccountMovement> accountMovementRepo;
        BankDBv2Entities context;

        public ClientService()
        {
            context = new BankDBv2Entities();
            clientRepo = new GenericRepository<Client>(context);
            accountRepo = new GenericRepository<Account>(context);
            accountMovementRepo = new GenericRepository<AccountMovement>(context);
        }

        private string Encode(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash).ToLower();

            return encoded;
        }

        private ClientMessages ValidateRequestRegister(RegisterClientModel model)
        {
            Match match = (new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")).Match(model.Email);
            if (!match.Success)
            {
                return ClientMessages.EMAILVALIDATIONERROR;
            }

            var clients = clientRepo.GetAll();
            var emailResults = clients.Where(client => client.Email == model.Email);
            var loginResults = clients.Where(client => client.Login == model.Login);
            if (emailResults.Count() > 0 || loginResults.Count() > 0) return ClientMessages.EMIALEXISTS | ClientMessages.LOGINEXISTS;

            return ClientMessages.OK;
        }

        public LoginClientResponseModel RegisterClient(RegisterClientModel model)
        {
            string hashed = Encode(model.Password).Substring(0, 18);


            var response = new LoginClientResponseModel()
            {
                AccessToken = Guid.NewGuid().ToString()
            };

            response.ErrorMessage = ValidateRequestRegister(model);

            Client newClient = new Client()
            {
                Names = model.Names,
                EGN = model.EGN,
                Password = hashed,
                Login = model.Login,
                Email = model.Email,
                Address = model.Address
            };

            var result = clientRepo.Insert(newClient);
            clientRepo.SaveChanges();

            return response;
        }

        public LoginClientResponseModel LoginClient(LoginClientRequestModel model)
        {
            var encoded = model.LoginPassword; //Encode(model.LoginPassword).Substring(0,18);
            var response = new LoginClientResponseModel()
            {
                AccessToken = Guid.NewGuid().ToString()
            };

            var result = clientRepo.GetAll().Where(client => client.Login == model.LoginName && client.Password == encoded);
            if (result.Count() > 0)
            {
                var client = result.First();
                if (client.State == true)
                {
                    response.ErrorMessage = ClientMessages.CLIENTISFROZEN;
                    return response;
                }

                response.ErrorMessage = ClientMessages.OK;
                return response;
            }

            response.ErrorMessage = ClientMessages.LOGINNOTEXIST;
            return response;
        }

        //public IQueryable<Account> GetByIBAN(string IBAN)
        //{
        //    var result = clientRepo.GetAll().Where(cl => cl.Accounts.Where(a => a.IBAN == IBAN));

        //}


        public ICollection<string> GetIBANS(int clientId)
        {
            return accountRepo.GetAll().Where(a => a.ClientID == clientId).Select(a => a.IBAN).ToList();
        }

        public ICollection<AccountMovementInfromationView> GetAccountMovements(ClientViewModel cl)
        {


            var movements = (
                from clients in clientRepo.GetAll()
                join accounts in accountRepo.GetAll()
                on clients.ID equals accounts.ClientID
                join accountMovements in accountMovementRepo.GetAll()
                on accounts.ID equals accountMovements.AccountID
                into clientToAccount
                from accountMovements in clientToAccount.DefaultIfEmpty()
                where cl.ID == clients.ID
                select new AccountMovementInfromationView
                {
                    movements = accountMovements == null ? null : accountMovements,
                    accountId = accounts.ID,
                    clientId = clients.ID
                }
            ).ToList();

            return movements;
        }

        public ICollection<AccountMovementInfromationView> GetAccountMovementsByIBAN(string iban)
        {
            var movements = (
                from clients in clientRepo.GetAll()
                join accounts in accountRepo.GetAll()
                on clients.ID equals accounts.ClientID
                join accountMovements in accountMovementRepo.GetAll()
                on accounts.ID equals accountMovements.AccountID
                into clientToAccount
                from accountMovements in clientToAccount.DefaultIfEmpty()
                where accounts.IBAN == iban
                select new AccountMovementInfromationView
                {
                    movements = accountMovements == null ? null : accountMovements,
                    accountId = accounts.ID,
                    clientId = clients.ID
                }
            ).ToList();


            return movements;
        }

        public ClientMessages DepositIntoAccount(DepositRequestModel model)
        {
            var accountMovement = new AccountMovement()
            {
                ClientID = model.ClientId,
                AccountID = model.AccountId,
                Amount = model.Amount,
                OperationTypeID = 1,
                EntryDate = DateTime.Today
            };

            var account = accountRepo.GetAll().First(ac => ac.ID == model.AccountId);
            if (account != null)
            {
                account.Balance += model.Amount;
            }

            this.ServiceSaveChanges(accountMovement, account);

            //accountMovementRepo.Insert(accountMovement);
            //accountMovementRepo.SaveChanges();
            //accountRepo.SaveChanges();

            return ClientMessages.OK;
        }

        public ClientMessages Withdraw(WithdrawRequestModel model)
        {
            var account = accountRepo.GetAll().First(ac => ac.ID == model.AccountId);
            if (account.Balance - model.Amount < 0)
                return ClientMessages.InsufficientAmount;

            account.Balance -= model.Amount;

            var accountMovement = new AccountMovement()
            {
                ClientID = model.ClientId,
                AccountID = model.AccountId,
                Amount = model.Amount,
                OperationTypeID = 2,
                EntryDate = DateTime.Today
            };


            this.ServiceSaveChanges(accountMovement, account);

            return ClientMessages.OK;
        }

        public void ServiceSaveChanges(AccountMovement movement, Account account)
        {

            using (var dbcxtransaction = context.Database.BeginTransaction())
            {
                try
                {
                    this.accountMovementRepo.Insert(movement);

                    this.accountMovementRepo.SaveChanges();
                    this.accountRepo.SaveChanges();

                    dbcxtransaction.Commit();
                }
                catch (System.Data.Entity.Validation.DbEntityValidationException e)
                {
                    string rs = "";
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        rs = string.Format("Entity of type \"{0}\" in state \"{1}\" has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        Console.WriteLine(rs);

                        foreach (var ve in eve.ValidationErrors)
                        {
                            rs += "<br />" + string.Format("- Property: \"{0}\", Error: \"{1}\"", ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw new Exception(rs);
                    dbcxtransaction.Rollback();
                }
            }

        }


        public bool GetClient(string username)
        {
            return clientRepo.GetAll().Where(cl => cl.Login == username).Count() > 0;
        }

        public ClientMessages Transfer(TransferRequestModel model)
        {

            WithdrawRequestModel withdraw = new WithdrawRequestModel()
            {
                AccountId = model.FromAccountId,
                ClientId = model.ClientId,
                Amount = model.Amount
            };

            DepositRequestModel deposit = new DepositRequestModel()
            {
                ClientId = model.ClientId,
                AccountId = model.ToAccountId,
                Amount = model.Amount
            };


            ClientMessages withdrawMessage = Withdraw(withdraw);

            if (withdrawMessage != ClientMessages.OK) return withdrawMessage;

            ClientMessages depositMessage = DepositIntoAccount(deposit);
            if (depositMessage != ClientMessages.OK) return depositMessage;


            return ClientMessages.OK;


        }
    }
}
