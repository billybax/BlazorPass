using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

public class EditLocPassModel : PageModel
{
 private readonly ApplicationDbContext _db;
 public EditLocPassModel(ApplicationDbContext db) => _db = db;

 [BindProperty]
 public LocPass Item { get; set; } = new LocPass();

 public bool IsNew => Item.Id ==0;

	public async Task<IActionResult> OnGetAsync(int id)
	{
		if (id == 0)
		{
			Item = new LocPass();
			return Page();
		}
		var existing = await _db.LocPasses.FindAsync(id);
		if (existing == null) return NotFound();
		Item = existing;
		return Page();
	}

	public async Task<IActionResult> OnPostAsync()
	{
		if (!ModelState.IsValid) return Page();
		if (Item.Id == 0)
		{
			_db.LocPasses.Add(Item);
		}
		else
		{
			var ex = await _db.LocPasses.FindAsync(Item.Id);
			if (ex == null) return NotFound();
			ex.ResName = Item.ResName;
			ex.Login = Item.Login;
			ex.Pass = Item.Pass;
			ex.Comment = Item.Comment;
			_db.Entry(ex).State = EntityState.Modified;
		}
		await _db.SaveChangesAsync();
		return LocalRedirect("~/locpass");
	}
}