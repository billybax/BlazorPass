using BlazorPass.Hubs;
using BlazorPass.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;

public class LocPassModel : PageModel
{
    private readonly ApplicationDbContext _db;
    private readonly IHubContext<TableUpdateHub> _hubContext;
    private readonly PasswordEntryService _passwordService;
    public List<LocPass> PasswordEntries { get; set; }

    public LocPassModel(ApplicationDbContext db, IHubContext<TableUpdateHub> hubContext, PasswordEntryService passwordService)
    {
        _db = db;
        _hubContext = hubContext;
        _passwordService = passwordService;
    }

    public LocPass[]? Passes { get; set; }

    public async Task OnGetAsync()
    {
        PasswordEntries = await _passwordService.GetPasswordEntriesAsync();
        Passes = await _db.LocPasses.OrderBy(l => l.ResName).ToArrayAsync();
    }

    public async Task<PartialViewResult> OnGetTableDataAsync()
    {
        var passes = await _db.LocPasses.OrderBy(l => l.ResName).ToArrayAsync();
        return Partial("Shared/_PassTablePartial", passes);
    }

    public async Task<IActionResult> OnPostAddAsync(LocPass entry)
    {
        await _passwordService.AddPasswordEntryAsync(entry);
        await _hubContext.Clients.All.SendAsync("RefreshTable");
        return new OkResult();
    }

    public async Task<IActionResult> OnPostDeleteAsync(int id)
    {
        await _passwordService.DeletePasswordEntryAsync(id);
        await _hubContext.Clients.All.SendAsync("RefreshTable");
        return new OkResult();
    }
}
