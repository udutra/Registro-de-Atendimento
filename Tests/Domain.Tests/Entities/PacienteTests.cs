using FluentAssertions;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class PacienteTests{
    [Fact]
    public void Deve_Criar_Paciente_Com_Dados_Validos(){
        var endereco = new Endereco("90660300", "Porto Alegre", "Santo Antônio", "Rua A, 123", "Ap 101");
        
        var paciente = new Paciente("Guilherme",new DateOnly(1991, 3, 14),"12345678900",SexoEnum.Masculino,
            endereco, StatusEnum.Ativo);
        
        paciente.Nome.Should().Be("Guilherme");
        paciente.Status.Should().Be(StatusEnum.Ativo);
        paciente.Endereco.Cidade.Should().Be("Porto Alegre");
    }
    
    [Fact]
    public void AtualizarDados_Deve_Alterar_Todos_Os_Valores(){
        var paciente = CriarPaciente();
        var novoEndereco = new Endereco("11111111", "Nova Cidade", "Novo Bairro", "Nova Rua", "Ap 2");

        paciente.AtualizarDados("João", new DateOnly(1980, 1, 1), "98765432100", SexoEnum.Masculino, novoEndereco, 
            StatusEnum.Inativo);

        paciente.Nome.Should().Be("João");
        paciente.DataNascimento.Should().Be(new DateOnly(1980, 1, 1));
        paciente.Cpf.Should().Be("98765432100");
        paciente.Sexo.Should().Be(SexoEnum.Masculino);
        paciente.Endereco.Should().Be(novoEndereco);
        paciente.Status.Should().Be(StatusEnum.Inativo);
    }
    
    [Fact]
    public void Paciente_Deve_Inicializar_Com_Lista_De_Atendimentos_Vazia(){
        var paciente = CriarPaciente();

        paciente.Atendimentos.Should().NotBeNull();
        paciente.Atendimentos.Should().BeEmpty();
    }
    

    [Fact]
    public void Inativar_Deve_Alterar_Status_Para_Inativo(){
        var paciente = CriarPaciente();
        paciente.Status = StatusEnum.Inativo;
        paciente.Status.Should().Be(StatusEnum.Inativo);
    }
    
    [Fact]
    public void Atualizar_Com_Mesmos_Dados_Deve_Manter_Os_Valores(){
        var paciente = CriarPaciente();
        var endereco = paciente.Endereco;

        paciente.AtualizarDados(paciente.Nome, paciente.DataNascimento, paciente.Cpf, paciente.Sexo, endereco, paciente.Status);

        paciente.Nome.Should().Be("Guilherme");
    }
    
    [Fact]
    public void Paciente_Deve_Ter_Um_Id_Valido(){
        var paciente = CriarPaciente();

        paciente.Id.Should().NotBe(Guid.Empty);
    }
    
    private static Paciente CriarPaciente() =>
        new("Guilherme", new DateOnly(1991, 3, 14), "02525311086", SexoEnum.Masculino, CriarEndereco(), StatusEnum.Ativo);

    private static Endereco CriarEndereco() =>
        new("90660300", "Porto Alegre", "Santo Antônio", "Rua A, 123", "Ap 101");
}