using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;

namespace RegistroDeAtendimento.Shared.Application.Interfaces;

public interface IPacienteService{
    Task<PagedResponse<List<Paciente>>> ListarPacientesAsync(ListarPacientesDto dto);
    Task<Response<Paciente?>> ObterPacientePorIdAsync(Guid id);
    Task<Response<Paciente?>> CriarPacienteAsync(CriarPacienteDto dto);
    Task<Response<Paciente?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto);
    Task<Response<Paciente?>> InativarPacienteAsync(Guid id);
    Task<Response<Paciente?>> AtivarPacienteAsync(Guid id);
}