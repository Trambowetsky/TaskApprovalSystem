using TaskApprovalSystem.Models;

namespace TaskApprovalSystem.Repositories;

public interface IRequestRepository
{
    IEnumerable<Request> GetAll();
    Request? GetById(Guid id);
    void Add(Request request);
    void Update(Request request);
}

public class EFRequestRepository : IRequestRepository
{
    private readonly AppDbContext _context;
    public EFRequestRepository(AppDbContext context)
    {
        _context = context;
    }
    public IEnumerable<Request> GetAll() => _context.Requests.ToList();

    public Request? GetById(Guid id) => _context.Requests.FirstOrDefault(x => x.Id == id);

    public void Add(Request request)
    {
        _context.Requests.Add(request);
        _context.SaveChanges();
    }

    public void Update(Request request)
    {
        _context.Requests.Update(request);
        _context.SaveChanges();
    }
}