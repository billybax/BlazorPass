using BlazorPass.Hubs;
using BlazorPass.Services;
using BlazorPass.Api.Endpoints;
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

// Add Swagger/OpenAPI
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// Добавляем конвертер глобально для Minimal APIs
builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.Converters.Add(new UniversalSystemDateTimeConverter());
});

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
else
{
	// Enable Swagger in Development
	app.UseSwagger();
	app.UseSwaggerUI(c =>
	{
		c.SwaggerEndpoint("/swagger/v1/swagger.json", "BlazorPass API v1");
		c.RoutePrefix = "swagger";
	});
}

app.UseStaticFiles();
app.UseRouting();

app.MapBlazorHub(); // ✅ Базовый хаб для Blazor
app.MapHub<TableUpdateHub>("/tableupdatehub"); // ✅ Ваш кастомный хаб для обновлений

app.MapGet("/", context => {
	context.Response.Redirect("/locpass");
	return Task.CompletedTask;
});

// Map API Endpoints
app.MapTrainingHistoryEndpoints();

app.UseAuthorization();
app.MapStaticAssets();
app.MapRazorPages().WithStaticAssets();

// Fallback route should be the last one
app.MapFallbackToPage("/locpass"); 

app.Run();
