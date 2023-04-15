using Microsoft.AspNetCore.ResponseCompression;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using MonitorPet.Infrastructure.Extension.Di;
using MonitorPet.Ui.Server.Context;
using MonitorPet.Ui.Server.Factories;
using MonitorPet.Ui.Server.Options;
using MonitorPet.Ui.Server.Security;
using MonitorPet.Ui.Server.Security.JwtBearerConfigurationExtension;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddSwaggerGen();
builder.Services.AddControllers(options => {
    options.Filters.Add<MonitorPet.Ui.Server.Filters.ExceptionErrorViewFilter>();
});
builder.Services.AddRazorPages();
builder.Services.AddHttpContextAccessor();
builder.Services.AddInfrastructure();

builder.Services.AddScoped<MonitorPet.Infrastructure.Factories.IUrlUserEmailFactory, UrlUserEmailFactory>();
builder.Services.AddScoped<MonitorPet.Infrastructure.Factories.IUrlChangePasswordFactory, UrlChangePasswordFactory>();
builder.Services.AddScoped<HttpContextClaim>();
builder.Services.AddScoped<MonitorPet.Application.Security.ContextClaim, HttpContextClaim>();

builder.Services.Configure<MonitorPet.Infrastructure.Options.DbOptions>(
     builder.Configuration.GetSection(MonitorPet.Infrastructure.Options.DbOptions.SECTION));
builder.Services.Configure<MonitorPet.Infrastructure.Options.EmailOptions>(
     builder.Configuration.GetSection(MonitorPet.Infrastructure.Options.EmailOptions.SECTION));
builder.Services.AddOptions<MonitorPet.Ui.Server.Options.JwtOptions>()
    .Bind(builder.Configuration.GetSection(MonitorPet.Ui.Server.Options.JwtOptions.SECTION))
    .ValidateDataAnnotations();

builder.Services.AddAutoMapper(cfg =>
{
    cfg.AddProfile<MonitorPet.Ui.Server.Profiles.ViewModelsProfile>();
});

builder.Services.AddAuthentication(x =>
    {
        x.DefaultAuthenticateScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
        x.DefaultChallengeScheme = Microsoft.AspNetCore.Authentication.JwtBearer.JwtBearerDefaults.AuthenticationScheme;
    })
    .AddApiToken(builder.Configuration.GetSection("Jwt:Key").Value)
    .AddEmailToken(builder.Configuration.GetSection("Jwt:KeyEmail").Value);
builder.Services.AddSingleton<MonitorPet.Ui.Server.Security.IApiToken>(
    (provider) => new ApiToken(provider.GetRequiredService<IOptions<JwtOptions>>()));
builder.Services.AddSingleton<MonitorPet.Ui.Server.Security.IEmailToken>(
    (provider) => new EmailToken(provider.GetRequiredService<IOptions<JwtOptions>>()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseWebAssemblyDebugging();
    app.UseSwagger();
    app.UseSwaggerUI();
}
else
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseBlazorFrameworkFiles();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapRazorPages();
app.MapControllers();
app.MapFallbackToFile("index.html");

app.Run();
