using FluentValidation;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Interfaces;

namespace RegistroDeAtendimento.Application.Validators;

public class PacienteValidator : AbstractValidator<Paciente>{
    public PacienteValidator(IPacienteRepository pacienteRepository){
        RuleFor(p => p.Nome)
            .NotEmpty().WithMessage("O nome é obrigatório.");

        RuleFor(p => p.DataNascimento)
            .NotEmpty().WithMessage("A data de nascimento é obrigatória.");

        RuleFor(p => p.Cpf)
            .NotEmpty().WithMessage("O CPF é obrigatório.")
            .Matches(@"^\d{11}$").WithMessage("O CPF deve conter 11 dígitos numéricos.")
            .Must(cpf => !pacienteRepository.ExisteCpfAsync(cpf).Result)
            .WithMessage("Já existe um paciente com este CPF.");

        RuleFor(p => p.Sexo)
            .IsInEnum().WithMessage("Sexo inválido.");

        RuleFor(p => p.Endereco)
            .SetValidator(new EnderecoValidator());

        RuleFor(p => p.Status)
            .IsInEnum().WithMessage("Status inválido.");
    }
}