using TaskApprovalSystem.Models;
using TaskApprovalSystem.Repositories;

namespace TaskApprovalSystem.Services;

public interface IRequestService
{
    Task<Request> CreateAsync(string title, string description, RequestTypes type, string createdBy);

    Task SubmitAsync(Guid requestId);

    Task ApproveAsync(Guid requestId);

    Task RejectAsync(Guid requestId, string? reason);

    Task CancelAsync(Guid requestId);
    Task<List<Request>> GetMyRequestsAsync(Guid userId);
}

public class RequestService : IRequestService
{
    private readonly IRequestRepository _repository;

    public RequestService(IRequestRepository requestRepository)
    {
        _repository = requestRepository;
    }
    public async Task<Request> CreateAsync(string title, string description, RequestTypes type, string createdBy)
    {
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Title = title,
            Description = description,
            Type = type,
            Status = RequestStatuses.Draft,
            CreatedOn = DateTime.UtcNow,
            CreatedBy = new User { Name = createdBy }
        };

        await _repository.AddAsync(request);
        return request;
    }

    public async Task SubmitAsync(Guid requestId)
    {
        var request = await _repository.GetByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request not found");

        if (request.Status != RequestStatuses.Draft)
            throw new InvalidOperationException("Only draft requests can be submitted");

        request.Status = RequestStatuses.Pending;

        await _repository.UpdateAsync(request);
    }

    public async Task ApproveAsync(Guid requestId)
    {
        var request = await _repository.GetByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request not found");

        if (request.Status != RequestStatuses.Pending)
            throw new InvalidOperationException("Only pending requests can be approved");

        request.Status = RequestStatuses.Approved;

        await _repository.UpdateAsync(request);
    }

    public async Task RejectAsync(Guid requestId, string? reason)
    {
        var request = await _repository.GetByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request not found");

        if (request.Status != RequestStatuses.Pending)
            throw new InvalidOperationException("Only pending requests can be rejected");

        request.Status = RequestStatuses.Rejected;

        await _repository.UpdateAsync(request);
    }

    public async Task CancelAsync(Guid requestId)
    {
        var request = await _repository.GetByIdAsync(requestId)
                      ?? throw new InvalidOperationException("Request not found");

        if (request.Status is not (RequestStatuses.Draft or RequestStatuses.Pending))
            throw new InvalidOperationException("Only draft or pending requests can be cancelled");

        request.Status = RequestStatuses.Cancelled;

        await _repository.UpdateAsync(request);
    }
    public async Task<List<Request>> GetMyRequestsAsync(Guid userId)
    {
        return await _repository.GetByUserIdAsync(userId);
    }
}