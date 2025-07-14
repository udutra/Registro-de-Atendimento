using FluentValidation.TestHelper;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Validators;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Tests.Validators;

public class CriarPacienteDtoValidatorTests{
    private readonly CriarPacienteDtoValidator _validator = new();

    [Fact]
    public void Deve_Passar_Quando_Dados_Sao_Validos(){
        var dto = new CriarPacienteDto{
            Nome = "João Silva",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = "12345678900",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Porto Alegre",
            Bairro = "Centro",
            Logradouro = "Rua das Flores, 123",
            Complemento = "Ap 101",
            Status = StatusEnum.Ativo
        };

        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    [InlineData(null)]
    public void Deve_Falhar_Quando_Nome_E_Vazio(string nome){
        var dto = CriarDtoValido();
        dto.Nome = nome;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Falhar_Quando_Nome_Excede_Maximo_Caracteres(){
        var dto = CriarDtoValido();
        dto.Nome = new string('A', 256);
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Passar_Quando_Nome_Tem_Maximo_Caracteres(){
        var dto = CriarDtoValido();
        dto.Nome = new string('A', 255);
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Nome);
    }

    [Fact]
    public void Deve_Falhar_Quando_DataNascimento_E_Futura(){
        var dto = CriarDtoValido();
        dto.DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.DataNascimento);
    }

    [Fact]
    public void Deve_Passar_Quando_DataNascimento_E_Passada(){
        var dto = CriarDtoValido();
        dto.DataNascimento = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.DataNascimento);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("1234567890")]
    [InlineData("123456789012")]
    [InlineData("1234567890a")]
    [InlineData("123.456.789-00")]
    public void Deve_Falhar_Quando_CPF_E_Invalido(string cpf){
        var dto = CriarDtoValido();
        dto.Cpf = cpf;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Cpf);
    }

    [Theory]
    [InlineData("12345678900")]
    [InlineData("98765432100")]
    [InlineData("11111111111")]
    public void Deve_Passar_Quando_CPF_E_Valido(string cpf){
        var dto = CriarDtoValido();
        dto.Cpf = cpf;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Cpf);
    }

    [Theory]
    [InlineData((SexoEnum)999)]
    public void Deve_Falhar_Quando_Sexo_E_Invalido(SexoEnum sexo){
        var dto = CriarDtoValido();
        dto.Sexo = sexo;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Sexo);
    }

    [Theory]
    [InlineData(SexoEnum.Feminino)]
    [InlineData(SexoEnum.Masculino)]
    [InlineData(SexoEnum.Outro)]
    public void Deve_Passar_Quando_Sexo_E_Valido(SexoEnum sexo){
        var dto = CriarDtoValido();
        dto.Sexo = sexo;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Sexo);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_CEP_E_Vazio(string cep){
        var dto = CriarDtoValido();
        dto.Cep = cep;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Cep);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Cidade_E_Vazia(string cidade){
        var dto = CriarDtoValido();
        dto.Cidade = cidade;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Cidade);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Bairro_E_Vazio(string bairro){
        var dto = CriarDtoValido();
        dto.Bairro = bairro;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Bairro);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Logradouro_E_Vazio(string logradouro){
        var dto = CriarDtoValido();
        dto.Logradouro = logradouro;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Logradouro);
    }

    [Theory]
    [InlineData((StatusEnum)999)]
    public void Deve_Falhar_Quando_Status_E_Invalido(StatusEnum status){
        var dto = CriarDtoValido();
        dto.Status = status;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldHaveValidationErrorFor(x => x.Status);
    }

    [Theory]
    [InlineData(StatusEnum.Ativo)]
    [InlineData(StatusEnum.Inativo)]
    public void Deve_Passar_Quando_Status_E_Valido(StatusEnum status){
        var dto = CriarDtoValido();
        dto.Status = status;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Status);
    }

    [Fact]
    public void Deve_Passar_Quando_Complemento_E_Nulo(){
        var dto = CriarDtoValido();
        dto.Complemento = null;
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Complemento);
    }
    
    [Fact]
    public void Deve_Criar_Endereco_Com_CEP_Com_Tracos(){
        var dto = CriarDtoValido();
        dto.Cep = "12345-678";

        dto.Cep.Should().Be("12345678"); // sem o traço
    }

    [Fact]
    public void Deve_Passar_Quando_Complemento_E_Vazio(){
        var dto = CriarDtoValido();
        dto.Complemento = "";
        var resultado = _validator.TestValidate(dto);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Complemento);
    }

    private static CriarPacienteDto CriarDtoValido(){
        return new CriarPacienteDto{
            Nome = "João Silva",
            DataNascimento = new DateOnly(1990, 1, 1),
            Cpf = "12345678900",
            Sexo = SexoEnum.Masculino,
            Cep = "12345678",
            Cidade = "Porto Alegre",
            Bairro = "Centro",
            Logradouro = "Rua das Flores, 123",
            Complemento = "Ap 101",
            Status = StatusEnum.Ativo
        };
    }
}