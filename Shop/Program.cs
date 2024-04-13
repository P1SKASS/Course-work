using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Shop.Models;
using System;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var optionsBuilder = new DbContextOptionsBuilder<SiteContex>();

builder.Services.AddDbContext<SiteContex>(options =>
{
    options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Course-work;Integrated Security=True;Encrypt=True");
});


builder.Services.AddSession(options =>
{
    options.Cookie.Name = ".Shop.Session";
    options.IdleTimeout = TimeSpan.FromSeconds(3600);
    options.Cookie.HttpOnly = true;
    options.Cookie.IsEssential = true;
});
var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    var context = services.GetRequiredService<SiteContex>();
    var users = context.Users.ToList();

        Console.WriteLine("Users:");
    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user.Id}, Login: {user.Login}, Email: {user.Mail}, Password: {user.Password}, Entrepreneur: {user.Entrepreneur}");
    }
}


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseSession();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();
