using Database_Project.Data;
using Database_Project.Repositories;
using Database_Project.Repositories.Interfaces;
using Database_Project.Services;
using Microsoft.EntityFrameworkCore;
using Database_Project.Models;
using Microsoft.AspNetCore.Identity;
using Database_Project.Services.Interfaces;
using Database_Project.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddRazorPages();

// Register your application's DbContext
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("LibraryDBConnection")));

// Replace AddDefaultIdentity with AddIdentity to properly include RoleManager
builder.Services.AddIdentity<User, IdentityRole<int>>(options =>
{
    options.SignIn.RequireConfirmedAccount = false;
    // You can add other identity options here if needed
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders();

// Register repositories and services
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
builder.Services.AddScoped<IBranchService, BranchService>();
builder.Services.AddScoped<IBookRepository, BookRepository>();
builder.Services.AddScoped<IBookService, BookService>();

// Add Unwanted Customers services
builder.Services.AddScoped<IUnwantedCustomersRepository, UnwantedCustomersRepository>();
builder.Services.AddScoped<IUnwantedCustomersService, UnwantedCustomersService>();

// Add Book Stock services
builder.Services.AddScoped<IBookStockRepository, BookStockRepository>();
builder.Services.AddScoped<IBookStockService, BookStockService>();
builder.Services.AddScoped<IImageService, ImageService>();

// Add Role Management Service
builder.Services.AddScoped<IRoleManagerService, RoleManagerService>();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole<int>>>();
    var roles = new[] { "Member", "Librarian", "Admin" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
            await roleManager.CreateAsync(new IdentityRole<int>(role));
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

app.MapStaticAssets();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}")
    .WithStaticAssets();

app.MapRazorPages();

app.Run();
