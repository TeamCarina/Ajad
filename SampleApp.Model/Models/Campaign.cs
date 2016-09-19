
namespace SampleApp.Model.Models
{
   
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Data.Entity.Spatial;

        [Table("Campaign")]
  public partial  class Campaign
    {
            public int Id { get; set; }
            public string UserId { get; set; }
            public string AppId { get; set; }
            public string Title { get; set; }
            public string URL { get; set; }
            public DateTime CreateDate { get; set; }
    }
}
