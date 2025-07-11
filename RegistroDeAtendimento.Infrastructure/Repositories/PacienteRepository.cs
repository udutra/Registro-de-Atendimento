using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;

namespace RegistroDeAtendimento.Infrastructure.Repositories;

public class PacienteRepository(AppDbContext context) : IPacienteRepository{
    public async Task<Paciente?> ObterPorIdAsync(Guid id)
        => await context.Pacientes.FindAsync(id);
    
    public IQueryable<Paciente> ObterTodos(){
        return context.Pacientes.AsQueryable();
    }
    
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

    public async Task InativarAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }
    
    public async Task AtivarAsync(Paciente paciente){
        context.Pacientes.Update(paciente);
        await context.SaveChangesAsync();
    }
}