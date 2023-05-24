using SystemMonitoringDemo.AutoMapper;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;
using SystemMonitoringDemo.Services.Service;

namespace SystemMonitoringDemo
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddHttpClient();

            builder.Services.AddTransient<IHttpClientExtension, HttpClientExtension>();
            builder.Services.AddTransient<IConvertDataExtension, ConvertDataExtension>();
            builder.Services.AddTransient<WinService>();
            builder.Services.AddTransient<LinuxService>();
            builder.Services.AddTransient(serviceProvider =>
            {
                Func<Type, IOSService> func = key =>
                {
                    if (key == typeof(WinService))
                    {
                        return serviceProvider.GetService<WinService>()!;
                    }
                    else if(key == typeof(LinuxService))
                    {
                        return serviceProvider.GetService<LinuxService>()!;
                    }
                    else
                    {
                        throw new ArgumentException($"不支持的DI Key: {key}");
                    }
                };
                return func;
            });
            builder.Services.AddAutoMapper(typeof(AutoMapperConfig));

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
        }
    }
}