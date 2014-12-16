using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace crebit.retailer.Models.Common
{
    

    public class UserInfo
    {
        public Double AvailBal { get; set; }
        public double Margin { get; set; }
        public int UType { get; set; }
        public bool ServiceStatus { get; set; }
        public Boolean IsFixedCharge { get; set; }

    }
}