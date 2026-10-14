namespace Domain.Class;

public class User
{
    public int Id { get; set; }
    public string Name { get; set; }
    public UserRole Role { get; set; }
    
    public enum UserRole
    {
        Admin,
        Acceptor,
        Waiter
    }
}