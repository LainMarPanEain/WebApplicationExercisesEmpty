using CRUDMVC.DAO;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var config = builder.Configuration;//create config obj
builder.Services.AddDbContext<StudentDbContext>(o=>o.UseSqlServer(config.GetConnectionString("studentdbcon")));//get the connection string from appsettings.json

var app = builder.Build();
app.MapControllerRoute(
    name: "default",
    pattern: "{Controller=Home}/{Action=Index}/{id?}");

//app.MapGet("/", () => "Hello World!");

app.Run();
