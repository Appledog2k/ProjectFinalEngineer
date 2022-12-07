
using AutoMapper;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Utilities;

namespace ProjectFinalEngineer.Areas.RoomChat.Mapper
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<AppUser, UserViewModel>()
                .ForMember(dst => dst.Username, opt => opt.MapFrom(x => x.UserName));
            CreateMap<UserViewModel, AppUser>();
        }
    }
}