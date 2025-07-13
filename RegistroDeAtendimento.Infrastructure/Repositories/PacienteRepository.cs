using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;

namespace RegistroDeAtendimento.Infrastructure.Repositories;

public class PacienteRepository(AppDbContext context) : IPacienteRepository{
    public async Task<Paciente?> ObterPacientePorIdAsync(Guid id){
        return await context.Pacientes.FindAsync(id);
    }

    public IQueryable<Paciente> ObterTodosPacientes(){
        return context.Pacientes.AsQueryable();
    }

    public async Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null){
        return await context.Pacientes.AnyAsync(p => p.Cpf == cpf && (!ignorarId.HasValue || p.Id != ignorarId));
    }

    public async Task AdicionarPacienteAsync(Paciente paciente){
        context.Pacientes.Add(paciente);
        await context.SaveChangesAsync();
    }

    public async Task AtualizarPacienteAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }

    public async Task InativarPacienteAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }

    public async Task AtivarPacienteAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }
}