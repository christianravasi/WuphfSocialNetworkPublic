using AutoMapper;
using WuphfWeb.Models.DTO;

namespace WuphfWeb
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            CreateMap<PostDTO,PostCreateDTO>().ReverseMap();
            CreateMap<PostDTO, PostUpdateDTO>().ReverseMap();

            CreateMap<CommentoDTO, CommentoCreateDTO>().ReverseMap();

            CreateMap<LikeDTO, LikeCreateDTO>().ReverseMap();

            CreateMap<ChatDTO, ChatCreateDTO>().ReverseMap();

            CreateMap<MessaggioDTO, MessaggioCreateDTO>().ReverseMap();

            CreateMap<StoriaDTO, StoriaCreateDTO>().ReverseMap();
        }
    }
}
