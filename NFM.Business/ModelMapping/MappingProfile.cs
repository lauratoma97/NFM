using AutoMapper;
using NFM.Business.ModelDTOs;
using NFM.Domain.Models;

namespace NFM.Business.ModelMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Product, ProductDto>(MemberList.Destination);
            CreateMap<Employee, EmployeeDto>(MemberList.Destination);
            CreateMap<ProductDto, Product>();
            CreateMap<CreateProductDto, Product>();
            CreateMap<EmployeeDto, Employee>();
        }
    }
}
