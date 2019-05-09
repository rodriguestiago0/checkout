using AutoMapper;
using Checkout.Api.Model;
using Checkout.Entities.Data;
using System.Linq;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Item, ItemResponse>();
        CreateMap<ItemResponse, Item>();
        CreateMap<Basket, BasketResponse>().ForMember(dest => dest.Items, orig => orig.MapFrom(s => s.Items.Values));
        CreateMap<BasketResponse, Basket>().ForMember(dest => dest.Items, orig => orig.MapFrom(s => s.Items.ToDictionary(k => k.Item.Id, v => v)));
        CreateMap<BasketItem, BasketItemResponse>();
        CreateMap<BasketItemResponse, BasketItem>();
    }
}
