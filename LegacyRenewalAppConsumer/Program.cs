using System;
using LegacyRenewalApp;

namespace LegacyRenewalAppConsumer
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            /*
             * DO NOT CHANGE THIS FILE AT ALL
             */

            var renewalService = new SubscriptionRenewalService();

            var invoice = renewalService.CreateRenewalInvoice(
                customerId: 3,
                planCode: "PRO",
                seatCount: 18,
                paymentMethod: "CARD",
                includePremiumSupport: true,
                useLoyaltyPoints: true);

            Console.WriteLine("Invoice created successfully");
            Console.WriteLine(invoice);
            Console.WriteLine($"Final amount: {invoice.FinalAmount:F2}");
        }
    }
}
