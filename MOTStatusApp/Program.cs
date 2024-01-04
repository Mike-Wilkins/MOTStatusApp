
using Microsoft.EntityFrameworkCore;
using MOTStatusWebApi.Controllers;
using MOTStatusWebApi.Data;
using MOTStatusWebApi.Interfaces;
using MOTStatusWebApi.Repository;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddInfrastructure();
builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddScoped<IMOTStatusDetailsRepository, MOTStatusDetailsRepository>();
builder.Services.AddScoped<IMOTTestCertificateDetailsRepository, MOTTestCertificateDetailsRepository>();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<DataContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
