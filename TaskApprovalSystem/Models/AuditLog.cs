using System;

namespace TaskApprovalSystem.Models;

public class AuditLog
{
    public Guid Id { get; set; }

    public Guid RequestId { get; set; }

    public RequestStatuses OldStatus { get; set; }

    public RequestStatuses NewStatus { get; set; }

    public string ChangedBy { get; set; } = default!;

    public DateTime ChangedOn { get; set; }
}