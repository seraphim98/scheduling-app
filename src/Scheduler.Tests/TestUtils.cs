using AutoMapper;

namespace Scheduler.Tests;

public class TestUtils
{
    public static IMapper GetMapper()
    {
        var mapperConfig = new MapperConfiguration(mc =>
        {
            mc.AddProfile(new MappingProfile());
        });
        IMapper mapper = mapperConfig.CreateMapper();
        return mapper;
    }
}
