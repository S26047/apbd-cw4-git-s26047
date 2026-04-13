using System;

namespace LegacyRenewalApp
{
    public class SubscriptionRenewalService
    {
        public RenewalInvoice CreateRenewalInvoice(
            int customerId,
            string planCode,
            int seatCount,
            string paymentMethod,
            bool includePremiumSupport,
            bool useLoyaltyPoints)
        {
            ValidateInput(customerId, planCode, seatCount, paymentMethod);

            string normalizedPlanCode = planCode.Trim().ToUpperInvariant();
            string normalizedPaymentMethod = paymentMethod.Trim().ToUpperInvariant();

            var customerRepository = new CustomerRepository();
            var planRepository = new SubscriptionPlanRepository();

            var (customer, plan) = GetRequiredData(customerId, customerRepository, planRepository, normalizedPlanCode);
            
            if (!customer.IsActive)
            {
                throw new InvalidOperationException("Inactive customers cannot renew subscriptions");
            }

            decimal baseAmount = (plan.MonthlyPricePerSeat * seatCount * 12m) + plan.SetupFee;
            var discountAmount = CalculateDiscount(seatCount, useLoyaltyPoints, customer, baseAmount, plan, out var notes);

            decimal subtotalAfterDiscount = baseAmount - discountAmount;
            if (subtotalAfterDiscount < 300m)
            {
                subtotalAfterDiscount = 300m;
                notes += "minimum discounted subtotal applied; ";
            }

            var supportFee = CalculateSupportFee(includePremiumSupport, normalizedPlanCode, out var supportNotes);
            notes += supportNotes;
            
            decimal paymentFee = 0m;
            if (normalizedPaymentMethod == "CARD")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.02m;
                notes += "card payment fee; ";
            }
            else if (normalizedPaymentMethod == "BANK_TRANSFER")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.01m;
                notes += "bank transfer fee; ";
            }
            else if (normalizedPaymentMethod == "PAYPAL")
            {
                paymentFee = (subtotalAfterDiscount + supportFee) * 0.035m;
                notes += "paypal fee; ";
            }
            else if (normalizedPaymentMethod == "INVOICE")
            {
                paymentFee = 0m;
                notes += "invoice payment; ";
            }
            else
            {
                throw new ArgumentException("Unsupported payment method");
            }

            decimal taxRate = 0.20m;
            if (customer.Country == "Poland")
            {
                taxRate = 0.23m;
            }
            else if (customer.Country == "Germany")
            {
                taxRate = 0.19m;
            }
            else if (customer.Country == "Czech Republic")
            {
                taxRate = 0.21m;
            }
            else if (customer.Country == "Norway")
            {
                taxRate = 0.25m;
            }

            decimal taxBase = subtotalAfterDiscount + supportFee + paymentFee;
            decimal taxAmount = taxBase * taxRate;
            decimal finalAmount = taxBase + taxAmount;

            if (finalAmount < 500m)
            {
                finalAmount = 500m;
                notes += "minimum invoice amount applied; ";
            }

            var invoice = new RenewalInvoice
            {
                InvoiceNumber = $"INV-{DateTime.UtcNow:yyyyMMdd}-{customerId}-{normalizedPlanCode}",
                CustomerName = customer.FullName,
                PlanCode = normalizedPlanCode,
                PaymentMethod = normalizedPaymentMethod,
                SeatCount = seatCount,
                BaseAmount = Math.Round(baseAmount, 2, MidpointRounding.AwayFromZero),
                DiscountAmount = Math.Round(discountAmount, 2, MidpointRounding.AwayFromZero),
                SupportFee = Math.Round(supportFee, 2, MidpointRounding.AwayFromZero),
                PaymentFee = Math.Round(paymentFee, 2, MidpointRounding.AwayFromZero),
                TaxAmount = Math.Round(taxAmount, 2, MidpointRounding.AwayFromZero),
                FinalAmount = Math.Round(finalAmount, 2, MidpointRounding.AwayFromZero),
                Notes = notes.Trim(),
                GeneratedAt = DateTime.UtcNow
            };

            LegacyBillingGateway.SaveInvoice(invoice);

            if (!string.IsNullOrWhiteSpace(customer.Email))
            {
                string subject = "Subscription renewal invoice";
                string body =
                    $"Hello {customer.FullName}, your renewal for plan {normalizedPlanCode} " +
                    $"has been prepared. Final amount: {invoice.FinalAmount:F2}.";

                LegacyBillingGateway.SendEmail(customer.Email, subject, body);
            }

            return invoice;
        }

        private static decimal CalculateSupportFee(
            bool includePremiumSupport,
            string normalizedPlanCode,
            out string notes)
        {
            decimal supportFee = 0m;
            notes = string.Empty;

            if (includePremiumSupport)
            {
                if (normalizedPlanCode == "START")
                {
                    supportFee = 250m;
                }
                else if (normalizedPlanCode == "PRO")
                {
                    supportFee = 400m;
                }
                else if (normalizedPlanCode == "ENTERPRISE")
                {
                    supportFee = 700m;
                }

                notes += "premium support included; ";
            }

            return supportFee;
        }

        private static decimal CalculateDiscount(int seatCount, bool useLoyaltyPoints, Customer customer, decimal baseAmount,
            SubscriptionPlan plan, out string notes)
        {
            decimal discountAmount = 0m;
            notes = string.Empty;

            discountAmount += CalculateSegmentDiscount(customer, plan, baseAmount, out var segmentNotes);
            notes += segmentNotes;

            discountAmount += CalculateTenureDiscount(customer, baseAmount, out var tenureNotes);
            notes += tenureNotes;

            discountAmount += CalculateSeatDiscount(seatCount, baseAmount, out var seatNotes);
            notes += seatNotes;

            discountAmount += ApplyLoyaltyPoints(customer, useLoyaltyPoints, out var loyaltyNotes);
            notes += loyaltyNotes;

            return discountAmount;
        }

        private static decimal ApplyLoyaltyPoints(Customer customer, bool useLoyaltyPoints, out string notes)
        {
            decimal discount = 0m;
            notes = string.Empty;

            if (useLoyaltyPoints && customer.LoyaltyPoints > 0)
            {
                int pointsToUse = customer.LoyaltyPoints > 200 ? 200 : customer.LoyaltyPoints;
                discount += pointsToUse;
                notes += $"loyalty points used: {pointsToUse}; ";
            }

            return discount;
        }

        private static decimal CalculateSeatDiscount(int seatCount, decimal baseAmount, out string notes)
        {
            decimal discount = 0m;
            notes = string.Empty;

            if (seatCount >= 50)
            {
                discount += baseAmount * 0.12m;
                notes += "large team discount; ";
            }
            else if (seatCount >= 20)
            {
                discount += baseAmount * 0.08m;
                notes += "medium team discount; ";
            }
            else if (seatCount >= 10)
            {
                discount += baseAmount * 0.04m;
                notes += "small team discount; ";
            }

            return discount;
        }

        private static decimal CalculateTenureDiscount(Customer customer, decimal baseAmount, out string notes)
        {
            decimal discount = 0m;
            notes = string.Empty;

            if (customer.YearsWithCompany >= 5)
            {
                discount += baseAmount * 0.07m;
                notes += "long-term loyalty discount; ";
            }
            else if (customer.YearsWithCompany >= 2)
            {
                discount += baseAmount * 0.03m;
                notes += "basic loyalty discount; ";
            }

            return discount;
        }

        private static decimal CalculateSegmentDiscount(Customer customer, SubscriptionPlan plan, decimal baseAmount, out string notes)
        {
            decimal discount = 0m;
            notes = string.Empty;

            if (customer.Segment == "Silver")
            {
                discount += baseAmount * 0.05m;
                notes += "silver discount; ";
            }
            else if (customer.Segment == "Gold")
            {
                discount += baseAmount * 0.10m;
                notes += "gold discount; ";
            }
            else if (customer.Segment == "Platinum")
            {
                discount += baseAmount * 0.15m;
                notes += "platinum discount; ";
            }
            else if (customer.Segment == "Education" && plan.IsEducationEligible)
            {
                discount += baseAmount * 0.20m;
                notes += "education discount; ";
            }

            return discount;
        }

        private static (Customer customer, SubscriptionPlan plan) GetRequiredData(
            int customerId,
            CustomerRepository customerRepository,
            SubscriptionPlanRepository planRepository,
            string normalizedPlanCode)
        {
            var customer = customerRepository.GetById(customerId);
            var plan = planRepository.GetByCode(normalizedPlanCode);

            return (customer, plan);
        }

        private static void ValidateInput(int customerId, string planCode, int seatCount, string paymentMethod)
        {
            if (customerId <= 0)
            {
                throw new ArgumentException("Customer id must be positive");
            }

            if (string.IsNullOrWhiteSpace(planCode))
            {
                throw new ArgumentException("Plan code is required");
            }

            if (seatCount <= 0)
            {
                throw new ArgumentException("Seat count must be positive");
            }

            if (string.IsNullOrWhiteSpace(paymentMethod))
            {
                throw new ArgumentException("Payment method is required");
            }
        }
    }
}
