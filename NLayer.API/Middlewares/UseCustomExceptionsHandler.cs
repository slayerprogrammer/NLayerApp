using Microsoft.AspNetCore.Diagnostics;
using NLayer.Core.DTOs.Custom;
using NLayer.Service.Exceptions;
using System.Text.Json;

namespace NLayer.API.Middlewares
{
    //Bir extensions metot yazabilmek için class ımız static olmak zorundadır !! 
    public static class UseCustomExceptionsHandler
    {
        //metotta static olmak zorundadır.
        public static void UseCustomExceptions(this IApplicationBuilder app)
        {
            //exception fırlatıldığında çalışan middlewaredır ve geriye bir model döner
            app.UseExceptionHandler(config =>
            {
                //API uygulaması olduğu için json döner. Response tipini belirtelim !!
                //Run sonlandırıcı bir middleware dir. Bundan sonrasında geriye döner.
                //Request buraya girdiğinde anda ileriye gitmeden bitirir.
                //Middleware Örn: 1 - 2 - 3 - 4 - 5 ilk middleware 1 den başlar 5 e kadar gelir
                //ve response dönüştükten sonra tekrar uğrar ve çıkar herhangi bir yerde exception
                //varsa daha ileriye gitmez. Ve geriye döner...
                config.Run(async context =>
                {
                    context.Response.ContentType = "application/json";
                    var exceptionFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var statusCode = exceptionFeature.Error switch
                    {
                        ClientSideException => 400,
                        NotFoundException => 404,
                        _ => 500
                    };
                    context.Response.StatusCode = statusCode;
                    var response = CustomResponseDto<NoContentDto>.Fail(statusCode,exceptionFeature.Error.Message);

                    //json kendimiz dönmemiz gerekiyor!! Otomatik Json dönme olayı olmadığı için biz yazıyoruz.
                    //Normalde NewtonSoft kütüphanesini kullanıyorduk. Artık bu konuda bir sınıf geldi. 
                    await context.Response.WriteAsync(JsonSerializer.Serialize(response));
                });

            });
        }
    }
}
