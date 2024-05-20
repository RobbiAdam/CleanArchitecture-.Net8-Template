namespace Template.Contract.Dto
{
    public record UserDto(
        string Id,
        string UserName,
        string Name,
        string Email,
        DateTime CreatedAt);

}
