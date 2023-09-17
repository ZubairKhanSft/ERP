using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class Screen
    {
        public int Id { get; set; }
       
        public string ScreenName { get; set; }
        public string ScreenCode { get; set; }
        public Constants.RecordStatus Status { get; set; }
        public DateTime CreatedOn { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string ScreenUrl { get; set; }
    }
}
