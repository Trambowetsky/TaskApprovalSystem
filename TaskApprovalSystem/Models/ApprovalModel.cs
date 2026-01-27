namespace TaskApprovalSystem.Models;

public enum ApprovalDecisions
{
    None = 0,
    Approved = 1,
    Rejected = 2
}
public class Approval
{
    public Guid Id { get; set; }
    public Guid RequestId { get; set; }
    public Guid ApproverId { get; set; }
    public ApprovalDecisions Decision { get; set; }
    public string Comment { get; set; }
    public DateTime DecidedOn { get; set; }
}