
namespace retailer.Handler
{
    public class Api_Services
    {
        public string UserId { get; set; }
        public string UserType { get; set; }
        public string TransactionType { get; set; }
        public string OperatorId { get; set; }
        public string Number { get; set; }
        public double Amount { get; set; }
        public string Source { get; set; } //app || website
        //Electricity
        public string CusAccNum { get; set; }
        public string BU { get; set; }
        public string CySubDivision { get; set; }
        public string DueDate { get; set; }
        //Insurance
        public string InsuranceDob { get; set; }
    }
}