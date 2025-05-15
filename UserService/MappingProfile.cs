using AutoMapper;
using Entities.Models;
using Shared.DTOObjects;

namespace UserService
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
            CreateMap<ProductForUpdateDTO, User>().ReverseMap();
        }
    }
}

