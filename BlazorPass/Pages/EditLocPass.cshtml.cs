using BlazorPass.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

public class EditLocPassModel : PageModel
{
    private readonly PasswordEntryService _passwordService;

    public EditLocPassModel(PasswordEntryService passwordService)
    {
        _passwordService = passwordService;
    }

    [BindProperty]
    public LocPass Item { get; set; } = new LocPass();

    public bool IsNew => Item.Id == 0;

    public async Task<IActionResult> OnGetAsync(int id)
    {
        if (id == 0)
        {
            Item = new LocPass();
            return Page();
        }
        
        var existing = await _passwordService.GetPasswordEntryByIdAsync(id);
        if (existing == null) return NotFound();
        Item = existing;
        return Page();
    }

    public async Task<IActionResult> OnPostAsync()
    {
        if (!ModelState.IsValid) return Page();

        if (Item.Id == 0)
        {
            await _passwordService.AddPasswordEntryAsync(Item);
        }
        else
        {
            await _passwordService.UpdatePasswordEntryAsync(Item);
        }

        return new JsonResult(new { success = true, redirectUrl = Url.Page("/LocPass") });
    }
}
