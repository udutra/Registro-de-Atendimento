using FluentValidation;
using RegistroDeAtendimento.Shared.Application.Dtos;

namespace RegistroDeAtendimento.Shared.Application.Validators;

public class CriarPacienteDtoValidator : AbstractValidator<CriarPacienteDto>{
    public CriarPacienteDtoValidator(){
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(255);

        RuleFor(p => p.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("A data de nascimento deve ser anterior à data atual.");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Length(11).WithMessage("O CPF deve ter 11 dígitos.")
            .Matches(@"^\d{11}$").WithMessage("O CPF deve conter apenas números.");

        RuleFor(p => p.Sexo)
            .IsInEnum().WithMessage("Sexo inválido.");

        RuleFor(p => p.Cep)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Length(8).WithMessage("O CEP deve conter 8 dígitos.")
            .Matches(@"^\d{8}$").WithMessage("O CEP deve conter apenas números.");

        RuleFor(p => p.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.");

        RuleFor(p => p.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.");

        RuleFor(p => p.Logradouro)
            .NotEmpty().WithMessage("O endereço é obrigatório.");

        RuleFor(p => p.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
    
    public Func<CriarPacienteDto, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) => {
        var result = await ValidateAsync(ValidationContext<CriarPacienteDto>.CreateWithOptions(model, 
            x => x.IncludeProperties(propertyName)));
        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}