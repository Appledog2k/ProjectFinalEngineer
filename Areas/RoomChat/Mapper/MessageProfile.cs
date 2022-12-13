
using AutoMapper;
using ProjectFinalEngineer.Areas.RoomChat.Models;
using ProjectFinalEngineer.Models.AggregateMessage;
using ProjectFinalEngineer.Utilities;

namespace ProjectFinalEngineer.Areas.RoomChat.Mapper
{
    public class MessageProfile : Profile
    {
        public MessageProfile()
        {
            CreateMap<Message, MessageViewModel>()
                    .ForMember(dst => dst.From, opt => opt.MapFrom(x => x.FromUser.FullName))
                    .ForMember(dst => dst.Room, opt => opt.MapFrom(x => x.ToRoom.Name))
                    .ForMember(dst => dst.Avatar, opt => opt.MapFrom(x => x.FromUser.Avatar))
                    .ForMember(dst => dst.Content, opt => opt.MapFrom(x => BasicEmojis.ParseEmojis(x.Content)))
                    .ForMember(dst => dst.Timestamp, opt => opt.MapFrom(x => x.Timestamp));
            CreateMap<MessageViewModel, Message>();
        }
    }
}