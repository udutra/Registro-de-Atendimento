using RegistroDeAtendimento.Domain.Entities;

namespace RegistroDeAtendimento.Domain.Interfaces;

public interface IAtendimentoRepository{
    
    Task AdicionarAtendimentoAsync(Atendimento atendimento);
    Task<Atendimento?> ObterAtendimentoPorIdAsync(Guid id);
    IQueryable<Atendimento> ObterTodosAtendimentos();
    Task AtualizarAtendimentoAsync(Atendimento atendimento);
    
}