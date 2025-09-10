namespace BackendWeb.Auth.Domain
{
    public class User
    {
        public int Id { get; private set; }
        public string FirstName { get; private set; } = string.Empty;
        public string LastName { get; private set; } = string.Empty;
        public string FullName => $"{FirstName} {LastName}".Trim();
        public string Email { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public string Role { get; private set; } = "User";
        public bool IsActive { get; private set; } = true;
        public DateTime CreatedAt { get; private set; }
        public DateTime UpdatedAt { get; private set; }

        protected User() 
        {
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public User(string firstName, string lastName, string email, string passwordHash, string role = "User")
        {
            FirstName = firstName;
            LastName = lastName;
            Email = email;
            PasswordHash = passwordHash;
            Role = role;
            IsActive = true;
            CreatedAt = DateTime.UtcNow;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateProfile(string firstName, string lastName)
        {
            FirstName = firstName;
            LastName = lastName;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdateRole(string role)
        {
            Role = role;
            UpdatedAt = DateTime.UtcNow;
        }

        public void SetActive(bool isActive)
        {
            IsActive = isActive;
            UpdatedAt = DateTime.UtcNow;
        }

        public void UpdatePassword(string passwordHash)
        {
            PasswordHash = passwordHash;
            UpdatedAt = DateTime.UtcNow;
        }
    }

    public interface IUserRepository
    {
        Task<User?> GetByEmailAsync(string email);
        Task<User?> GetByIdAsync(int id);
        Task<List<User>> GetAllAsync();
        Task AddAsync(User user);
        Task UpdateAsync(User user);
    }
}
