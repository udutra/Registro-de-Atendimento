using FluentValidation;
using RegistroDeAtendimento.Shared.Application.Dtos;

namespace RegistroDeAtendimento.Shared.Application.Validators;

public class AtualizarAtendimentoDtoValidator : AbstractValidator<AtualizarAtendimentoDto>
{
    public AtualizarAtendimentoDtoValidator()
    {
        When(x => x.PacienteId.HasValue, () => {
            RuleFor(x => x.PacienteId.Value)
                .NotEmpty().WithMessage("O ID do paciente é obrigatório.");
        });

        When(x => x.DataHora.HasValue, () => {
            RuleFor(x => x.DataHora.Value)
                .Must(data => data <= DateTime.UtcNow)
                .WithMessage("A data e hora não podem estar no futuro.");
        });

        When(x => !string.IsNullOrWhiteSpace(x.Descricao), () => {
            RuleFor(x => x.Descricao)
                .MaximumLength(5000).WithMessage("A descrição é muito longa.");
        });
    }
}