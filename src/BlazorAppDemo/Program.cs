using BlazorAppDemo.Components;
using Infrastructure;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddHttpClient();

builder.Services.AddRazorComponents()
	.AddInteractiveServerComponents();

IConfigurationRoot configuration = builder.Configuration;
string connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddInfrastructureServices(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
	app.UseExceptionHandler("/Error", createScopeForErrors: true);
	// The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
	app.UseHsts();
}

app.UseHttpsRedirection();

app.UseAntiforgery();

app.UseStaticFiles();
app.MapRazorComponents<App>()
	.AddInteractiveServerRenderMode();
app.MapControllers();

app.Run();
