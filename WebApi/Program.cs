using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Service;
using WebApi.Binder;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContextPool<BamaTestDbContext>(c => c.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.AddScoped<IReservHourService, ReservHourService>();
builder.Services.AddScoped<IWorkHourService, WorkHourService>();

builder.Services.AddControllers(options =>
{
    options.ModelBinderProviders.Insert(0, new DateOnlyModelBinderProvider());
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseAuthorization();

app.MapControllers();

app.Run();
