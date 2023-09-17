using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.Reconciliation
{
   public class ReconciliationEditModel
    {
        public int Id { get; set; }
        public int BankAccountId { get; set; }
        public DateTime? ReconciliationDate { get; set; }
        public Decimal StatementBalance { get; set; }
        public Decimal IcloseBalance { get; set; }

        public bool IsReconciliation { get; set; }

        public int ReconciliationStatus { get; set; }
        // public BankAccount bank { get; set; }

    }
}
