namespace TaskApprovalSystem.Models;

public enum UserRoles
{
    Employee = 1,
    Manager = 2,
    Finance = 3,
    Admin = 4
}
public class User
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public UserRoles Role { get; set; }
}