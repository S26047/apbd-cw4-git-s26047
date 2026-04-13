namespace LegacyRenewalApp
{
    public class SubscriptionPlan
    {
        public string Code { get; set; } = string.Empty;
        public string Name { get; set; } = string.Empty;
        public decimal MonthlyPricePerSeat { get; set; }
        public decimal SetupFee { get; set; }
        public bool IsEducationEligible { get; set; }
    }
}
