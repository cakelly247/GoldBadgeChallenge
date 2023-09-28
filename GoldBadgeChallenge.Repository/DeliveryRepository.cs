using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GoldBadgeChallenge.Data;

namespace GoldBadgeChallenge.Repository
{
    public class DeliveryRepository
    {
        private readonly List<Delivery> _deliveryDbContext = new List<Delivery>();

        private int _count = 0;

        public bool AddDelivery(Delivery delivery)
        {
            if (delivery is null)
            {
                return false;
            }
            else
            {
                _count++;
                delivery.Id = _count;
                _deliveryDbContext.Add(delivery);
                return true;
            }
        }

        public List<Delivery> GetDeliveries()
        {
            return _deliveryDbContext;
        }

        public Delivery GetDeliveryById(int id)
        {
            return _deliveryDbContext.FirstOrDefault(x => x.Id == id)!;
        }

        public List<Delivery> GetDeliveryByCustomerId(int customerId)
        {
            return _deliveryDbContext.Where(x => x.CustomerId == customerId).ToList();
        }

        public List<Delivery> GetCompletedDeliveries()
        {
            List<Delivery> completedDeliveries = new List<Delivery>();
            foreach (var delivery in _deliveryDbContext)
            {
                if(delivery.OrderStatus.ToLower() == "Complete".ToLower())
                {
                    completedDeliveries.Add(delivery);
                }
            }

            return completedDeliveries;
        }

        public List<Delivery> GetActiveDeliveries()
        {
            List<Delivery> activeDeliveries = new List<Delivery>();
            foreach (var delivery in _deliveryDbContext)
            {
                if(delivery.OrderStatus.ToLower() == "EnRoute".ToLower())
                {
                    activeDeliveries.Add(delivery);
                }
            }
            return activeDeliveries;
        }

        public List<Delivery> GetScheduledDeliveries()
        {
            List<Delivery> scheduledDeliveries = new List<Delivery>();
            foreach (var delivery in _deliveryDbContext)
            {
                if(delivery.OrderStatus.ToLower() == "Scheduled".ToLower())
                {
                    scheduledDeliveries.Add(delivery);
                }
            }

            return scheduledDeliveries;
        }

        public List<Delivery> GetCanceledDeliveries()
        {
            List<Delivery> canceledDeliveries = new List<Delivery>();
            foreach (var delivery in _deliveryDbContext)
            {
                if(delivery.OrderStatus.ToLower() == "Canceled".ToLower())
                {
                    canceledDeliveries.Add(delivery);
                }
            }
            
            return canceledDeliveries;
        }

        public bool EditDeliveryInfo(int id, Delivery newDeliveryData)
        {
            Delivery selectedDelivery = GetDeliveryById(id);
            if(selectedDelivery is null)
                return false;
            else
            {
                selectedDelivery.OrderDate = newDeliveryData.OrderDate;
                selectedDelivery.ItemNumber = newDeliveryData.ItemNumber;
                selectedDelivery.ItemQuantity = newDeliveryData.ItemQuantity;
                selectedDelivery.CustomerId = newDeliveryData.CustomerId;
                selectedDelivery.OrderStatus = newDeliveryData.OrderStatus;
                selectedDelivery.DeliveryDate = newDeliveryData.DeliveryDate;
                return true;
            }
        }

        public bool UpdateDeliveryStatus(int id, Delivery newDeliveryStatus)
        {
            Delivery selectedDelivery = GetDeliveryById(id);
            if(selectedDelivery is null)
                return false;
            else
            {
                selectedDelivery.OrderStatus = newDeliveryStatus.OrderStatus;
                return true;
            }
        }

        public bool DeleteDelivery(Delivery delivery)
        {
            return _deliveryDbContext.Remove(delivery);
        }

        public bool DeleteDeliveriesByCustomerId(int customerId)
        {
            var targetedDeliveries = GetDeliveryByCustomerId(customerId);
            if(targetedDeliveries.Count() == 0)
                return false;
            else
            {
                foreach (var delivery in targetedDeliveries)
                {
                    _deliveryDbContext.Remove(delivery);
                }
                return true;
            }
        }
    }
}