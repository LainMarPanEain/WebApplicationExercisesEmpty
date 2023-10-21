var builder = WebApplication.CreateBuilder(args);//define web app builder
builder.Services.AddControllersWithViews();//register for all Controllers Viwes
var app = builder.Build();//create web app using builder

//app.MapGet("/", () => "Hello World!");//routing path for URL
app.MapGet("/sayhi", () => "Hi.Nice to meet you!");
app.MapGet("/now", () => DateTime.Now.ToString());
app.MapGet("/me", () => "Eain");

//app.UseRouting();
app.MapControllerRoute(//if run, call the action in the {action} first, like home page
    name: "default",
    pattern: "{controller=home}/{action=index}/{id?}");

app.Run();//run web app
