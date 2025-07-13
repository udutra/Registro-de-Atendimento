using FluentAssertions;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Exceptions;
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
    public void Criar_Atendimento_Com_Quebra_De_Linha_Deve_Funcionar(){
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

    [Theory]
    [InlineData(StatusEnum.Ativo)]
    [InlineData(StatusEnum.Inativo)]
    public void Deve_Criar_Atendimento_Com_Todos_Os_Status(StatusEnum status){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.Now, "Descrição", status);

        atendimento.Status.Should().Be(status);
    }

    [Fact]
    public void Deve_Criar_Atendimento_Com_Descricao_Vazia(){
        var paciente = CriarPaciente();
        var atendimento = new Atendimento(paciente, DateTime.Now, "", StatusEnum.Ativo);

        atendimento.Descricao.Should().Be("");
    }

    [Fact]
    public void Deve_Criar_Atendimento_Com_Data_Hora_Passada(){
        var paciente = CriarPaciente();
        var dataHoraPassada = DateTime.Now.AddDays(-1);
        var atendimento = new Atendimento(paciente, dataHoraPassada, "Consulta passada", StatusEnum.Ativo);

        atendimento.DataHora.Should().Be(dataHoraPassada);
    }

    [Fact]
    public void Nao_Deve_Permitir_Atendimento_Com_Data_Futura(){
        var paciente = CriarPaciente();
        var dataHoraFutura = DateTime.Now.AddHours(1);

        var act = () => { new Atendimento(paciente, dataHoraFutura, "Consulta futura", StatusEnum.Ativo); };

        act.Should().Throw<DomainException>().WithMessage("A data e hora do atendimento não pode estar no futuro.");
    }

    [Fact]
    public void Deve_Criar_Atendimento_Com_Descricao_Com_Caracteres_Especiais(){
        var paciente = CriarPaciente();
        const string descricao = "Consulta com caracteres especiais: áéíóú çãõ ñ";
        var atendimento = new Atendimento(paciente, DateTime.Now, descricao, StatusEnum.Ativo);

        atendimento.Descricao.Should().Be(descricao);
    }

    private static Paciente CriarPaciente(){
        return new Paciente("Guilherme", new DateOnly(1991, 3, 14), "02525311086", SexoEnum.Masculino, CriarEndereco(),
            StatusEnum.Ativo);
    }

    private static Endereco CriarEndereco(){
        return new Endereco("90660300", "Porto Alegre", "Santo Antônio", "Rua A, 123", "Ap 101");
    }
}