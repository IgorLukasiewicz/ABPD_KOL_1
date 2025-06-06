

using APBD_KOL_1.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddScoped<IVisitService, VisitService>();

var app = builder.Build();

app.UseAuthorization();

app.MapControllers();

app.Run();