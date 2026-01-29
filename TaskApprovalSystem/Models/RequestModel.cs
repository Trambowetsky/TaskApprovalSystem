namespace TaskApprovalSystem.Models;

public enum RequestTypes
{
    Vacation = 1,
    SickLeave = 2,
    EquipmentPurchase = 3,
    BusinessTrip = 4,
    Other = 5
}

public enum RequestStatuses
{
    Draft = 1,
    Pending = 2,
    Approved = 3,
    Rejected = 4,
    Cancelled = 5
}

public class Request
{
    public Guid Id { get; set; }
    public required string Title { get; set; }
    public RequestTypes Type { get; set; }
    public string Description { get; set; }
    public RequestStatuses Status { get; set; }
    public DateTime CreatedOn { get; set; }
    public Guid CreatedById { get; set; }
    public User CreatedBy { get; set; }
    public DateTime? UpdatedOn { get; set; }
    public Guid? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }
    public string? ManagerComment { get; set; }
}