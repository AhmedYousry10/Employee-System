using Employee_Management_System.DTO;
using Employee_Management_System.Models;
using AutoMapper;

namespace Employee_Management_System.MappingProfile
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<Employee, EmployeeDTO>()
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ReverseMap();

            CreateMap<RegisterDTO, Employee>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.Email));

            CreateMap<LoginDTO, Employee>().ReverseMap();
        }
    }
}
