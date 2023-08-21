using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

public class Startup
{
    public Startup(IConfiguration configuration)
    {
        Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices(IServiceCollection services)
    {
        // Configuración para usar el servicio de autenticación y autorización de ASP.NET Core
        services.AddAuthentication(options =>
        {
            options.DefaultAuthenticateScheme = "CustomScheme";
            options.DefaultChallengeScheme = "CustomScheme";
        })
        .AddCookie("CustomScheme", options =>
        {
            options.LoginPath = "/Account/Login"; // Ruta a la vista de login
            options.AccessDeniedPath = "/Account/AccessDenied"; // Ruta a la vista de acceso denegado
        });

        services.AddControllersWithViews();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }
        else
        {
            app.UseExceptionHandler("/Home/Error");
        }

        app.UseStaticFiles();

        app.UseRouting();

        // Agregar el middleware de autenticación antes del middleware de enrutamiento
        app.UseAuthentication();

        app.UseAuthorization();

        app.UseEndpoints(endpoints =>
        {
            endpoints.MapControllerRoute(
                name: "default",
                pattern: "{controller=Account}/{action=Login}/{id?}");
        });
    }
}
