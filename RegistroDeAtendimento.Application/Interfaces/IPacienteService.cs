using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Interfaces;

public interface IPacienteService{
    Task<PagedResponse<List<Paciente>>> ListarPacientesAsync(ListarPacientesDto dto);
    Task<Response<Paciente?>> ObterPacientePorIdAsync(Guid id);
    Task<Response<Paciente?>> CriarPacienteAsync(CriarPacienteDto dto);
    Task<Response<Paciente?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto);
    Task<Response<Paciente?>> InativarPacienteAsync(Guid id);
    Task<Response<Paciente?>> AtivarPacienteAsync(Guid id);
}