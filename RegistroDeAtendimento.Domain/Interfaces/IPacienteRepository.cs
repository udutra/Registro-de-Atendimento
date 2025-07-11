using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Domain.Interfaces;

public interface IPacienteRepository{
    Task<Paciente?> ObterPorIdAsync(Guid id);
    IQueryable<Paciente> ObterTodos();
    Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null);
    Task AdicionarAsync(Paciente paciente);
    Task AtualizarAsync(Paciente paciente);
    Task InativarAsync(Paciente paciente);
    Task AtivarAsync(Paciente paciente);
}