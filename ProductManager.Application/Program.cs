//var builder = WebApplication.CreateBuilder(args);
//var app = builder.Build();

//app.MapGet("/", () => "Hello World!");

//app.Run();

using Microsoft.EntityFrameworkCore;
using ProductManager.Application.Models;
using ProductManager.Application.Models.DBEntities;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();//set up shared objects for MVC framework
builder.Services.AddDbContext<PredpriyatieDBContext>(options => //allow connect to database by using connection string (defined in appsettings json)
{//where objects and data mapped with hep of context class (PredpriyatieDBContext cs)
    options.UseSqlServer(builder.Configuration["ConnectionStrings:ProductManagerConnection"]);
});
builder.Services.AddScoped<IProductRepository, EFProductRepository>();//create repository service where http requests get it`s own repository object(usual way to use EF Core)
builder.Services.AddScoped<IPricelistRepository, EFPricelistRepository>();//create repository service where http requests get it`s own repository object(usual way to use EF Core)
var app = builder.Build();

app.UseStaticFiles();//use static content from wwwroot folder
app.MapDefaultControllerRoute();//registers MVC framework as source of endpoint by using default convention of mapping requests to classes and methods
SeedData.EnsurePopulated(app);//fill db with sample data values//dotnet ef database drop --force --context PredpriyatieDBContext
app.Run();