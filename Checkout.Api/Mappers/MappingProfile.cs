using AutoMapper;
using Checkout.Api.Model;
using Checkout.Entities.Data;

public class MappingProfile : Profile {
    public MappingProfile() {
        CreateMap<Item, ItemResponse>();
        CreateMap<ItemResponse, Item>();
        CreateMap<Basket, BasketResponse>();
        CreateMap<BasketResponse, Basket>();
        CreateMap<BasketItem, BasketItemResponse>();
        CreateMap<BasketItemResponse, BasketItem>();
    }
}