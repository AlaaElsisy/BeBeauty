using AutoMapper;
using BeBeauty.DTOs.ProductsDTos;
using BeBeauty.Models;

namespace BeBeauty.Mapping
{
    public class MappingConfig:Profile
    {
        public MappingConfig() {

            CreateMap<Product, Displayproduct>().ReverseMap();
            CreateMap<Product, AddProduct>().ReverseMap();

        }
    }
}
