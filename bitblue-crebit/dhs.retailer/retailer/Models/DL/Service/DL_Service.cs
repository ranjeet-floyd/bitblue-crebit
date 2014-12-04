
using Newtonsoft.Json;
namespace crebit.retailer.Models.DL.Service
{
    // Added properties MobileNo, CityId, ServiceId,Amount
    public class DL_TorrentPower
    { 
        //[JsonProperty("userId")]
        public string UserId { get; set; }
        //[JsonProperty("key")]
        public string Key { get; set; }
       // [JsonProperty("Bu")]
        public int Bu { get; set; }
        //[JsonProperty("CusAcc")]
        public string CusAcc { get; set; }
        //[JsonProperty("cusMobileNo")]
        public int cusMob { get; set; }
        //[JsonProperty("amount")]
        public string amount { get; set; }
       

    }

    // return type status and Message.
    public class DL_TorrentPowerReturn
    {
        [JsonProperty("Status")]
        public int Status { get; set; }
        [JsonProperty("Message")]
        public string Message { get; set; }
        [JsonProperty("AvaiBal")]
        public double AvaiBal { get; set; }
        //public string CusCity { get; set; }
        //public string BillDate { get; set; }
        //public string DueDate { get; set; }
        //public double NetAmount { get; set; }
        //public double GrossAmount { get; set; }
        //public int GroupNo { get; set; }
    }

}