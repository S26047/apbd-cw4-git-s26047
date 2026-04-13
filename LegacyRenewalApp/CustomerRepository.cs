using System;
using System.Collections.Generic;
using System.Threading;

namespace LegacyRenewalApp
{
    public class CustomerRepository
    {
        public static readonly Dictionary<int, Customer> Database = new Dictionary<int, Customer>
        {
            { 1, new Customer { Id = 1, FullName = "Anna Kowalska", Email = "anna.kowalska@example.com", Segment = "Standard", Country = "Poland", YearsWithCompany = 1, LoyaltyPoints = 20, IsActive = true } },
            { 2, new Customer { Id = 2, FullName = "Piotr Lis", Email = "piotr.lis@example.com", Segment = "Gold", Country = "Poland", YearsWithCompany = 4, LoyaltyPoints = 140, IsActive = true } },
            { 3, new Customer { Id = 3, FullName = "John Smith", Email = "john.smith@example.com", Segment = "Platinum", Country = "Germany", YearsWithCompany = 7, LoyaltyPoints = 260, IsActive = true } },
            { 4, new Customer { Id = 4, FullName = "School Lab", Email = "it-admin@school.example.com", Segment = "Education", Country = "Czech Republic", YearsWithCompany = 3, LoyaltyPoints = 90, IsActive = true } },
            { 5, new Customer { Id = 5, FullName = "Nordic Ventures", Email = "finance@nordic.example.com", Segment = "Silver", Country = "Norway", YearsWithCompany = 2, LoyaltyPoints = 30, IsActive = true } }
        };

        public Customer GetById(int customerId)
        {
            int randomWaitTime = new Random().Next(500);
            Thread.Sleep(randomWaitTime);

            if (Database.ContainsKey(customerId))
            {
                return Database[customerId];
            }

            throw new ArgumentException($"Customer with id {customerId} does not exist");
        }
    }
}
