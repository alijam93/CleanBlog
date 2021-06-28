using AutoMapper;

using CleanBlog.Domain.Entities;
using CleanBlog.Shared.Dtos;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Service.Mapping
{
    public class PostProfile : Profile
    {
        public PostProfile()
        {
            CreateMap<Post, PostDTO>().ReverseMap();
            CreateMap<Post, AddPostDTO>().ReverseMap();
            CreateMap<Post, EditPostDTO>().ReverseMap();
        }
    }
}
