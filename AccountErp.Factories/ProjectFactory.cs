using AccountErp.Entities;
using AccountErp.Models.Bill;
using AccountErp.Models.Project;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ProjectFactory
    {
        public static Project Create(ProjectAddModel model, string userId)
        {
            var item = new Project
            {
                ProjectName = model.ProjectName,
                CustomerId = model.CustomerId,
                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime()
            };
            return item;
        }
        public static void Create(ProjectEditModel model, Project entity, string userId)
        {
            entity.ProjectName = model.ProjectName;
            entity.CustomerId = model.CustomerId;
            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
        }

        public static ProjectTransaction CreateByInvoice(Invoice model, int projectId, string userId)
        {
            var item = new ProjectTransaction
            {
             ProjectId = projectId,
             InvoiceId = model.Id,
             BillId = null,
             TransType = Constants.ProjectTransactionType.Invoice
             
            };
            return item;
        }

        public static ProjectTransaction CreateByBill(BillAddModel model, int billId, string userId)
        {
            var item = new ProjectTransaction
            {
              //  ProjectId = model.ProjectId,  zk
                InvoiceId = null,
                BillId = billId,
                TransType = Constants.ProjectTransactionType.Bill

            };
            return item;
        }
    }
}
