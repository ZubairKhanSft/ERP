using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Entities
{
    public class ProjectTransaction
    {
        public Guid Id { get; set; }
        public int ProjectId { get; set; }
        public int? InvoiceId { get; set; }
        public int? BillId { get; set; }
        public Constants.ProjectTransactionType TransType { get; set; }
        public Invoice Invoice { get; set; }
        public Bill Bill { get; set; }
    }
}
