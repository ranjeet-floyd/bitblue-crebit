using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crebit.retailer.Models.Common
{
    public enum APIName
    {
        Crebit = 1,
        Cyber_Plate = 2,
        Fund_Transfer = 3,
        Bank_Transfer = 4,
        Electricity_MSEB = 5
    };
    public enum AccountType { Enterprise = 0, Personal = 1 };
}