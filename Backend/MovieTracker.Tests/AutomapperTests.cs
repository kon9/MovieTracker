using MovieTracker.Infrastructure.Mapper;

namespace MovieTracker.Tests;

public class AutomapperTests
{
    [Fact]
    [Trait("Automapper", "MapperConfiguration")]
    public void ItShouldBeCorrectlyConfigured()
    {
        var cfg = MapperRegistration.GetMapperConfiguration();

        cfg.AssertConfigurationIsValid();
    }
}