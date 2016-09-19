namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("FailedEmail")]
    public partial class FailedEmail
    {
        [Key]
        public Guid FailEmailID { get; set; }

        [StringLength(100)]
        public string Email { get; set; }

        [StringLength(200)]
        public string Subject { get; set; }

        public string Body { get; set; }
        
    }
}
