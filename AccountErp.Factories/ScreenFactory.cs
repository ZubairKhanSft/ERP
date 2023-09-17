using AccountErp.Entities;
using AccountErp.Models.Screen;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class ScreenFactory
    {
        public static Screen Create(AddScreenModel model, string userId)
        {
            var data = new Screen
            {
                ScreenName = model.ScreenName,
                ScreenCode = model.ScreenCode,
                Status = Constants.RecordStatus.Active,
               
                CreatedOn = Utility.GetDateTime(),
                ScreenUrl = model.ScreenUrl,
               
            };
            return data;
        }
        public static void Create(AddScreenModel model, Screen entity, string userId)
        {
            entity.ScreenCode = model.ScreenCode;
            entity.ScreenName = model.ScreenName;
           
            entity.UpdatedOn = Utility.GetDateTime();
            entity.ScreenUrl = model.ScreenUrl;
          
        }
    }
}
