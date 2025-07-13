using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Entities;

namespace RegistroDeAtendimento.Application.Interfaces;

public interface IAtendimentoService{
    Task<PagedResponse<List<Atendimento>>> ListarAtendimentosAsync(ListarAtendimentosDto dto);
    Task<Response<Atendimento?>> ObterAtendimentoPorIdAsync(Guid id);
    Task<Response<Atendimento?>> CriarAtendimentoAsync(CriarAtendimentoDto dto);
    Task<Response<Atendimento?>> AtualizarAtendimentoAsync(Guid id, AtualizarAtendimentoDto dto);
    Task<Response<Atendimento?>> InativarAtendimentoAsync(Guid id);
    Task<Response<Atendimento?>> AtivarAtendimentoAsync(Guid id);
}