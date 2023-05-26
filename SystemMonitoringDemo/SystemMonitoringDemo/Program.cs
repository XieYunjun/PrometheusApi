using Microsoft.OpenApi.Models;
using System.Reflection;
using SystemMonitoringDemo.AutoMapper;
using SystemMonitoringDemo.Extensions;
using SystemMonitoringDemo.Services.IService;
using SystemMonitoringDemo.Services.Service;

namespace SystemMonitoringDemo
{
    /// <summary>
    /// Program
    /// </summary>
    public class Program
    {
        /// <summary>
        /// Main
        /// </summary>
        /// <param name="args"></param>
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen((options) =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo { Title = "监测服务器资源API", Version = "v1", Description = "基于Prometheus", });
                var basePath = Path.GetDirectoryName(typeof(Program).Assembly.Location);//获取应用程序所在目录
                var xmlPath = Path.Combine(basePath!, $"{Assembly.GetExecutingAssembly().GetName().Name}.xml");//接口action显示注释
                options.IncludeXmlComments(Path.Combine(basePath!, "MonitorDemoMode.xml"), true);//实体类注释
            });

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
                app.UseSwaggerUI(c =>c.SwaggerEndpoint("/swagger/v1/swagger.json", "监测服务器资源API v1"));
            }

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}