using AgendaCitaOdontologia.Servicios;
using AgendaCitaOdontologia.Servicios.ProyectoU3.Servicios;
using ProyectoU3.Servicios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();

//Custom services
builder.Services.AddTransient<IRepositorioAgenda, RepositorioAgenda>();
builder.Services.AddTransient<IServicioUsuarios, ServicioUsuario>();


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

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();
