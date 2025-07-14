using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Entities;

namespace RegistroDeAtendimento.Application.Interfaces;

public interface IAtendimentoService{
    Task<PagedResponse<List<AtendimentoResponseDto>>> ListarAtendimentosAsync(ListarAtendimentosDto dto);
    Task<Response<AtendimentoResponseDto?>> ObterAtendimentoPorIdAsync(Guid id);
    Task<Response<AtendimentoResponseDto?>> CriarAtendimentoAsync(CriarAtendimentoDto dto);
    Task<Response<AtendimentoResponseDto?>> AtualizarAtendimentoAsync(Guid id, AtualizarAtendimentoDto dto);
    Task<Response<AtendimentoResponseDto?>> InativarAtendimentoAsync(Guid id);
    Task<Response<AtendimentoResponseDto?>> AtivarAtendimentoAsync(Guid id);
}