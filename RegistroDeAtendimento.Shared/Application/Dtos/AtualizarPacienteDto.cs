using FluentValidation;
using RegistroDeAtendimento.Core.Domain.Enums;

namespace RegistroDeAtendimento.Shared.Application.Dtos;

public class AtualizarPacienteDto{
    public string? Nome{ get; set; } = string.Empty;
    public string? Cpf{ get; set; } = string.Empty;
    public DateOnly? DataNascimento{ get; set; }
    public SexoEnum? Sexo{ get; set; }
    private string? _cep;
    public string? Cidade{ get; set; } = string.Empty;
    public string? Bairro{ get; set; } = string.Empty;
    public string? Logradouro{ get; set; } = string.Empty;
    public string? Complemento{ get; set; }
    public StatusEnum? Status{ get; set; }
    
    public string? Cep {
        get => _cep;
        set => _cep = value?.Replace("-", "").Trim();
    }
}