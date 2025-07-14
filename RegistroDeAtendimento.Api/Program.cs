using System.Text.Json.Serialization;
using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Application.Interfaces;
using RegistroDeAtendimento.Application.Services;
using RegistroDeAtendimento.Application.Validators;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;
using RegistroDeAtendimento.Infrastructure.Repositories;

public class Program{
    public static void Main(string[] args){

        var builder = WebApplication.CreateBuilder(args);


        builder.Services.AddControllers().AddJsonOptions(options => {
            options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter());
        });

        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        if (!builder.Environment.IsEnvironment("Testing"))
        {
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
        }
        

        builder.Services.AddScoped<IPacienteRepository, PacienteRepository>();
        builder.Services.AddScoped<IAtendimentoRepository, AtendimentoRepository>();

        builder.Services.AddScoped<IPacienteService, PacienteService>();
        builder.Services.AddScoped<IAtendimentoService, AtendimentoService>();

        builder.Services.AddValidatorsFromAssemblyContaining<CriarPacienteDtoValidator>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment()){
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();
        app.UseAuthorization();
        app.MapControllers();
        app.Run();
    }
}