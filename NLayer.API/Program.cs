using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.Core.DTOs.Product;
using NLayer.Core.Repositories;
using NLayer.Core.Services;
using NLayer.Core.UnitOfWorks;
using NLayer.Repository;
using NLayer.Repository.Repositories;
using NLayer.Repository.UnitOfWorks;
using NLayer.Service.Mapping;
using NLayer.Service.Services;
using NLayer.Service.Validations;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

//Configuration
// Add services to the container.

builder.Services.AddControllers(options => options.Filters.Add(new ValidateFilterAttribute()))
                .AddFluentValidation(x =>
                                            //diğer validator larıda alttaki gibi ekleyip devam edebiliriz.
                                            //x.RegisterValidatorsFromAssemblyContaining<CategoryDtoValidator>()
                                            x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());

//Fluent validations false customs filter enable true (MVC tarafında bunu yapmaya gerek yok !!)
builder.Services.Configure<ApiBehaviorOptions>(options =>
{

    options.SuppressModelStateInvalidFilter = true;
});


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();
builder.Services.AddScoped(typeof(IGenericRepository<>), typeof(GenericRepository<>));
//service katmanında class ımızı oluşturmadığımız için comment ledik 
builder.Services.AddScoped(typeof(IService<>), typeof(Service<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddScoped<IProductRepository, ProductRepository>();
builder.Services.AddScoped<IProductService, ProductService>();

builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
builder.Services.AddScoped<ICategoryService, CategoryService>();

builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
     {
        // her iki kullanımda olur fakat ilerleyen durumda ilgili katman ismi değiştiğinde burayıda değiştirmek gerekiyor bu nedenle altta ki kod daha mantıklıdır. Tip güvenliği açısından önemlidir. 
        //option.MigrationsAssembly("NLayer.Repository");
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
     });
});

//app ile başlayanların hepsi bir middleware dir dostlar :)
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
