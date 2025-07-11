using RegistroDeAtendimento.Domain.Entities;

namespace RegistroDeAtendimento.Domain.Interfaces;

public interface IPacienteRepository{
    Task<Paciente?> ObterPorIdAsync(Guid id);
    Task<IEnumerable<Paciente>> ObterTodosAsync();
    Task<IEnumerable<Paciente>> ObterAtivosAsync();
    Task<bool> ExisteCpfAsync(string cpf, Guid? ignorarId = null);
    Task AdicionarAsync(Paciente paciente);
    Task AtualizarAsync(Paciente paciente);
    Task InativarAsync(Guid id);
}