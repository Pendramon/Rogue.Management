using AutoMapper;
using Rogue.Management.Data.Model;
using Rogue.Management.View.Model;

namespace Rogue.Service.Mappings
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            this.CreateMap<UserDto, User>();
        }
    }
}
