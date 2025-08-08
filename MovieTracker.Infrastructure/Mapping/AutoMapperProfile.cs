using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;

namespace MovieTracker.Infrastructure.Mapping
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User mappings
            CreateMap<User, UserDto>();
            CreateMap<CreateUserDto, User>();
            CreateMap<UpdateUserDto, User>();

            // Movie mappings
            CreateMap<Movie, MovieDto>();
            CreateMap<CreateMovieDto, Movie>();
            CreateMap<UpdateMovieDto, Movie>();

            // Queue mappings (generic)
            CreateMap<Queue, QueueDto>()
                .ForMember(dest => dest.OwnerUsername, opt => opt.MapFrom(src => src.Owner.Username));
            CreateMap<CreateQueueDto, Queue>();
            CreateMap<UpdateQueueDto, Queue>();

            // QueueItem mappings
            CreateMap<QueueItem, QueueItemDto>()
                .ForMember(dest => dest.AddedByUsername, opt => opt.MapFrom(src => src.AddedBy.Username))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));
            CreateMap<AddItemToQueueDto, QueueItem>();

            // QueueMember mappings
            CreateMap<QueueMember, QueueMemberDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()));

            // Comment mappings
            CreateMap<Comment, CommentDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<CreateCommentDto, Comment>();
            CreateMap<UpdateCommentDto, Comment>();

            // Rating mappings
            CreateMap<Rating, RatingDto>()
                .ForMember(dest => dest.Username, opt => opt.MapFrom(src => src.User.Username));
            CreateMap<CreateRatingDto, Rating>();
            CreateMap<UpdateRatingDto, Rating>();
        }
    }
} 