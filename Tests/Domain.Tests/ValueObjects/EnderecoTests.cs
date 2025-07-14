using FluentAssertions;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace Domain.Tests.ValueObjects;

public class EnderecoTests{
    [Fact]
    public void Deve_Criar_Endereco_Com_Todos_Valores(){
        var endereco = new Endereco("90660300", "Porto Alegre", "Santo Antônio", "Rua Joaquim Cruz, 300",
            "106B");

        endereco.Cep.Should().Be("90660300");
        endereco.Cidade.Should().Be("Porto Alegre");
        endereco.Bairro.Should().Be("Santo Antônio");
        endereco.Logradouro.Should().Be("Rua Joaquim Cruz, 300");
        endereco.Complemento.Should().Be("106B");
    }

    [Fact]
    public void Dois_Enderecos_Iguais_Devem_Ser_Iguais(){
        var e1 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 2");
        var e2 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 2");

        e1.Should().Be(e2);
        e1.Equals(e2).Should().BeTrue();
    }

    [Fact]
    public void Dois_Enderecos_Diferentes_Devem_Ser_Diferentes(){
        var e1 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 2");
        var e2 = new Endereco("87654321", "Outra", "Bairro", "Rua 1", "Ap 2");

        e1.Should().NotBe(e2);
    }

    [Fact]
    public void Deve_Criar_Endereco_Com_Complemento_Nulo(){
        var endereco = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", null);

        endereco.Complemento.Should().BeNull();
    }

    [Fact]
    public void Deve_Criar_Endereco_Com_Complemento_Vazio(){
        var endereco = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "");

        endereco.Complemento.Should().Be("");
    }

    [Fact]
    public void Deve_Criar_Endereco_Com_Logradouro_Com_Numeros(){
        var endereco = new Endereco("12345678", "Cidade", "Bairro", "Rua das Flores, 123", "Ap 2");

        endereco.Logradouro.Should().Be("Rua das Flores, 123");
    }

    [Fact]
    public void Enderecos_Com_Complementos_Diferentes_Devem_Ser_Diferentes(){
        var e1 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 1");
        var e2 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 2");

        e1.Should().NotBe(e2);
    }

    [Fact]
    public void Enderecos_Com_Complemento_Nulo_E_Vazio_Devem_Ser_Diferentes(){
        var e1 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", null);
        var e2 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "");

        e1.Should().NotBe(e2);
    }

    [Fact]
    public void GetHashCode_Deve_Retornar_Valores_Diferentes_Para_Enderecos_Diferentes(){
        var e1 = new Endereco("12345678", "Cidade", "Bairro", "Rua 1", "Ap 1");
        var e2 = new Endereco("87654321", "Cidade", "Bairro", "Rua 1", "Ap 1");

        e1.GetHashCode().Should().NotBe(e2.GetHashCode());
    }
}