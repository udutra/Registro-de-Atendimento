using FluentValidation.TestHelper;
using RegistroDeAtendimento.Core.Domain.ValueObjects;
using RegistroDeAtendimento.Domain.Validators;

namespace RegistroDeAtendimento.Tests.Validators;

public class EnderecoValidatorTests{
    private readonly EnderecoValidator _validator = new();

    [Fact]
    public void Deve_Passar_Quando_Endereco_E_Valido(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "Ap 101");
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveAnyValidationErrors();
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_CEP_E_Vazio(string cep){
        var endereco = CriarEnderecoValido();
        var enderecoComCepInvalido =
            new Endereco(cep, endereco.Cidade, endereco.Bairro, endereco.Logradouro, endereco.Complemento);
        var resultado = _validator.TestValidate(enderecoComCepInvalido);
        resultado.ShouldHaveValidationErrorFor(x => x.Cep);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Cidade_E_Vazia(string cidade){
        var endereco = CriarEnderecoValido();
        var enderecoComCidadeInvalida = new Endereco(endereco.Cep, cidade, endereco.Bairro, endereco.Logradouro,
            endereco.Complemento);
        var resultado = _validator.TestValidate(enderecoComCidadeInvalida);
        resultado.ShouldHaveValidationErrorFor(x => x.Cidade);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Bairro_E_Vazio(string bairro){
        var endereco = CriarEnderecoValido();
        var enderecoComBairroInvalido = new Endereco(endereco.Cep, endereco.Cidade, bairro, endereco.Logradouro,
            endereco.Complemento);
        var resultado = _validator.TestValidate(enderecoComBairroInvalido);
        resultado.ShouldHaveValidationErrorFor(x => x.Bairro);
    }

    [Theory]
    [InlineData("")]
    [InlineData(null)]
    [InlineData("   ")]
    public void Deve_Falhar_Quando_Logradouro_E_Vazio(string logradouro){
        var endereco = CriarEnderecoValido();
        var enderecoComLogradouroInvalido = new Endereco(endereco.Cep, endereco.Cidade, endereco.Bairro, logradouro,
            endereco.Complemento);
        var resultado = _validator.TestValidate(enderecoComLogradouroInvalido);
        resultado.ShouldHaveValidationErrorFor(x => x.Logradouro);
    }

    [Fact]
    public void Deve_Passar_Quando_Complemento_E_Nulo(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", null);
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Complemento);
    }

    [Fact]
    public void Deve_Passar_Quando_Complemento_E_Vazio(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "");
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Complemento);
    }

 [Fact]
    public void Deve_Passar_Quando_Logradouro_Tem_Numeros(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123, Apto 5", "Ap 101");
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Logradouro);
    }

    [Fact]
    public void Deve_Passar_Quando_Cidade_Tem_Acentos(){
        var endereco = new Endereco("12345678", "São Paulo", "Centro", "Rua das Flores, 123", "Ap 101");
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Cidade);
    }

    [Fact]
    public void Deve_Passar_Quando_Bairro_Tem_Acentos(){
        var endereco = new Endereco("12345678", "Porto Alegre", "São João", "Rua das Flores, 123", "Ap 101");
        var resultado = _validator.TestValidate(endereco);
        resultado.ShouldNotHaveValidationErrorFor(x => x.Bairro);
    }

    [Fact]
    public void Deve_Passar_Quando_Todos_Os_Campos_Obrigatorios_Sao_Preenchidos(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "Ap 101");
        var resultado = _validator.TestValidate(endereco);

        resultado.ShouldNotHaveAnyValidationErrors();
        endereco.Cep.Should().Be("12345678");
        endereco.Cidade.Should().Be("Porto Alegre");
        endereco.Bairro.Should().Be("Centro");
        endereco.Logradouro.Should().Be("Rua das Flores, 123");
        endereco.Complemento.Should().Be("Ap 101");
    }

    private static Endereco CriarEnderecoValido(){
        return new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "Ap 101");
    }
}