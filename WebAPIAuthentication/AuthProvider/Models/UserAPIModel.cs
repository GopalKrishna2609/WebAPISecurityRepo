namespace AuthProvider.Models
{
    public class UserAPIModel
    {
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? Password { get; set; }

        public UserRole RoleType { get; set; }

    }

    public enum UserRole
    {
        Admin = 1,
        Viewer =2
    }
}

