using FluentValidation;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace RegistroDeAtendimento.Application.Validators;

public class EnderecoValidator : AbstractValidator<Endereco>{
    public EnderecoValidator(){
        RuleFor(p => p.Cep)
            .NotEmpty().WithMessage("O CEP é obrigatório.")
            .Length(8).WithMessage("O CEP deve conter 8 dígitos.")
            .Matches(@"^\d{8}$").WithMessage("O CEP deve conter apenas números.");

        RuleFor(e => e.Cidade)
            .NotEmpty().WithMessage("A cidade é obrigatória.");

        RuleFor(e => e.Bairro)
            .NotEmpty().WithMessage("O bairro é obrigatório.");

        RuleFor(e => e.Logradouro)
            .NotEmpty().WithMessage("O endereço é obrigatório.");
    }
}