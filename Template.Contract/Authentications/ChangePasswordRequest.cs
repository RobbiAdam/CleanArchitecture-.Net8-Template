namespace Template.Contract.Authentications
{
    public record ChangePasswordRequest(
        string OldPassword,
        string NewPassword);
    
}
