using FluentValidation;
using RegistroDeAtendimento.Application.Dtos;

namespace RegistroDeAtendimento.Application.Validators;

public class CriarAtendimentoDtoValidator : AbstractValidator<CriarAtendimentoDto>{
    public CriarAtendimentoDtoValidator(){
        RuleFor(x => x.PacienteId)
            .NotEmpty().WithMessage("O ID do paciente é obrigatório.");

        RuleFor(x => x.DataHora)
            .NotEmpty().WithMessage("A data e hora são obrigatórias.")
            .Must(data => data <= DateTime.UtcNow)
            .WithMessage("A data e hora não podem estar no futuro.");

        RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("A descrição é obrigatória.")
            .MaximumLength(5000).WithMessage("A descrição é muito longa.");

        RuleFor(x => x.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
}