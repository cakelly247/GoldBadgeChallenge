using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GoldBadgeChallenge.Data
{
    public class Delivery
    {
        public Delivery () {}
        public Delivery(string orderDate, int itemNumber, int itemQuantity, int customerId, string orderStatus, string deliveryDate)
        {
            OrderDate = orderDate;
            ItemNumber = itemNumber;
            ItemQuantity = itemQuantity;
            CustomerId = customerId;
            OrderStatus = orderStatus;
            DeliveryDate = deliveryDate;
        }
        public string OrderDate { get; set; }
        public string DeliveryDate { get; set; }
        public int ItemNumber { get; set; }
        public int ItemQuantity { get; set; }
        public int CustomerId { get; set; }
        public int Id { get; set; }
        public string OrderStatus { get; set; }
    }
}