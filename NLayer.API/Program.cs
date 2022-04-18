using NLayer.Repository;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository.UnitOfWorks;
using NLayer.Core.Repositories;
using NLayer.Repository.Repositories;
using NLayer.Core.Services;

var builder = WebApplication.CreateBuilder(args);

//Configuration
// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>),typeof(GenericRepository<>));
//service katmanında class ımızı oluşturmadığımız için comment ledik 
//builder.Services.AddScoped(typeof(IService<>),typeof(Service<>));
builder.Services.AddDbContext<AppDbContext>(x=>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"),option =>
    {
        // her iki kullanımda olur fakat ilerleyen durumda ilgili katman ismi değiştiğinde burayıda değiştirmek gerekiyor bu nedenle altta ki kod daha mantıklıdır. Tip güvenliği açısından önemlidir. 
        //option.MigrationsAssembly("NLayer.Repository");
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
    });
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
