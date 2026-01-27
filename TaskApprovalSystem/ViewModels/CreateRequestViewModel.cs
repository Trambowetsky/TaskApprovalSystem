using TaskApprovalSystem.Models;

namespace TaskApprovalSystem.ViewModels;

public class CreateRequestViewModel
{
    public string Title { get; set; }
    public RequestTypes Type { get; set; }
    public string Description { get; set; }
}