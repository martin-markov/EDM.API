using AutoMapper;
using EDM.Data.Models;
using EDM.Models;

namespace EDM.API
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }

}
