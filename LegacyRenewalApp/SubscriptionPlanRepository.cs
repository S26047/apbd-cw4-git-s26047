using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyRenewalApp
{
    public class SubscriptionPlanRepository
    {
        public static readonly Dictionary<string, SubscriptionPlan> Database = new Dictionary<string, SubscriptionPlan>
        {
            { "START", new SubscriptionPlan { Code = "START", Name = "Start", MonthlyPricePerSeat = 49m, SetupFee = 120m, IsEducationEligible = false } },
            { "PRO", new SubscriptionPlan { Code = "PRO", Name = "Professional", MonthlyPricePerSeat = 89m, SetupFee = 180m, IsEducationEligible = true } },
            { "ENTERPRISE", new SubscriptionPlan { Code = "ENTERPRISE", Name = "Enterprise", MonthlyPricePerSeat = 149m, SetupFee = 300m, IsEducationEligible = false } }
        };

        public SubscriptionPlan GetByCode(string code)
        {
            int randomWaitTime = new Random().Next(500);
            Thread.Sleep(randomWaitTime);

            string normalizedCode = code.ToUpperInvariant();
            if (Database.ContainsKey(normalizedCode))
            {
                return Database[normalizedCode];
            }

            throw new ArgumentException($"Plan with code {code} does not exist");
        }
    }
}
