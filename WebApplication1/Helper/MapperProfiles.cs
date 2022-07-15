using AutoMapper;
using WebApplication1.Context.ApplicationUser;
using WebApplication1.Models;

namespace WebApplication1.Helper
{
    public class MapperProfiles : Profile
    {
        public MapperProfiles()
        {
            CreateMap<ApplicationUsers, UserRegister>().ReverseMap();
            CreateMap<ApplicationUsers, EditUser>().ReverseMap();
        }
    }
}
