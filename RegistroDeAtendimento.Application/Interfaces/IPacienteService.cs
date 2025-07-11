using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Interfaces;

public interface IPacienteService{
    Task<PagedResponse<List<Paciente>>> ListarTodosAsync(int page, int itemsPerPage, OrderByPacienteEnum orderBy, SortDirectionEnum sort);
    Task<PagedResponse<List<Paciente>>> ListarAtivosAsync(int page, int itemsPerPage, OrderByPacienteEnum orderBy, SortDirectionEnum sort);
    Task<Response<Paciente?>> ObterPorIdAsync(Guid id);
    Task<Response<Paciente?>> CriarAsync(CriarPacienteDto dto);
    Task<Response<Paciente?>> AtualizarAsync(Guid id, AtualizarPacienteDto dto);
    Task<Response<Paciente?>> InativarAsync(Guid id);
    Task<Response<Paciente?>> AtivarAsync(Guid id);
}