using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;

namespace RegistroDeAtendimento.Shared.Application.Interfaces;

public interface IPacienteService{
    Task<PagedResponse<List<PacienteResponseDto>>> ListarPacientesAsync(ListarPacientesDto dto);
    Task<Response<PacienteResponseDto?>> ObterPacientePorIdAsync(Guid id);
    Task<Response<PacienteResponseDto?>> CriarPacienteAsync(CriarPacienteDto dto);
    Task<Response<PacienteResponseDto?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto);
    Task<Response<PacienteResponseDto?>> InativarPacienteAsync(Guid id);
    Task<Response<PacienteResponseDto?>> AtivarPacienteAsync(Guid id);
}