using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RestaurantManagementSystem.DAO;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

var config = builder.Configuration;//create config obj
builder.Services.AddDbContext<RMSDBContext>(o => o.UseSqlServer(config.GetConnectionString("RMSConnectionString")));//get the connection string from appsettings.json

//the following 2 lines are newly added for Authentication and Authorization
builder.Services.AddRazorPages();//for identity functions UIs
//register the Identity for Identity User and Identity Role with related dbContext
builder.Services.AddIdentity<IdentityUser, IdentityRole>().AddEntityFrameworkStores<RMSDBContext>().AddDefaultUI().AddDefaultTokenProviders();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();//for enabling the Authentication and Authorization
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();//for enabling the route path for Authentication and Authorization(Controllers path)

app.UseEndpoints(endpoints =>
{
    // Route for About
    endpoints.MapControllerRoute(
        name: "about",
        pattern: "about",
        defaults: new { controller = "Home", action = "About" });

    // Route for Service
    endpoints.MapControllerRoute(
        name: "service",
        pattern: "service",
        defaults: new { controller = "Home", action = "Service" });

    // Route for Team
    endpoints.MapControllerRoute(
        name: "team",
        pattern: "team",
        defaults: new { controller = "Home", action = "Team" });

    // Route for Testimonial
    endpoints.MapControllerRoute(
        name: "testimonial",
        pattern: "testimonial",
        defaults: new { controller = "Home", action = "Testimonial" });

    endpoints.MapRazorPages();
});


app.Run();
