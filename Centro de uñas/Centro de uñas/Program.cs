using System.Text.Json.Serialization;
using CentroUnas.Infrastructure.Context;
using CentroUnas.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;

namespace Centro_de_uñas
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddDbContext<CentroUnasContext>(options =>
            {
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
            });

            builder.Services.AddScoped<ClienteRepository>();
            builder.Services.AddScoped<EmpleadoRepository>();
            builder.Services.AddScoped<TecnicaRepository>();
            builder.Services.AddScoped<ServicioRepository>();
            builder.Services.AddScoped<HorarioRepository>();
            builder.Services.AddScoped<TurnoRepository>();
            builder.Services.AddScoped<PagoRepository>();

            builder.Services.AddControllers()
                .AddJsonOptions(options =>
                {
                    options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter(allowIntegerValues: false));
                });

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

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
