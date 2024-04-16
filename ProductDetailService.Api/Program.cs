using MassTransit;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using ProductDetailService.Api.Repository;
using Steeltoe.Discovery.Client;
using Steeltoe.Discovery.Eureka;
using System.Text;

namespace ProductDetailService.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            //add jwt service
            builder.Services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            })
             .AddJwtBearer(options =>
             {
                 options.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
                 {
                     ValidateIssuer = true,
                     ValidateAudience = true,
                     ValidateLifetime = true,
                     ValidIssuer = builder.Configuration["JWT:Issuer"],
                     ValidAudience = builder.Configuration["JWT:Audience"],
                     IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["JWT:Key"]))
                 };
             });
            builder.Services.AddServiceDiscovery(o => o.UseEureka());

            builder.Services.AddScoped<IProductDetailRepository, ProductDetailRepository>();
            //masstransit service
            builder.Services.AddMassTransit(config =>
            {
                config.AddConsumer<ProductDetailMsgConsumer>();
                config.AddConsumer<ProductRemovedConsumer>();
                config.AddConsumer<ProductUpdatedConsumer>();
                config.UsingRabbitMq((ctx, cfg) =>
                {
                    cfg.Host("amqp://guest:guest@localhost:5672");
                    cfg.ReceiveEndpoint("product-detail-queue", c =>
                    {
                        c.ConfigureConsumer<ProductDetailMsgConsumer>(ctx);
                        c.ConfigureConsumer<ProductRemovedConsumer>(ctx);
                        
                    });
                    cfg.ReceiveEndpoint("product-updated-queue", c =>
                    {
                        c.ConfigureConsumer<ProductUpdatedConsumer>(ctx);

                    });
                });
            });

            builder.Services.AddMassTransitHostedService();
            builder.Services.AddControllers();
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseHttpsRedirection();
            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}