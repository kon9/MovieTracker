using System.Reflection;
using AutoMapper;

namespace MovieTracker.Infrastructure.Mapper;

public static class MapperRegistration
{
    public static MapperConfiguration GetMapperConfiguration()
    {
        var profiles = GetProfiles();
        return new MapperConfiguration(cfg =>
        {
            foreach (var profile in profiles.Select(profile => (Profile)Activator.CreateInstance(profile)!))
            {
                cfg.AddProfile(profile);
            }
        });
    }

    private static List<Type> GetProfiles()
    {
        return (from t in typeof(MappingProfile).GetTypeInfo().Assembly.GetTypes()
                where typeof(MappingProfile).IsAssignableFrom(t) && !t.GetTypeInfo().IsAbstract
                select t).ToList();
    }
}