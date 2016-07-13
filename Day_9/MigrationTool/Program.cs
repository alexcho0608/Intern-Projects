using DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MigrationTool
{
    class Program
    {

        public static void Seed(BankDBv2Entities e)
        {
            genClientsAndAccounts(e);
        }

        public static void genClientsAndAccounts(BankDBv2Entities e)
        {
            Operator[] oList = new Operator[]{
                    new Operator(){
                        Names = "operator1",
                        Password = "007",
                        Login = "operatorbond1",
                    },
                    new Operator()
                    {
                        Names = "operator2",
                        Password = "007",
                        Login = "operatorbond2",
                    }
                };

            e.Operators.Add(oList[0]);
            e.Operators.Add(oList[1]);
            e.SaveChanges();


            string name = "name",
                address = "haha",
                email = "bond@james.",
                login = "bond",
                passowrd = "jamsesbond",
                iban = "iban";

            Client temp = new Client()
            {
            };

            Account temp2 = new Account()
            {

            };

            for (int i = 0; i < 100000; i++)
            {
                
                temp.Names = "name" + i;
                temp.Address = "haha" + i;
                temp.EGN = 0 + i;
                temp.Email = "bond@james." + i;
                temp.Login = "bond" + i;
                temp.Password = "jamsesbond" + i;

                temp2.Balance = 0 + i;
                temp2.CreatedDate = DateTime.Now;
                temp2.IBAN = "IBAN" + i;
                temp2.CurrencyID = i % 2 + 1;
                temp2.LastUpdateDateTime = DateTime.Now;
                temp2.BankOfficeID = i % 2 + 1;
                temp2.CreateByOperatorID = oList[i % 2].ID;
                temp2.AccountTypeID = i % 3 + 1;

                
                temp.Accounts.Add(temp2);
                e.Clients.Add(temp);
                e.Accounts.Add(temp2);
                e.SaveChanges();
            }
        }

        static void Main(string[] args)
        {


            

            using (BankDBv2Entities context = new BankDBv2Entities())
            {
            }
        }
    }
}
