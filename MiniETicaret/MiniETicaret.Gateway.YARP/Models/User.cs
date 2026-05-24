namespace MiniETicaret.Gateway.YARP.Models;

public class User
{
    public User()
    {
        Id = Guid.CreateVersion7();
    }
    public Guid Id { get; set; }
    public string UserName { get; set; } = default!;
    public string Password { get; set; } = default!;
}
