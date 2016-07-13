//------------------------------------------------------------------------------
// <auto-generated>
//    This code was generated from a template.
//
//    Manual changes to this file may cause unexpected behavior in your application.
//    Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace DAL
{
    using System;
    using System.Collections.Generic;
    
    public partial class Account : EntityBase
    {
        public Account()
        {
            this.AccountMovements = new HashSet<AccountMovement>();
        }
    
        public int ID { get; set; }
        public decimal Balance { get; set; }
        public int ClientID { get; set; }
        public int CurrencyID { get; set; }
        public string IBAN { get; set; }
        public int BankOfficeID { get; set; }
        public System.DateTime CreatedDate { get; set; }
        public int CreateByOperatorID { get; set; }
        public System.DateTime LastUpdateDateTime { get; set; }
        public int AccountTypeID { get; set; }
    
        public virtual AccountType AccountType { get; set; }
        public virtual BankOffice BankOffice { get; set; }
        public virtual Client Client { get; set; }
        public virtual Currency Currency { get; set; }
        public virtual Operator Operator { get; set; }
        public virtual ICollection<AccountMovement> AccountMovements { get; set; }
    }
}
