using System;

namespace LegacyRenewalApp
{
    public class RenewalInvoice
    {
        public string InvoiceNumber { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string PlanCode { get; set; } = string.Empty;
        public string PaymentMethod { get; set; } = string.Empty;
        public int SeatCount { get; set; }
        public decimal BaseAmount { get; set; }
        public decimal DiscountAmount { get; set; }
        public decimal SupportFee { get; set; }
        public decimal PaymentFee { get; set; }
        public decimal TaxAmount { get; set; }
        public decimal FinalAmount { get; set; }
        public string Notes { get; set; } = string.Empty;
        public DateTime GeneratedAt { get; set; }

        public override string ToString()
        {
            return $"InvoiceNumber={InvoiceNumber}, Customer={CustomerName}, Plan={PlanCode}, Seats={SeatCount}, FinalAmount={FinalAmount:F2}, Notes={Notes}";
        }
    }
}
