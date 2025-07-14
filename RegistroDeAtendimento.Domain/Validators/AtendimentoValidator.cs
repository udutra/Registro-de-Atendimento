using FluentValidation;
using RegistroDeAtendimento.Core.Domain.Entities;

namespace RegistroDeAtendimento.Domain.Validators;

public class AtendimentoValidator : AbstractValidator<Atendimento>{
    public AtendimentoValidator(){
        RuleFor(a => a.PacienteId)
            .NotEmpty()
            .WithMessage("O paciente é obrigatório.");

        RuleFor(a => a.DataHora)
            .NotEmpty()
            .WithMessage("A data e hora são obrigatórias.")
            .Must(data => data <= DateTime.UtcNow)
            .WithMessage("A data e hora não podem estar no futuro.");

        RuleFor(a => a.Descricao)
            .NotEmpty()
            .WithMessage("A descrição do atendimento é obrigatória.");

        RuleFor(a => a.Status)
            .IsInEnum()
            .WithMessage("Status inválido.");
    }
}