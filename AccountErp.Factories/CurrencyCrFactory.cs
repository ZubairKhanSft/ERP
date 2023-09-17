using AccountErp.Entities;
using AccountErp.Models.CurrencyCr;
using AccountErp.Models.Vendor;
using AccountErp.Utilities;
using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Factories
{
    public class CurrencyCrFactory
    {

        public static CurrencyCr Create(AddCurrencyCrModel model)
        {
            var vendor = new CurrencyCr
            {
                UserId = model.UserId,
                Name = model.Name,
                CreatedOn = Utility.GetDateTime(),
                Status = Constants.RecordStatus.Active,
                Symbol = model.Symbol,
            };

            return vendor;
        }

        public static void Create(AddCurrencyCrModel model, CurrencyCr entity)
        {
            entity.Name = model.Name != null ? model.Name : entity.Name;
            entity.UserId = model.UserId != 0 ? model.UserId : entity.UserId;
            entity.Symbol = model.Symbol != null ? model.Symbol : entity.Symbol;
           /* entity.City = model.City != null ? model.City : entity.City;
            entity.ZipCode = model.ZipCode != null ? model.ZipCode : entity.ZipCode;
            entity.State = model.State != null ? model.State : entity.State;
            entity.Country = model.Country != null ? model.Country : entity.Country;
            entity.Email = model.Email != null ? model.Email : entity.Email;
            entity.Phone = model.Phone != null ? model.Phone : entity.Phone;*/
            entity.UpdatedOn = DateTime.Now;
        }

    }
}
