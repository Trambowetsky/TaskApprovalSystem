using TaskApprovalSystem.Models;
using TaskApprovalSystem.Repositories;

namespace TaskApprovalSystem.Services;

public interface IRequestService
{
    Task<Request> CreateAsync(string title, string description, RequestTypes type, Guid createdById);

    Task SubmitAsync(Guid requestId);
    Task ApproveAsync(Guid requestId);
    Task RejectAsync(Guid requestId, string? reason);
    Task CancelAsync(Guid requestId);

    Task<List<Request>> GetMyRequestsAsync(Guid userId);
}

public class RequestService : IRequestService
{
    private readonly IRequestRepository _repository;

    public RequestService(IRequestRepository repository)
    {
        _repository = repository;
    }

    public async Task<Request> CreateAsync(
        string title,
        string description,
        RequestTypes type,
        Guid createdById)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Type = type,
            Status = RequestStatuses.Draft,
            CreatedOn = DateTime.UtcNow,
            CreatedById = createdById
        };

        await _repository.AddAsync(request);
        return request;
    }

    public Task SubmitAsync(Guid requestId) =>
        ChangeStatusAsync(requestId, RequestStatuses.Pending);

    public Task ApproveAsync(Guid requestId) =>
        ChangeStatusAsync(requestId, RequestStatuses.Approved);

    public Task RejectAsync(Guid requestId, string? reason)
    {
        // reason можно позже использовать для истории
        return ChangeStatusAsync(requestId, RequestStatuses.Rejected);
    }

    public Task CancelAsync(Guid requestId) =>
        ChangeStatusAsync(requestId, RequestStatuses.Cancelled);

    public async Task<List<Request>> GetMyRequestsAsync(Guid userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }

    private async Task ChangeStatusAsync(Guid requestId, RequestStatuses newStatus)
    {
        var request = await _repository.GetByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request not found");

        if (!RequestStateMachine.CanTransition(request.Status, newStatus))
        {
            throw new InvalidOperationException(
                $"Cannot transition from {request.Status} to {newStatus}");
        }

        request.Status = newStatus;

        await _repository.UpdateAsync(request);
    }
}

public static class RequestStateMachine
{
    public static bool CanTransition(RequestStatuses from, RequestStatuses to)
    {
        return from switch
        {
            RequestStatuses.Draft =>
                to == RequestStatuses.Pending ||
                to == RequestStatuses.Cancelled,

            RequestStatuses.Pending =>
                to == RequestStatuses.Approved ||
                to == RequestStatuses.Rejected ||
                to == RequestStatuses.Cancelled,

            _ => false
        };
    }
}