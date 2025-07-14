using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Core.Domain.ValueObjects;
using RegistroDeAtendimento.Infrastructure.Data;

namespace Infrastructure.Tests;

public abstract class TestBase : IDisposable{
    protected readonly AppDbContext Context;

    protected TestBase(){
        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseInMemoryDatabase(Guid.NewGuid().ToString())
            .Options;

        Context = new AppDbContext(options);
        Context.Database.EnsureCreated();
    }

    public void Dispose(){
        Context?.Dispose();
    }

    protected async Task<Paciente> CriarPacienteAsync(string nome = "João Silva", string cpf = "12345678900"){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "Ap 101");
        var paciente = new Paciente(nome, new DateOnly(1990, 1, 1), cpf, SexoEnum.Masculino, endereco,
            StatusEnum.Ativo);

        Context.Pacientes.Add(paciente);
        await Context.SaveChangesAsync();

        return paciente;
    }

    protected async Task<Atendimento> CriarAtendimentoAsync(Paciente paciente, string descricao = "Consulta de rotina"){
        var atendimento = new Atendimento(paciente, DateTime.UtcNow, descricao, StatusEnum.Ativo);

        Context.Atendimentos.Add(atendimento);
        await Context.SaveChangesAsync();
        return atendimento;

    }
}