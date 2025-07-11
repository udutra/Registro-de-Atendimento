using FluentValidation;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Interfaces;

namespace RegistroDeAtendimento.Application.Validators;

public class AtendimentoValidator: AbstractValidator<Atendimento>{
    public AtendimentoValidator(IAtendimentoRepository atendimentoRepository){
        RuleFor(a => a.PacienteId)
            .NotEmpty()
            .WithMessage("O paciente é obrigatório.");

        RuleFor(a => a.DataHora)
            .NotEmpty()
            .WithMessage("A data e hora são obrigatórias.")
            .Must(data => data <= DateTime.Now)
            .WithMessage("A data e hora não podem estar no futuro.");

        RuleFor(a => a.Descricao)
            .NotEmpty()
            .WithMessage("A descrição do atendimento é obrigatória.")
            .WithMessage("A descrição é muito longa.");

        RuleFor(a => a.Status)
            .IsInEnum()
            .WithMessage("Status inválido.");
    }
}