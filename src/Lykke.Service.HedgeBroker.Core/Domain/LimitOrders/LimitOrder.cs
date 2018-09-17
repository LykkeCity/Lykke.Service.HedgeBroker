using System;

namespace Lykke.Service.HedgeBroker.Core.Domain.LimitOrders
{
    public class LimitOrder
    {
        public LimitOrder()
        {
        }

        public LimitOrder(decimal price, decimal volume, LimitOrderType type)
        {
            Id = Guid.NewGuid().ToString("D");
            Price = price;
            Volume = volume;
            Type = type;
        }

        public string Id { get; }

        public decimal Price { get; }

        public decimal Volume { get; private set; }

        public decimal ExecutedVolume { get; private set; }

        public LimitOrderType Type { get; }

        public LimitOrderError Error { get; set; }

        public string ErrorMessage { get; set; }

        public bool Executed => ExecutedVolume > 0;

        public void ExecuteVolume(decimal volume)
        {
            Volume -= volume;
            ExecutedVolume += volume;
        }

        public void UpdateVolume(decimal volume)
        {
            Volume = volume;
        }
    }
}
