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
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

        }

        public StripeList<Subscription> GetMembership(string customerId)
        {
            StripeConfiguration.ApiKey = Environment.GetEnvironmentVariable("StripeKey");

            
            var options = new CustomerGetOptions();
            var service = new CustomerService();
            options.AddExpand("subscriptions");

            var customer = service.Get(customerId, options);

            return customer.Subscriptions;

        }

        public bool UsersExists(string email)
        {
            
            var options = new CustomerListOptions();

            options.Email = email;

            var service = new CustomerService();

            var list = service.List(options);

            if (list.Data.Count > 1)
            {
                return true;
            }
            else
            {
                return false;
            }

        }

        public string GetCustomerID (string customerEmail)
        {
            var options = new CustomerListOptions();
            options.Email = customerEmail;

            var service = new CustomerService();

            var customer = service.List(options);

            if (customer.Data.Count > 0)
            {
                return customer.Data[0].Id;
            }

            else return null;

           
        }

        public Plan GetPlanIDByCustomerID(string customerID)
        {

            var options = new CustomerGetOptions();
            var service = new CustomerService();
            options.AddExpand("subscriptions");

            var customer = service.Get(customerID, options);

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

