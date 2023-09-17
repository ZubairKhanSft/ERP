using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Dtos.EndingStatementBalance
{
  public class EndingStatementBalanceDto
    {
        public int Id { get; set; }
        public DateTime? EndingBalanceDate { get; set; }
        public Decimal EndingBalanceAmount { get; set; }
        public int BankAccountId { get; set; }
        //public BankAccount bank { get; set; }
    }
}
