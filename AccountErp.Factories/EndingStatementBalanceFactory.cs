using AccountErp.Entities;
using AccountErp.Models.EndingStatementBalance;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
  public  class EndingStatementBalanceFactory
    {
        public static EndingStatementBalance Create(EndingStatementBalanceAddModel model, string userId)
        {
            var endingStatementBalance = new EndingStatementBalance
            {

                BankAccountId = model.BankAccountId,
                EndingBalanceDate =model.EndingBalanceDate,
                EndingBalanceAmount=model.EndingBalanceAmount



            };
            return endingStatementBalance;
        }
        public static void Create(EndingStatementBalanceEditModel model, EndingStatementBalance entity, string userId)
        {
            entity.Id = model.Id;
            entity.BankAccountId = model.BankAccountId;
            entity.EndingBalanceAmount = model.EndingBalanceAmount;
            entity.EndingBalanceDate = model.EndingBalanceDate;


        }
    }
}
