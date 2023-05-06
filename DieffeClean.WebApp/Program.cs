using System.Text.Json.Serialization;
using DieffeClean.Domain.Data;
using DieffeClean.Domain.Model;
using DieffeClean.Utils.Email;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using NLog;
using NLog.Web;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("NLog.config")
    .GetCurrentClassLogger();

try
{
    logger.Info("Init main");
    var builder = WebApplication.CreateBuilder(args);
    
    builder.Configuration
        .SetBasePath(builder.Environment.ContentRootPath)
        .AddJsonFile($"appsettings.{builder.Environment.EnvironmentName}.json", true)
        .AddEnvironmentVariables();
    
    builder.Logging.ClearProviders();
    builder.Logging.SetMinimumLevel(Microsoft.Extensions.Logging.LogLevel.Debug);
    builder.Host.UseNLog();
    
    ConfigurationManager Configuration = builder.Configuration;
    // Add services to the container.
    builder.Services.AddDbContext<ApplicationDbContext>(options =>
        options.UseSqlServer(Configuration.GetConnectionString("DieffeCleanConnection"))); // per entity framework
    
    var emailConfig = Configuration
        .GetSection("EmailConfiguration")
        .Get<EmailConfiguration>();
    
    builder.Services.AddSingleton(emailConfig);

    builder.Services.AddIdentity<MyUser, IdentityRole>(opts =>
        {
            opts.SignIn.RequireConfirmedEmail = false;
            opts.SignIn.RequireConfirmedAccount = false;
            opts.Password.RequireDigit = true;
            opts.Password.RequireLowercase = true;
            opts.Password.RequireUppercase = true;
            opts.Password.RequireNonAlphanumeric = true;
            opts.Password.RequiredLength = 8;
        })
        .AddEntityFrameworkStores<ApplicationDbContext>()
        .AddDefaultTokenProviders();

    // Add services to the container.
    builder.Services.AddControllersWithViews()
        .AddJsonOptions(x => x.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles)
        .AddRazorRuntimeCompilation();
    
    builder.Services.ConfigureApplicationCookie(options =>
    {
        options.LoginPath = "/auth/login";
        options.LogoutPath = "/auth/logout";
        options.Events = new CookieAuthenticationEvents
        {
            OnRedirectToLogin = ctx =>
            {
                ctx.Response.Redirect(options.LoginPath);
                return Task.FromResult(0);
            }
        };
    });

    var app = builder.Build();
    
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;

        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
    }

    // Configure the HTTP request pipeline.
    if (!app.Environment.IsDevelopment())
    {
        app.UseExceptionHandler("/Home/Error");
        // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
        app.UseHsts();
    }

    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    
    var serviceProvider = builder.Services.BuildServiceProvider();
    var userManager = serviceProvider.GetService<UserManager<MyUser>>();
    var roleManager = serviceProvider.GetService<RoleManager<IdentityRole>>();

    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        DbInitializer.SeedUsersAndRoles(userManager, roleManager, context); 
    }
    
    logger.Debug("Program Initialized, Running App...");
    
    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped program because of exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}