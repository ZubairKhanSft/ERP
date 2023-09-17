using AccountErp.Entities;
using AccountErp.Models.BankAccount;
using AccountErp.Models.Reconciliation;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ReconciliationFactory
    {
        public static Reconciliation Create(ReconciliationAddModel model, string userId)
        {
            var reconciliation = new Reconciliation
            {
                Id=model.Id,
                BankAccountId = model.BankAccountId,
                ReconciliationDate = model.ReconciliationDate,
                StatementBalance = model.StatementBalance,
                IcloseBalance = model.IcloseBalance,
                ReconciliationStatus = model.ReconciliationStatus,
                IsReconciliation = model.IsReconciliation

            };
            return reconciliation;
        }
        public static void Create(ReconciliationEditModel model, Reconciliation entity, string userId)
        {
            entity.BankAccountId = model.BankAccountId;
            entity.ReconciliationDate = model.ReconciliationDate;
            entity.StatementBalance = model.StatementBalance;
            entity.IcloseBalance = model.IcloseBalance;
            entity.ReconciliationStatus = model.ReconciliationStatus;
                entity.IsReconciliation = model.IsReconciliation;


    }

        public static void Create(int id, Reconciliation entity)
        {
            entity.BankAccountId = id;
            entity.ReconciliationDate = null;
            entity.StatementBalance = 0;
            entity.IcloseBalance = 0;
            entity.ReconciliationStatus =1;
            entity.IsReconciliation = false;


        }
    }
}

