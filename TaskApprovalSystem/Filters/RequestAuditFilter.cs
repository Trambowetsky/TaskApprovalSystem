using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using TaskApprovalSystem.Models;

namespace TaskApprovalSystem.Filters;

public class RequestAuditFilter : IAsyncActionFilter
{
    private readonly AppDbContext _context;

    public RequestAuditFilter(AppDbContext context)
    {
        _context = context;
    }

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        if (!context.ActionArguments.TryGetValue("id", out var idValue)
            || idValue is not Guid requestId)
        {
            await next();
            return;
        }

        var before = await _context.Requests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == requestId);

        if (before == null)
        {
            await next();
            return;
        }

        var oldStatus = before.Status;

        var resultContext = await next();

        if (resultContext.Exception != null && !resultContext.ExceptionHandled)
            return;

        var after = await _context.Requests
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == requestId);

        if (after == null)
            return;

        if (oldStatus != after.Status)
        {
            var userName = context.HttpContext.User.Identity?.Name ?? "system";

            var log = new AuditLog
            {
                Id = Guid.NewGuid(),
                RequestId = requestId,
                OldStatus = oldStatus,
                NewStatus = after.Status,
                ChangedBy = userName,
                ChangedOn = DateTime.UtcNow
            };

            _context.AuditLogs.Add(log);
            await _context.SaveChangesAsync();
        }
    }
}