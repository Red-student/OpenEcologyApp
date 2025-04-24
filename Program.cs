using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using OpenEcologyApp.Data;
using Microsoft.EntityFrameworkCore;
using OpenEcologyApp.Services;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddDbContext<EcologyDbContext>(options =>
    options.UseSqlite("Data Source=app.db"));

// Добавляем CORS
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(policy =>
    {
        policy.AllowAnyOrigin()
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// Добавляем HttpClient с базовым адресом
builder.Services.AddHttpClient("Default", client =>
{
    client.BaseAddress = new Uri("https://localhost:7290/");
});

// Регистрируем сервисы
builder.Services.AddScoped<JsonDataService>();
builder.Services.AddScoped<GrainHarvestService>();

var app = builder.Build();

// Initialize the database
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcologyDbContext>();
        
        // Проверяем, существует ли база данных
        if (!context.Database.CanConnect())
        {
            context.Database.EnsureCreated();
            DbInitializer.Initialize(context);
        }
        
        // Проверяем, что данные добавились
        var count = context.GrainHarvests.Count();
        Console.WriteLine($"База данных инициализирована. Добавлено {count} записей.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseCors();

app.UseEndpoints(endpoints =>
{
    endpoints.MapControllers();
    endpoints.MapBlazorHub();
    endpoints.MapFallbackToPage("/_Host");
});

// Инициализация базы данных
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<EcologyDbContext>();
        context.Database.EnsureCreated();
        
        var harvestService = services.GetRequiredService<GrainHarvestService>();
        await harvestService.InitializeDataAsync();
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "Произошла ошибка при инициализации базы данных.");
    }
}

app.Run();
