using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();

// Configure EF Core with PostgreSQL
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseNpgsql(connectionString));

var app = builder.Build();

// Ensure database is created on startup
//using (var scope = app.Services.CreateScope())
//{
//	var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
//	db.Database.Migrate();
//}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error");
	// The default HSTS value is30 days. You may want to change this for production scenarios1, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.MapGet("/", context => {
	context.Response.Redirect("/locpass");
	return Task.CompletedTask;
});


app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

app.Run();
