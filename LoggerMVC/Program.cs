using NLog;
using NLog.Web;

var logger = LogManager.Setup()
    .LoadConfigurationFromFile("nlog.config.xml")
    .GetCurrentClassLogger();
try
{
    var builder = WebApplication.CreateBuilder(args);

    builder.Logging.ClearProviders();
    builder.Host.UseNLog();

    builder.Services.AddControllersWithViews();

    var app = builder.Build();

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
        pattern: "{controller=Home}/{action=Index}/{id?}");

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "Stopped due to exception");
    throw;
}
finally
{
    LogManager.Shutdown();
}