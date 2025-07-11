using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Domain.Entities;

namespace RegistroDeAtendimento.Application.Interfaces;

public interface IPacienteService{
    Task<IEnumerable<Paciente>> ListarTodosAsync();
    Task<IEnumerable<Paciente>> ListarAtivosAsync();
    Task<Paciente?> ObterPorIdAsync(Guid id);
    Task<bool> CriarAsync(CriarPacienteDto dto);
    Task<bool> AtualizarAsync(Guid id, AtualizarPacienteDto dto);
    Task<bool> InativarAsync(Guid id);
}