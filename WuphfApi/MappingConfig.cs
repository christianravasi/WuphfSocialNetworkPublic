using AutoMapper;
using WuphfApi.Models;
using WuphfApi.Models.DTO;

namespace WuphfApi
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<Post, PostDTO>();
            CreateMap<PostDTO, Post>();

            CreateMap<Post, PostCreateDTO>().ReverseMap();
            CreateMap<Post, PostUpdateDTO>().ReverseMap();

            CreateMap<Commento, CommentoDTO>();
            CreateMap<CommentoDTO, Commento>();

            CreateMap<Commento, CommentoCreateDTO>().ReverseMap();

            CreateMap<Like, LikeDTO>();
            CreateMap<LikeDTO, Like>();

            CreateMap<Like, LikeCreateDTO>().ReverseMap();

            CreateMap<Chat, ChatDTO>();
            CreateMap<ChatDTO, Chat>();

            CreateMap<Chat, ChatCreateDTO>().ReverseMap();

            CreateMap<Messaggio, MessaggioDTO>();
            CreateMap<MessaggioDTO, Messaggio>();

            CreateMap<Messaggio, MessaggioCreateDTO>().ReverseMap();

            CreateMap<Storia, StoriaDTO>();
            CreateMap<StoriaDTO, Storia>();

            CreateMap<Storia, StoriaCreateDTO>().ReverseMap();

            CreateMap<ApplicationUser, UserDTO>().ReverseMap();
        }
    }
}
