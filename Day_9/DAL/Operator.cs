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

    public partial class Operator : EntityBase
    {
        public Operator()
        {
            this.Accounts = new HashSet<Account>();
            this.Client2Operator = new HashSet<Client2Operator>();
        }
    
        public int ID { get; set; }
        public string Names { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
    
        public virtual ICollection<Account> Accounts { get; set; }
        public virtual ICollection<Client2Operator> Client2Operator { get; set; }
    }
}
