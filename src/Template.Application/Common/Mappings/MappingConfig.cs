using Mapster;
using Template.Contract.Responses.Users;
using Template.Domain.Users;

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
