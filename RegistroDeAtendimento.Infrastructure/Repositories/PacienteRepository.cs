using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;

namespace RegistroDeAtendimento.Infrastructure.Repositories;

public class PacienteRepository(AppDbContext context) : IPacienteRepository{
    public async Task<Paciente?> ObterPorIdAsync(Guid id)
        => await context.Pacientes.FindAsync(id);

    public async Task<IEnumerable<Paciente>> ObterTodosAsync()
        => await context.Pacientes.ToListAsync();

    public async Task<IEnumerable<Paciente>> ObterAtivosAsync()
        => await context.Pacientes.Where(p => p.Status == StatusEnum.Ativo).ToListAsync();

    public async Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null)
        => await context.Pacientes.AnyAsync(p => p.Cpf == cpf && (!ignorarId.HasValue || p.Id != ignorarId));

    public async Task AdicionarAsync(Paciente paciente){
        context.Pacientes.Add(paciente);
        await context.SaveChangesAsync();
    }

    public async Task AtualizarAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }

    public async Task InativarAsync(Guid id){
        var paciente = await context.Pacientes.FindAsync(id);
        if (paciente is null) return;

        paciente.Status = StatusEnum.Inativo;
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }
}