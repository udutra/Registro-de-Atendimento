using RegistroDeAtendimento.Core.Domain.Entities;

namespace RegistroDeAtendimento.Domain.Interfaces;

public interface IPacienteRepository{
    Task<Paciente?> ObterPacientePorIdAsync(Guid id);
    IQueryable<Paciente> ObterTodosPacientes();
    Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null);
    Task AdicionarPacienteAsync(Paciente paciente);
    Task AtualizarPacienteAsync(Paciente paciente);
}