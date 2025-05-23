using AutoMapper;
using BeBeauty.DTOs.CartDtO;
using BeBeauty.DTOs.CatecoryDTos;
using BeBeauty.DTOs.OrdersDTos;
using BeBeauty.DTOs.ProductsDTos;
using BeBeauty.Models;

namespace BeBeauty.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig() {

            CreateMap<Product, Displayproduct>().ReverseMap();
            CreateMap<Product, AddProduct>().ReverseMap();
             
            CreateMap<UpdateProductDto, Product>()
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
            /////
            CreateMap<Category, DisplayCategory>().ReverseMap();
            ///////////////////////////////////////////////
            CreateMap<Order, DisplayOrder>().ReverseMap();
            CreateMap<AddOrder, Order>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();

            ////////////////////////////////////////////

            // CartItem to DisplayCartItemDTO
            CreateMap<CartItem, DisplayCartItem>()
                .ForMember(dest => dest.Product, opt => opt.MapFrom(src => src.Product));

            // AddCartItemDTO to CartItem
            CreateMap<AddCartItem, CartItem>()
                .ForMember(dest => dest.CartItemId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

            // UpdateCartItemDTO to CartItem
            CreateMap<UpdateCartItem, CartItem>()
                .ForMember(dest => dest.UserId, opt => opt.Ignore())
                .ForMember(dest => dest.ProductId, opt => opt.Ignore())
                .ForMember(dest => dest.User, opt => opt.Ignore())
                .ForMember(dest => dest.Product, opt => opt.Ignore());

        

        }
    }
}
