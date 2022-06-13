using Autofac;
using Autofac.Extensions.DependencyInjection;
using FluentValidation.AspNetCore;
using Microsoft.EntityFrameworkCore;
using NLayer.MVC.Filters;
using NLayer.Repository;
using NLayer.Service.Mapping;
using NLayer.Service.Validations;
using NLayer.Web.Modules;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews().AddFluentValidation(x =>
                                            //diğer validator larıda alttaki gibi ekleyip devam edebiliriz.
                                            //x.RegisterValidatorsFromAssemblyContaining<CategoryDtoValidator>()
                                            x.RegisterValidatorsFromAssemblyContaining<ProductDtoValidator>());
//Fluent validations false customs filter enable true (MVC tarafında bunu yapmaya gerek yok !!)

builder.Services.AddMemoryCache();

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

builder.Services.AddScoped(typeof(NotFoundFilter<>));

builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder => containerBuilder.RegisterModule(new RepoServiceModule()));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
    app.UseExceptionHandler("/Home/Error");
{
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
