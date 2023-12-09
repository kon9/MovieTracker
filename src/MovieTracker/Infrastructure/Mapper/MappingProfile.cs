using AutoMapper;
using MovieTracker.Models;
using MovieTracker.Models.Dto;
using MovieTracker.Models.ViewModels;

namespace MovieTracker.Infrastructure.Mapper;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Movie, MovieDto>().ReverseMap();
        CreateMap<Movie, MovieVm>().ReverseMap();
        CreateMap<AppUser, AppUserDto>().ReverseMap(); 
        CreateMap<Comment, CommentDto>().ReverseMap();
        CreateMap<Comment, CommentVm>()
            .ForMember(
                dest => dest.UserName,
                opt => opt.MapFrom(src => src.AppUser.UserName)
            );
        CreateMap<Rating, RatingDto>().ReverseMap();
        CreateMap<Rating, RatingVm>().ReverseMap();
        //TODO Configure mapper
    }
}