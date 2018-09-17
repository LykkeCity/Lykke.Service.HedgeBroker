namespace Lykke.Service.HedgeBroker.Core.Domain
{
    public class Balance
    {
        public Balance(string exchange, string assetId, decimal amount)
        {
            Exchange = exchange;
            AssetId = assetId;
            Amount = amount;
        }

        public string Exchange { get; }

        public string AssetId { get; }

        public decimal Amount { get; }
    }
}
