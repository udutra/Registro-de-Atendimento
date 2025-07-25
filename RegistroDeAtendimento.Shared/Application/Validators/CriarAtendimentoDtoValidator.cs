using FluentValidation;
using RegistroDeAtendimento.Shared.Application.Dtos;

namespace RegistroDeAtendimento.Shared.Application.Validators;

public class CriarAtendimentoDtoValidator : AbstractValidator<CriarAtendimentoDto>{
    public CriarAtendimentoDtoValidator(){
        RuleFor(x => x.PacienteId)
            .NotEmpty().WithMessage("O ID do paciente é obrigatório.");

        RuleFor(x => x.DataHora)
            .NotEmpty().WithMessage("A data e hora são obrigatórias.")
            .Must(data => data <= DateTime.Now)
            .WithMessage("A data e hora não podem estar no futuro.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(5000).WithMessage("A descrição é muito longa.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
    
    public Func<CriarAtendimentoDto, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) => {
        var result = await ValidateAsync(ValidationContext<CriarAtendimentoDto>.CreateWithOptions(model, 
            x => x.IncludeProperties(propertyName)));
        return result.IsValid ? [] : result.Errors.Select(e => e.ErrorMessage);
    };
}