using FluentValidation;
using RegistroDeAtendimento.Application.Dtos;

namespace RegistroDeAtendimento.Application.Validators;

public class AtualizarPacienteDtoValidator : AbstractValidator<AtualizarPacienteDto>{
    public AtualizarPacienteDtoValidator(){
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.")
            .MaximumLength(255);

        RuleFor(p => p.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.")
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .WithMessage("A data de nascimento deve ser anterior à data atual.");

        RuleFor(p => p.Sexo)
            .IsInEnum().WithMessage("Sexo inválido.");

        RuleFor(p => p.Cep)
            .NotEmpty().WithMessage("O CEP é obrigatório.");

        RuleFor(p => p.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.");

        RuleFor(p => p.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.");

        RuleFor(p => p.Logradouro)
            .NotEmpty().WithMessage("O endereço é obrigatório.");

        RuleFor(p => p.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
}