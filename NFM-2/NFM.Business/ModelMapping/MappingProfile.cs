using AutoMapper;
using NFM.Business.ModelDTOs;
using NFM.Domain.Models;

namespace NFM.Business.ModelMapping
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {            
            CreateMap<Employee, EmployeeDto>(MemberList.Destination);
            
            CreateMap<EmployeeDto, Employee>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}
