namespace Template.Contract.Authentications
{
    public record RegisterRequest(
        string UserName,
        string Name,
        string Email,
        string Password);
    
}
