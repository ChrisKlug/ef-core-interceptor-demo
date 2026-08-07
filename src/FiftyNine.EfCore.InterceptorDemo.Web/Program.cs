using FiftyNine.EfCore.InterceptorDemo.Web;
using FiftyNine.EfCore.InterceptorDemo.Web.Data;
using FiftyNine.EfCore.InterceptorDemo.Web.Data.Interceptors;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddHttpContextAccessor();

builder.Services.AddSingleton<IUsers, InMemoryUsers>();
builder.Services.AddSingleton<IUserContext, HttpContextUserContext>();
builder.Services.AddHostedService<MigrationRunnerService>();
builder.Services.AddHostedService<SeedDataService>();

builder.Services.AddDbContext<DemoDbContext>((sp, options) => {
    options.UseSqlServer(builder.Configuration.GetConnectionString("default"))
           .AddInterceptors(
                new UserContextConnectionInterceptor(
                    sp.GetRequiredService<IUserContext>(),
                    section => builder.Configuration.GetConnectionString(section.ToString())!
                ),
                new ChangeTrackingInterceptor(
                    sp.GetRequiredService<IUserContext>(),
                    TimeProvider.System
                ),
                ProductsProviderInjectionInterceptor.Instance
                // ProductsProviderPropertyInterceptor.Instance
                // ILoadProductsInterceptor.Instance
            );
});

builder.Services.AddAuthentication()
    .AddCookie(options => { options.LoginPath = "/Login"; });

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("Admin", policy => policy.RequireClaim("IsAdmin", "True"));
});

builder.Services.AddRazorPages(options =>
{
    options.Conventions.AuthorizeFolder("/Admin", "Admin");
});

var app = builder.Build();

app.UseStaticFiles();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();

app.Run();