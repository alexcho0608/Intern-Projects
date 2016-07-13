using BAL.Messages;
using BAL.Models;
using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace BAL
{
    public class UserService
    {
        GenericRepository<User> userRepo;
        GenericRepository<Role> roleRepo;
        PublisherEntities dbContext;

        public UserService()
        {
            this.dbContext = new PublisherEntities();
            this.userRepo = new GenericRepository<User>(dbContext);
            this.roleRepo = new GenericRepository<Role>(dbContext);
        }

        private string Encode(string password)
        {
            byte[] encodedPassword = new UTF8Encoding().GetBytes(password);

            byte[] hash = ((HashAlgorithm)CryptoConfig.CreateFromName("MD5")).ComputeHash(encodedPassword);

            string encoded = BitConverter.ToString(hash).ToLower();

            return encoded;
        }

        private ClientMessage ValidateRequestRegister(RegisterUserModelRequest model)
        {
            Match match = (new Regex(@"^([\w\.\-]+)@([\w\-]+)((\.(\w){2,3})+)$")).Match(model.Email);
            if (!match.Success)
            {
                return ClientMessage.EMAILVALIDATIONERROR;
            }

            var clients = userRepo.GetAll();
            var emailResults = clients.Where(client => client.Email == model.Email);
            var loginResults = clients.Where(client => client.Login == model.Login);
            if (emailResults.Count() > 0 || loginResults.Count() > 0) return ClientMessage.EMIALEXISTS | ClientMessage.LOGINEXISTS;

            return ClientMessage.OK;
        }


        public static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }

        public RegisterUserModelResponse RegisterClient(RegisterUserModelRequest model)
        {
            string hashed = Encode(model.Password);

            var response = new RegisterUserModelResponse()
            {
                StringCookie = UserService.RandomString(20),
            };

            var role = roleRepo.GetAll().Where(r => r.Name == model.Role);
            if(role.Count() == 0)
            {
                response.ResponseMessage = ClientMessage.INVALIDROLE;
                return response;
            }

            response.ResponseMessage = ValidateRequestRegister(model);
            response.Role = model.Role;

            User newClient = new User()
            {
                Login = model.Login,
                Password = hashed,
                Email = model.Email,
                AccountMoney = 30,
                RoleId = role.First().Id
            };

            var result = userRepo.Insert(newClient);
            userRepo.SaveChanges();

            return response;
        }

        public LoginUserModelResponse LoginClient(LoginUserModelRequest model)
        {
            var encoded = Encode(model.Password);
            var response = new LoginUserModelResponse()
            {
                StringCookie = UserService.RandomString(20)
            };

            var result = userRepo.GetAll().Where(client => client.Login == model.Login && client.Password == encoded);
            if (result.Count() > 0)
            {
                response.ResponseMessage = ClientMessage.OK;
                response.Role = result.First().Role.Name;
                return response;
            }

            response.ResponseMessage = ClientMessage.LOGINEXISTS;
            return response;
        }
    }
}
