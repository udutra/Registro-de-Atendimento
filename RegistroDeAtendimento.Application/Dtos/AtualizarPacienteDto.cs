using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Application.Dtos;

public class AtualizarPacienteDto{
    public string Nome { get; set; } = string.Empty;
    public string Cpf { get; set; } = string.Empty;
    public DateOnly DataNascimento { get; set; }
    public SexoEnum Sexo { get; set; }
    public string Cep { get; set; } = string.Empty;
    public string Cidade { get; set; } = string.Empty;
    public string Bairro { get; set; } = string.Empty;
    public string Logradouro { get; set; } = string.Empty;
    public string? Complemento { get; set; }
    public StatusEnum Status { get; set; }
}