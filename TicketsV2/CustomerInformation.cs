using System;
using Stripe;
using TicketsV2.Models;

namespace TicketsV2
{
	public class CustomerInformation
	{
        public string CustomerID { get; set; }
		public StripeList<Subscription> Membership { get; set; }
		public Plan CustomerPlan { get; set; }
        public string IsMembershipActive { get; set; }
        public string PriceID { get; set; }
        public AccessLevel AccessLevel { get; set; }

        public CustomerInformation()
		{
			
		}

		public CustomerInformation GetCustomerInformation(string clientID)
		{
            var paymentUtility = new PaymentUtility();

            var customerId = paymentUtility.GetCustomerID(clientID);

            var membership = paymentUtility.GetMembership(customerId);

            var isMembershipActive = membership.Data[0].Status;

            var customerPlan = paymentUtility.GetPlanIDByCustomerID(customerId);

            var priceID = customerPlan.Id;

            var accessLevel = paymentUtility.CheckAccesslevel(priceID);

            var customerInfo = new CustomerInformation()
            {
                CustomerID = customerId,
                Membership = membership,
                CustomerPlan = customerPlan,
                IsMembershipActive = isMembershipActive,
                PriceID = priceID,
                AccessLevel = accessLevel

            };

            return customerInfo;
        }
	}
}

