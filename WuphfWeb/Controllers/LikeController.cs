using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using WuphfWeb.Services.IServices;

namespace WuphfWeb.Controllers
{
    public class LikeController : Controller
    {
        private readonly ILikeService _likeService;
        private readonly IMapper _mapper;
        public LikeController(ILikeService likeService, IMapper mapper)
        {
            _likeService = likeService;
            _mapper = mapper;
        }
    }
}
