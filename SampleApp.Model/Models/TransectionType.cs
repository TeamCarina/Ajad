namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransectionType")]
    public partial class TransectionType
    {
        public TransectionType()
        {
            Transection = new HashSet<Transection>();
        }

        public Guid TransectionTypeId { get; set; }

        [StringLength(50)]
        public string Type { get; set; }

        public virtual ICollection<Transection> Transection { get; set; }
    }
}
