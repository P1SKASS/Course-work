using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Shop.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<SiteContex>(options =>
{
    options.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Course-work;Integrated Security=True;Encrypt=True");
});

var app = builder.Build();

var optionsBuilder = new DbContextOptionsBuilder<SiteContex>();
optionsBuilder.UseSqlServer(@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Course-work;Integrated Security=True;Encrypt=True");

using (var context = new SiteContex(optionsBuilder.Options))
{
    var users = context.Users.ToList();

    Console.WriteLine("Users:");
    foreach (var user in users)
    {
        Console.WriteLine($"Id: {user.Id},Name: {user.Login} ,Email: {user.Mail}, Password: {user.Password}");
    }
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Account}/{action=Login}/{id?}");

app.Run();