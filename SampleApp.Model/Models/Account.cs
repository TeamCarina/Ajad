namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;

    [Table("Account")]
    public partial class Account
    {
        public Account()
        {
            Transection = new HashSet<Transection>();
        }

        public Guid AccountId { get; set; }

        [StringLength(50)]
        public string AccountNumber { get; set; }

        public int? PIN { get; set; }

        public DateTime? CreatedDate { get; set; }

        public decimal? CurrentBalance { get; set; }

        [StringLength(128)]
        public string UserId { get; set; }

        public virtual AspNetUsers AspNetUsers { get; set; }

        public virtual ICollection<Transection> Transection { get; set; }

       

      
    }
}