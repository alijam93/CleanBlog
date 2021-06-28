using AutoMapper;

using CleanBlog.Domain.Identity;
using CleanBlog.Shared.Dtos.Identity;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CleanBlog.Service.Mapping
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDTO>();
            CreateMap<User, UserUpdateDTO>();
        }
    }
}
