using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class LocPassModel : PageModel
{
 private readonly ApplicationDbContext _db;
	public LocPassModel(ApplicationDbContext db)
	{
		_db = db;
	}

	public LocPass[]? Passes { get; set; }

	public async Task OnGetAsync()
	{
		Passes = await _db.LocPasses.OrderBy(l => l.ResName).ToArrayAsync();
	}
}
