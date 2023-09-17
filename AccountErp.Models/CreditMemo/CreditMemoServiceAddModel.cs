using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.CreditMemo
{
   public class CreditMemoServiceAddModel
    {
        public int CreditMemoId { get; set; }
        public int? ServiceId { get; set; }
        public int? ProductId { get; set; }
        public decimal Rate { get; set; }
        public decimal Price { get; set; }
        public int? TaxId { get; set; }
        public decimal? TaxPrice { get; set; }
        public decimal? TaxPercentage { get; set; }
        public decimal LineAmount { get; set; }
        public int OldQuantity { get; set; }
        public int NewQuantity { get; set; }
        public int TaxBankAccountId { get; set; }
        public int BankAccountId { get; set; }
        public Decimal OldAmmount { get; set; }
        public Decimal NewAmmount { get; set; }
        public Decimal DiffAmmount { get; set; }
        public Decimal TaxDiffAmmount { get; set; }

    }
}
