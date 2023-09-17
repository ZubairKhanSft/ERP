using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class Project
    {
        public int Id { get; set; }
        public string ProjectName { get; set; }
        public int CustomerId { get; set; }
        public Constants.RecordStatus Status{ get; set; }
        public DateTime CreatedOn { get; set; }
        public string CreatedBy { get; set; }
        public DateTime? UpdatedOn { get; set; }
        public string UpdatedBy { get; set; }
        public Customer Customer { get; set; }
        public ICollection<ProjectTransaction> ProjectTransaction { get; set; }
    }
}
