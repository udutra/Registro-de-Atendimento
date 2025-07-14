using RegistroDeAtendimento.Core.Domain.Enums;

namespace RegistroDeAtendimento.Shared.Application.Dtos.Responses;

public class PacienteResponseDto{
    public Guid Id { get; set; }
    public string Nome{ get; set; }
    public DateOnly DataNascimento{ get; set; }
    public string Cpf{ get; set; }
    public SexoEnum Sexo{ get; set; }
    public string Cep{ get; set; }
    public string Cidade{ get; set; }
    public string Bairro{ get; set; }
    public string Logradouro{ get; set; }
    public string? Complemento{ get; set; }
    public StatusEnum? Status{ get; set; }
}