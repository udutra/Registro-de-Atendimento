

using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;

namespace RegistroDeAtendimento.Infrastructure.Repositories;

public class AtendimentoRepository(AppDbContext context) : IAtendimentoRepository{
    public async Task AdicionarAtendimentoAsync(Atendimento atendimento){
        context.Atendimentos.Add(atendimento);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarAtendimentoAsync(Atendimento atendimento){
        context.Atendimentos.Update(atendimento);
        await context.SaveChangesAsync();
    }
    
    public async Task<Atendimento?> ObterAtendimentoPorIdAsync(Guid id){
        return await context.Atendimentos
            .Include(a => a.Paciente)
            .FirstOrDefaultAsync(a => a.Id == id);
    }
    
    public IQueryable<Atendimento> ObterTodosAtendimentos(){
        return context.Atendimentos.AsQueryable();
    }
}