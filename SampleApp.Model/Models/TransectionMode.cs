namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("TransectionMode")]
    public partial class TransectionMode
    {
        public TransectionMode()
        {
            Transection = new HashSet<Transection>();
        }

        public Guid TransectionModeId { get; set; }

        [StringLength(50)]
        public string Mode { get; set; }

        public virtual ICollection<Transection> Transection { get; set; }
    }
}
