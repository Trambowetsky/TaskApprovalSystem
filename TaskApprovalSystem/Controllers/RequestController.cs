using Microsoft.AspNetCore.Mvc;
using TaskApprovalSystem.Models;
using TaskApprovalSystem.ViewModels;

namespace TaskApprovalSystem.Controllers;

public class RequestController : Controller
{
    private readonly AppDbContext _context;
    public RequestController(AppDbContext context) {
        _context = context;
    }
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }
    
    [HttpPost]
    [ValidateAntiForgeryToken]
    public IActionResult Create(CreateRequestViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);
        
        
        var request = new Request
        {
            Id = Guid.NewGuid(),
            Title = model.Title,
            Description = model.Description,
            Type = model.Type,
            Status = RequestStatuses.Pending,
            CreatedOn = DateTime.UtcNow
        };
        _context.Requests.Add(request);
        _context.SaveChanges();

        return RedirectToAction(nameof(Index));
    }
}