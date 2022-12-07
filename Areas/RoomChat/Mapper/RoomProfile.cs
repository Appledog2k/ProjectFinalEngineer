
using AutoMapper;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Models.AggregateRoom;
using ProjectFinalEngineer.Utilities;

namespace ProjectFinalEngineer.Areas.RoomChat.Mapper
{
    public class RoomProfile : Profile
    {
        public RoomProfile()
        {
            CreateMap<Room, RoomViewModel>();
            CreateMap<RoomViewModel, Room>();
        }
    }
}