using AutoMapper;
using Microsoft.Extensions.Logging;
using MovieTracker.Core.DTOs;
using MovieTracker.Core.Entities;
using MovieTracker.Infrastructure.Mapping;

namespace MovieTracker.Tests
{
    public static class TestHelpers
    {
        public static IMapper CreateTestMapper()
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.AddProfile<AutoMapperProfile>();
            }, new LoggerFactory());

            return config.CreateMapper();
        }
    }
}
