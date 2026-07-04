using BlazorPass.Hubs;
using BlazorPass.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddHttpClient();
builder.Services.AddHttpClient<GeminiService>();
builder.Services.AddServerSideBlazor();
builder.Services.AddSignalR();
builder.Services.AddScoped<PasswordEntryService>();
builder.Services.AddScoped<GeminiService>();

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
	// The default HSTS value is30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
	app.UseHttpsRedirection();
}
app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub(); // ✅ Базовый хаб для Blazor
app.MapHub<TableUpdateHub>("/tableupdatehub"); // ✅ Ваш кастомный хаб для обновлений

app.MapGet("/", context => {
	context.Response.Redirect("/locpass");
	return Task.CompletedTask;
});

app.MapGet("/api/training-history", async (ApplicationDbContext db) =>
{
	var trainingHistory = await db.TrainingHistories
		.Where(t => !t.IsDeleted)
		.OrderByDescending(t => t.TrainDate)
		.ThenByDescending(t => t.TrainTime)
		.Select(t => new 
		{ 
			t.Id,
			t.TrainDate,
			t.TrainTime,
			t.Minutes,
			t.Train,
			t.KpBefore,
			t.KpAfter,
			t.RFaktor,
			t.Practise,
			t.Sleep,
			t.Health,
			t.ClientId,
			t.UpdatedAt,
			t.IsDeleted
		})
		.ToListAsync();
	return Results.Ok(trainingHistory);
});

app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

// Fallback route should be the last one
app.MapFallbackToPage("/locpass"); 

app.Run();
