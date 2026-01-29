using Microsoft.EntityFrameworkCore;
using TaskApprovalSystem.Models;

namespace TaskApprovalSystem.Repositories;

public interface IRequestRepository
{
    Task<List<Request>> GetAllAsync();
    Task<Request?> GetByIdAsync(Guid id);
    Task AddAsync(Request request);
    Task UpdateAsync(Request request);
    Task DeleteAsync(Guid id);
}
public class EfRequestRepository : IRequestRepository
{
    private readonly AppDbContext _context;

    public EfRequestRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Request>> GetAllAsync()
    {
        return await _context.Requests
            .OrderByDescending(x => x.CreatedOn)
            .ToListAsync();
    }

    public async Task<Request?> GetByIdAsync(Guid id)
    {
        return await _context.Requests.FindAsync(id);
    }

    public async Task AddAsync(Request request)
    {
        _context.Requests.Add(request);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(Request request)
    {
        _context.Requests.Update(request);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var entity = await _context.Requests.FindAsync(id);
        if (entity != null)
        {
            _context.Requests.Remove(entity);
            await _context.SaveChangesAsync();
        }
    }
}