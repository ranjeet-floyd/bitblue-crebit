using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace api.dhs.Models.DL.Common
{
    //used for admin uodates
    public class DL_AdminUpdate
    {
        [JsonProperty("text")]
        public string Text { get; set; }
        [JsonProperty("fDate")]
        public string FDate { get; set; }
        [JsonProperty("tDate")]
        public string TDate { get; set; }
    }

    public class DL_PopUpMessage : DL_AdminUpdate
    {
    }

    public class DL_MonthlyRetailerCharges
    {
        public string UserId { get; set; }
        //public string MyProperty { get; set; }
    }
}