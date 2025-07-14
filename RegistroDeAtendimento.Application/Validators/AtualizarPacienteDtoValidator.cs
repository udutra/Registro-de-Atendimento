using FluentValidation;
using RegistroDeAtendimento.Application.Dtos;

namespace RegistroDeAtendimento.Application.Validators;

public class AtualizarPacienteDtoValidator : AbstractValidator<AtualizarPacienteDto>{
    public AtualizarPacienteDtoValidator(){
        RuleFor(p => p.Nome)
            .MaximumLength(255)
            .When(p => p.Nome is not null)
            .WithMessage("O nome não pode ultrapassar 255 caracteres.");

        RuleFor(p => p.DataNascimento)
            .LessThan(DateOnly.FromDateTime(DateTime.Today))
            .When(p => p.DataNascimento is not null)
            .WithMessage("A data de nascimento deve ser anterior à data atual.");

        RuleFor(p => p.Sexo)
            .IsInEnum()
            .When(p => p.Sexo is not null)
            .WithMessage("Sexo inválido.");

        RuleFor(p => p.Cep)
            .NotEmpty()
            .WithMessage("O CEP é obrigatório.")
            .When(p => !string.IsNullOrWhiteSpace(p.Cep));

        RuleFor(p => p.Cidade)
            .NotEmpty()
            .WithMessage("A cidade é obrigatória.")
            .When(p => !string.IsNullOrWhiteSpace(p.Cidade));

        RuleFor(p => p.Bairro)
            .NotEmpty()
            .WithMessage("O bairro é obrigatório.")
            .When(p => !string.IsNullOrWhiteSpace(p.Bairro));

        RuleFor(p => p.Logradouro)
            .NotEmpty()
            .WithMessage("O endereço é obrigatório.")
            .When(p => !string.IsNullOrWhiteSpace(p.Logradouro));

        RuleFor(p => p.Status)
            .IsInEnum()
            .When(p => p.Status is not null)
            .WithMessage("Status inválido.");
    }
}