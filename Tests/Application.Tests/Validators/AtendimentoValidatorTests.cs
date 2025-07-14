using FluentValidation.TestHelper;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Core.Domain.Exceptions;
using RegistroDeAtendimento.Core.Domain.ValueObjects;
using RegistroDeAtendimento.Domain.Validators;

namespace RegistroDeAtendimento.Tests.Validators;

public class AtendimentoValidatorTests{
    private readonly AtendimentoValidator _validator = new();

    [Fact]
    public void Deve_Passar_Quando_Atendimento_E_Valido(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", StatusEnum.Ativo);
        var property = typeof(Atendimento).GetProperty("PacienteId");
        property?.SetValue(atendimento, paciente.Id);

        var resultado = _validator.TestValidate(atendimento);

        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Fact]
    public void Deve_Falhar_Quando_PacienteId_E_Vazio(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", StatusEnum.Ativo);
        var property = typeof(Atendimento).GetProperty("PacienteId");
        property?.SetValue(atendimento, Guid.Empty);

        var resultado = _validator.TestValidate(atendimento);

        resultado.ShouldHaveValidationErrorFor(x => x.PacienteId);
    }

    [Fact]
    public void Deve_Passar_Quando_PacienteId_E_Valido(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", StatusEnum.Ativo);
        var property = typeof(Atendimento).GetProperty("PacienteId");
        property?.SetValue(atendimento, paciente.Id);

        var resultado = _validator.TestValidate(atendimento);

        resultado.ShouldNotHaveValidationErrorFor(x => x.PacienteId);
    }
    
    [Fact]
    public void Nao_Deve_Criar_Atendimento_Com_Data_Futura()
    {
        var paciente = CriarPaciente();
        var dataHoraFutura = DateTime.UtcNow.AddHours(1);

        Action act = () => new Atendimento(paciente, dataHoraFutura, "Consulta", StatusEnum.Ativo);

        act.Should().Throw<DomainException>()
            .WithMessage("A data e hora do atendimento não pode estar no futuro.");
    }

    [Fact]
    public void Deve_Passar_Quando_DataHora_E_Passada(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.DataHora);
    }

    [Fact]
    public void Deve_Passar_Quando_DataHora_E_Agora(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow, "Consulta de rotina", StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.DataHora);
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Deve_Falhar_Quando_Descricao_E_Vazia(string descricao){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), descricao, StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Deve_Passar_Quando_Descricao_E_Valida(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1),
            "Consulta de rotina com descrição detalhada",
            StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Deve_Passar_Quando_Descricao_Tem_Caracteres_Especiais(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1),
            "Consulta com caracteres especiais: áéíóú çãõ ñ",
            StatusEnum.Ativo);

        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Theory]
    [InlineData((StatusEnum)999)]
    public void Deve_Falhar_Quando_Status_E_Invalido(StatusEnum status){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", status);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Theory]
    [InlineData(StatusEnum.Ativo)]
    [InlineData(StatusEnum.Inativo)]
    public void Deve_Passar_Quando_Status_E_Valido(StatusEnum status){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), "Consulta de rotina", status);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Deve_Passar_Quando_Descricao_E_Longa(){
        var paciente = CriarPaciente();
        var descricaoLonga = new string('A', 1000);
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddHours(-1), descricaoLonga, StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Descricao);
    }

    [Fact]
    public void Deve_Passar_Quando_DataHora_E_Muito_Antiga(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.UtcNow.AddYears(-10), "Consulta antiga", StatusEnum.Ativo);
        var resultado = _validator.TestValidate(atendimento);
        resultado.ShouldNotHaveValidationErrorFor(x => x.DataHora);
    }

    private static Paciente CriarPaciente(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua da Praia, 123", "Ap 101");
        return new Paciente("João Silva", new DateOnly(1990, 1, 1), "12345678900", SexoEnum.Masculino, endereco,
            StatusEnum.Ativo);
    }
}