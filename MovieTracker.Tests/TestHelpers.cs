using AutoMapper;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;

namespace MovieTracker.Tests
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                // User mappings
                cfg.CreateMap<User, UserDto>();
                cfg.CreateMap<CreateUserDto, User>();
                cfg.CreateMap<UpdateUserDto, User>();
                
                // Queue mappings
                cfg.CreateMap<Queue, QueueDto>();
                cfg.CreateMap<CreateQueueDto, Queue>();
                cfg.CreateMap<UpdateQueueDto, Queue>();
                
                // QueueItem mappings
                cfg.CreateMap<QueueItem, QueueItemDto>();
                cfg.CreateMap<AddItemToQueueDto, QueueItem>();
                cfg.CreateMap<UpdateQueueItemDto, QueueItem>();
                
                // QueueMember mappings
                cfg.CreateMap<QueueMember, QueueMemberDto>();
                cfg.CreateMap<AddMemberToQueueDto, QueueMember>();
                
                // Movie mappings
                cfg.CreateMap<Movie, MovieDto>();
                cfg.CreateMap<CreateMovieDto, Movie>();
                cfg.CreateMap<UpdateMovieDto, Movie>();
                
                // Comment mappings
                cfg.CreateMap<Comment, CommentDto>();
                cfg.CreateMap<CreateCommentDto, Comment>();
                cfg.CreateMap<UpdateCommentDto, Comment>();
                
                // Rating mappings
                cfg.CreateMap<Rating, RatingDto>();
                cfg.CreateMap<CreateRatingDto, Rating>();
                cfg.CreateMap<UpdateRatingDto, Rating>();
            });

            return config.CreateMapper();
        }
    }
}
