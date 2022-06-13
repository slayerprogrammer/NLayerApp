using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NLayer.API.Filters;
using NLayer.API.Middlewares;
using NLayer.API.Modules;
using NLayer.Repository;
using NLayer.Service.Mapping;
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
builder.Services.AddMemoryCache();

builder.Services.AddScoped(typeof(NotFoundFilter<>));
builder.Services.AddAutoMapper(typeof(MapProfile));
builder.Services.AddDbContext<AppDbContext>(x =>
{
    x.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"), option =>
     {
        // her iki kullanımda olur fakat ilerleyen durumda ilgili katman ismi değiştiğinde burayıda değiştirmek gerekiyor
        // bu nedenle altta ki kod daha mantıklıdır. Tip güvenliği açısından önemlidir. 
        //option.MigrationsAssembly("NLayer.Repository");
        option.MigrationsAssembly(Assembly.GetAssembly(typeof(AppDbContext)).GetName().Name);
     });
});

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

//app ile başlayanların hepsi bir middleware dir dostlar :)
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCustomExceptions();
app.UseAuthorization();
app.MapControllers();
app.Run();
