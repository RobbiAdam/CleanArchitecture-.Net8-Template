using Template.Domain.Abstractions;

namespace Template.Domain.Users
{
    public static class UserErrors
    {
        public static readonly Error InvalidUser = new("users:invalid-user", "Invalid user");
        public static readonly Error InvalidEmail = new("users:invalid-email", "Invalid email");
        public static readonly Error InvalidEmailOrPassword = new("users:invalid-email-or-password", 
            "Invalid email or password");
        public static readonly Error UserNotFound = new("users:user-not-found", "User not found");
        public static readonly Error InvalidPassword = new("users:invalid-password", "Invalid password");
        public static readonly Error EmailAlreadyExist = new("users:email-already-exist", "Email already exist");
    }
}
