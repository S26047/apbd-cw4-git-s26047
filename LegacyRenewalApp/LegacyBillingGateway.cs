using System;
using System.Threading;

namespace LegacyRenewalApp
{
    /*
     * DO NOT CHANGE THIS FILE AT ALL
     * We assume that this is a legacy static dependency that cannot be modified.
     */
    public static class LegacyBillingGateway
    {
        public static void SaveInvoice(RenewalInvoice invoice)
        {
            int randomWaitTime = new Random().Next(700);
            Thread.Sleep(randomWaitTime);
            Console.WriteLine($"Invoice {invoice.InvoiceNumber} saved for {invoice.CustomerName}");
        }

        public static void SendEmail(string email, string subject, string body)
        {
            int randomWaitTime = new Random().Next(700);
            Thread.Sleep(randomWaitTime);
            Console.WriteLine($"Email sent to {email}: {subject}");
        }
    }
}
