namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    public partial class Status
    {
        public Status()
        {
            Transection = new HashSet<Transection>();
        }

       

        public Guid StatusID { get; set; }

        [StringLength(50)]
        public string StatusKey { get; set; }

        [StringLength(50)]
        public string StatusValue { get; set; }

        [StringLength(50)]
        public string DisplayValue { get; set; }

        public virtual ICollection<Transection> Transection { get; set; }
    }
}
