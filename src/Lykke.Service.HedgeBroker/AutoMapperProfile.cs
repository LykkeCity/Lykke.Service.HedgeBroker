using System.Linq;
using AutoMapper;
using Lykke.Service.HedgeBroker.Domain.Domain.OrderBooks;
using Lykke.Service.HedgeBroker.Rabbit.Messages.ExternalOrderBook;

namespace Lykke.Service.HedgeBroker
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<ExternalOrderBook, OrderBook>()
                .ForMember(x => x.Exchange, opt => opt.MapFrom(o => o.Source))
                .ForMember(x => x.SellLimitOrders, opt => opt.MapFrom(o =>
                    o.SellLimitOrders
                        .Select(e => new OrderBookLimitOrder
                        {
                            Price = e.Price,
                            Volume = e.Volume
                        })
                        .ToArray()))

                .ForMember(x => x.BuyLimitOrders, opt => opt.MapFrom(o =>
                    o.BuyLimitOrders
                        .Select(e => new OrderBookLimitOrder
                        {
                            Price = e.Price,
                            Volume = e.Volume
                        })
                        .ToArray()));

            CreateMap<OrderBook, ExternalOrderBook>()
                .ForMember(x => x.Source, opt => opt.MapFrom(o => o.Exchange))
                .ForMember(x => x.SellLimitOrders, opt => opt.MapFrom(o =>
                    o.SellLimitOrders
                        .Select(e => new ExternalLimitOrder
                        {
                            Price = e.Price,
                            Volume = e.Volume
                        })
                        .ToArray()))

                .ForMember(x => x.BuyLimitOrders, opt => opt.MapFrom(o =>
                    o.BuyLimitOrders
                        .Select(e => new OrderBookLimitOrder
                        {
                            Price = e.Price,
                            Volume = e.Volume
                        })
                        .ToArray()));
        }
    }
}
