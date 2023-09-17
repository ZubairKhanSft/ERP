using AccountErp.Entities;
using AccountErp.Models.Permission;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class PermissionFactory
    {

        public static Permission Create(PermissionAddModel model, string userId)
        {
            var data = new Permission()
            {
                Permisions = model.Permissions,
                PermisionDescription = model.PermissionDescription,
                PermissionCode = model.PermissionCode,

                ScreenId = model.ScreenId,

                ScreenCode = model.ScreenCode,
                ScreenUrl = model.ScreenUrl,


                Status = Constants.RecordStatus.Active,
                CreatedBy = userId ?? "0",
                CreatedOn = Utility.GetDateTime(),
                UpdatedBy = userId ?? "0",
                UpdatedOn = Utility.GetDateTime(),
               
            };
            return data;
        }


        public static void Create(PermissionEditModel model, Permission entity, string userId)
        {
            entity.Permisions = model.Permissions;
            entity.PermisionDescription = model.PermissionDescription;
            entity.PermissionCode = model.PermissionCode;
            entity.ScreenId = model.ScreenId;


            entity.ScreenCode = model.ScreenCode;
            entity.ScreenUrl = model.ScreenUrl;

            
            entity.UpdatedBy = userId ?? "0";
            entity.UpdatedOn = Utility.GetDateTime();
        }

    }
}
