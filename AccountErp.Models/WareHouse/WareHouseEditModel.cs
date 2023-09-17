using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace AccountErp.Models.WareHouse
{
   public class WareHouseEditModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }
        [Required]
        public string Location { get; set; }
     //   public string CreatedBy { get; set; }
      //  public string UpdatedBy { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
       // public Constants.RecordStatus Status { get; set; }
    }
}
