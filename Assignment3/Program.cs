using Assignment3.Hub;
using Assignment3.Models;
using Assignment3.Services.Implement;
using Assignment3.Services.Repository;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddSession();
builder.Services.AddSignalR();
builder.Services.AddDbContext<PRN221_BL5_Assignment3Context>(opt =>
    opt.UseSqlServer(builder.Configuration.GetConnectionString("MyCnn")));
builder.Services.AddTransient<IUser, UserService>();
builder.Services.AddTransient<ICategory, CategoryService>();
builder.Services.AddTransient<IEvent, EventService>();
builder.Services.AddTransient<IAttendee, AttendeeService>();
var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
//app.MapHub<MyHub>("/myHub");
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseSession();
app.UseRouting();
app.UseEndpoints(endpoints =>
{
    endpoints.MapRazorPages();
    endpoints.MapHub<MyHub>("/myHub");
}
);
app.Run();