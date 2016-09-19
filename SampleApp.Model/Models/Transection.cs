namespace SampleApp.Model.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

    [Table("Transection")]
    public partial class Transection
    {
        public Guid TransectionId { get; set; }

        [StringLength(200)]
        public string Remarks { get; set; }

        public decimal? Amount { get; set; }

        public DateTime? TransectionDate { get; set; }

        public Guid? StatusId { get; set; }

        public Guid? AccountId { get; set; }

        public Guid? TransectionTypeId { get; set; }

        public Guid? TransectionModeId { get; set; }

        public virtual Account Account { get; set; }

        public virtual Status Status { get; set; }

        public virtual TransectionMode TransectionMode { get; set; }

        public virtual TransectionType TransectionType { get; set; }
    }
}
