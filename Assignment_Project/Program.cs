using Assignment_Project.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
//builder.Services.AddDbContext<PRN221_FapProject2Context>(
//    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"))
//    );
builder.Services.AddDbContext<PRN221_FapProjectContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn"))
    );
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
