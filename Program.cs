using Infrastructure.Repositories;
using Claims.Domain;
using Claims.Domain.Messages;
using Microsoft.EntityFrameworkCore;
using Claims.Domain.Interfaces;
using Infrastructure;

namespace ClaimsSystem
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddControllers();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddSingleton<IClaimMessagePublisher, ServiceBusClaimPublisher>(); //Service bus
            builder.Services.AddDbContext<ClaimsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("ClaimsDatabase"), 
                sqlOptions => {sqlOptions.MigrationsAssembly("Claims.Api");})); //SQL Db service bus / migrations

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
        }
    }
}