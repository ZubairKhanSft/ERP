using System;
using System.Collections.Generic;
using System.Text;

namespace AccountErp.Models.CurrencyCr
{
    public class AddCurrencyCrModel
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }

    }
}
