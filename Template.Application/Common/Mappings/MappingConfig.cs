using Mapster;
using Template.Contract.Response.Users;
using Template.Domain.Entities;

namespace Template.Application.Common.Mappings
{
    public static class MappingConfig
    {
        public static void Configure()
        {
            TypeAdapterConfig<User, GetCurrentUserResponse>.NewConfig()
                .Map(dest => dest.UserDto, src => src);
        }
    }
}
