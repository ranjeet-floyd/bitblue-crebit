using Newtonsoft.Json;

namespace WebApplication1.Models.DL.User
{
    public class DL_Login
    {
        public string Mobile { get; set; }
        public string Pass { get; set; }
        public string Version { get; set; }
    }
    //Used for login status return.
    public class DL_LoginReturn
    {
        [JsonProperty("isSupported")]
        public bool IsSupported { get; set; }
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }
        [JsonProperty("userId")]
        public string UserId { get; set; }
        [JsonProperty("availableBalance")]
        public string AvailableBalance { get; set; }
        [JsonProperty("isUpdated")]
        public bool IsUpdated { get; set; }
        [JsonProperty("isDataUpdated")]
        public bool IsDataUpdated { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("userKey")]
        public string UserKey { get; set; }
        [JsonProperty("uType")]
        public string UType { get; set; }
    }
    public class DL_SignUp
    {
        public int UserType { get; set; }
        public string Name { get; set; }
        public string Pass { get; set; }
        public string Mobile { get; set; }
    }
    public class DL_SignUpReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class DL_ChangePassword
    {
        public string UserId { get; set; }
        public string Key { get; set; }
        public string OPass { get; set; }
        public string NPass { get; set; }
    }


    public class DL_ChangePasswordReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class DL_ForgotPassword
    {
        public string Mobile { get; set; }
    }

    public class DL_ForgotPasswordReturn
    {
        [JsonProperty("status")]
        public string Status { get; set; }
    }

    public class DL_BankDetailsReturn
    {
        [JsonProperty("id")]
        public int Id { get; set; }
        [JsonProperty("name")]
        public string Name { get; set; }
        [JsonProperty("account")]
        public string Account { get; set; }
        [JsonProperty("iFSC")]
        public string IFSC { get; set; }
        [JsonProperty("mobile")]
        public string Mobile { get; set; }
        [JsonProperty("amount")]
        public string Amount { get; set; }
    }

}