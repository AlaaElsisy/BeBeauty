using AutoMapper;
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

            /////
            CreateMap<Category, DisplayCategory>().ReverseMap();
            ///////////////////////////////////////////////
            CreateMap<Order, DisplayOrder>().ReverseMap();
            CreateMap<AddOrder, Order>().ReverseMap();
            CreateMap<OrderDetail, OrderDetailDTO>().ReverseMap();

        }
    }
}
