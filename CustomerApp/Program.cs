using CustomerApp.Interfaces;
using CustomerApp.ViewModels;
using Microsoft.EntityFrameworkCore;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddScoped<IMOTCustomerStatusViewData, MOTCustomerStatusViewData>();
builder.Services.AddScoped<IMOTTestCertificateDetailsRepository, MOTTestCertificateDetailsRepository>();
builder.Services.AddScoped<IMOTStatusDetailsRepository, MOTStatusDetailsRepository>();
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

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
