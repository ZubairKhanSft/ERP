using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.Reconciliation
{
   public class NewReconciliationDto
    {
        public int bankAccountId { get; set; }
        public decimal ammount { get; set; }

        public string bankname { get; set; }


        public List<ReconciliationDto> reconciliationDtos { get; set; }
    }
}
