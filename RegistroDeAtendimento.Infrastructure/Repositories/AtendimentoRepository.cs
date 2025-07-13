using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Infrastructure.Data;

namespace RegistroDeAtendimento.Infrastructure.Repositories;

public class AtendimentoRepository(AppDbContext context) : IAtendimentoRepository{
    public async Task AdicionarAtendimentoAsync(Atendimento atendimento){
        context.Antedimentos.Add(atendimento);
        await context.SaveChangesAsync();
    }
    
    public async Task AtualizarAtendimentoAsync(Atendimento atendimento){
        context.Antedimentos.Update(atendimento);
        await context.SaveChangesAsync();
    }
    
    public async Task<Atendimento?> ObterAtendimentoPorIdAsync(Guid id){
        return await context.Antedimentos.FindAsync(id);
    }
    
    public IQueryable<Atendimento> ObterTodosAtendimentos(){
        return context.Antedimentos.AsQueryable();
    }
}