using FluentAssertions;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace Domain.Tests.Entities;

public class AtendimentoTests{
    [Fact]
    public void Criar_Atendimento_Valido_Deve_Preencher_Campos_Corretamente(){
        var paciente = CriarPaciente();
        var dataHora = DateTime.Now;
        var descricao = "Consulta de rotina.";
        var status = StatusEnum.Ativo;

        var atendimento = new Atendimento(paciente, dataHora, descricao, status);

        atendimento.Paciente.Should().Be(paciente);
        atendimento.DataHora.Should().Be(dataHora);
        atendimento.Descricao.Should().Be(descricao);
        atendimento.Status.Should().Be(status);
    }

    [Fact]
    public void Criar_Atendimento_Com_Descricao_Grande_Deve_Funcionar()
    {
        var paciente = CriarPaciente();
        var descricao = "Consulta inicial\nPaciente com queixas de dor abdominal\nPrescrição realizada";

        var atendimento = new Atendimento(paciente, DateTime.Now, descricao, StatusEnum.Ativo);

        atendimento.Descricao.Should().Be(descricao);
    }
    
    [Fact]
    public void Atendimento_Deve_Ter_Um_Id_Valido(){
        var atendimento = new Atendimento(CriarPaciente(), DateTime.Now, "Descrição", StatusEnum.Ativo);

        atendimento.Id.Should().NotBe(Guid.Empty);
    }
    
    private static Paciente CriarPaciente() =>
        new("Guilherme", new DateOnly(1991, 3, 14), "02525311086", SexoEnum.Masculino, CriarEndereco(), StatusEnum.Ativo);

    private static Endereco CriarEndereco() =>
        new("90660300", "Porto Alegre", "Santo Antônio", "Rua A, 123", "Ap 101");
}