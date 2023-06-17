using System;
using System.IO;
using System.Threading.Tasks;
using Stripe;
using TicketsV2.Models;

namespace TicketsV2
{
	public class PaymentUtility
	{
		public PaymentUtility()
		{
            
        }

        public async Task<StripeList<Subscription>> GetMembership(string customerId)
        {
            var options = new CustomerGetOptions();
            var service = new CustomerService();
            options.AddExpand("subscriptions");

            var customer = await service.GetAsync(customerId, options);

            return customer.Subscriptions;

        }

        public async Task<bool> UsersExists(string email)
        {
            
            var options = new CustomerListOptions();

            options.Email = email;

            var service = new CustomerService();

            var list = await service.ListAsync(options);

            if (list.Data.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public async Task<string> GetCustomerID(string customerEmail)
        {
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            var options = new CustomerListOptions();

            options.Email = customerEmail;

            var service = new CustomerService();

            var result = await service.ListAsync(options);

            if(result.Data.Count > 0)
            {
                return result.Data[0].Id;
            }
            else
            {
                return "Customer ID Not Found";
            }
           
        }

        public async Task<Plan> GetPlanIDByCustomerID(string customerID)
        {
            var options = new CustomerGetOptions();
            var service = new CustomerService();
            options.AddExpand("subscriptions");

            var customer = await service.GetAsync(customerID, options);

            return customer.Subscriptions.Data[0].Items.Data[0].Plan;
        }

        public AccessLevel CheckAccesslevel(string priceID)
        {
            if(priceID == "price_1NGxwCBhMrFju7BcRJeWxTKJ")
            {
                //Pro
                var accesslevel = new AccessLevel()
                {
                    AccessLvl = "Pro",
                    EventAmount = 5,
                    TicketAmount = 250
                };
                return accesslevel;


            }
            else if (priceID == "price_1NGxqrBhMrFju7BcSW9gx02k")
            {

                //Premium
                var accesslevel = new AccessLevel()
                {
                    AccessLvl = "Premium",
                    EventAmount = 10,
                    TicketAmount = 500
                };
                return accesslevel;


            }
            else if (priceID == "price_1NGxtHBhMrFju7BcgLfc7QHm")
            {
                //basico
                var accesslevel = new AccessLevel()
                {
                    AccessLvl = "Basico",
                    EventAmount = 3,
                    TicketAmount = 150

                };

                return accesslevel;
            }
            else
            {
                return new AccessLevel();
            }
        }
    }
	
}

