using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.ValueObjects;
using RegistroDeAtendimento.Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class PacienteRepositoryTests : TestBase{
    private readonly PacienteRepository _repository;

    public PacienteRepositoryTests(){
        _repository = new PacienteRepository(Context);
    }

    [Fact]
    public async Task AdicionarAsync_Deve_Salvar_Paciente_No_Banco(){
        var endereco = new Endereco("12345678", "Porto Alegre", "Centro", "Rua das Flores, 123", "Ap 101");
        var paciente = new Paciente("João Silva", new DateOnly(1990, 1, 1), "12345678900", SexoEnum.Masculino, endereco,
            StatusEnum.Ativo);

        await _repository.AdicionarPacienteAsync(paciente);

        var pacienteSalvo = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteSalvo.Should().NotBeNull();
        pacienteSalvo!.Nome.Should().Be("João Silva");
        pacienteSalvo.Cpf.Should().Be("12345678900");
        pacienteSalvo.Status.Should().Be(StatusEnum.Ativo);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Paciente_Quando_Existe(){
        var paciente = await CriarPacienteAsync();
        var pacienteEncontrado = await _repository.ObterPacientePorIdAsync(paciente.Id);
        pacienteEncontrado.Should().NotBeNull();
        pacienteEncontrado!.Id.Should().Be(paciente.Id);
        pacienteEncontrado.Nome.Should().Be(paciente.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Null_Quando_Paciente_Nao_Existe(){
        var idInexistente = Guid.NewGuid();
        var pacienteEncontrado = await _repository.ObterPacientePorIdAsync(idInexistente);
        pacienteEncontrado.Should().BeNull();
    }

    [Fact]
    public async Task ObterTodos_Deve_Retornar_Todos_Os_Pacientes(){
        await CriarPacienteAsync("João", "11111111111");
        await CriarPacienteAsync("Maria", "22222222222");
        await CriarPacienteAsync("Pedro", "33333333333");
        var pacientes = _repository.ObterTodosPacientes().ToList();
        pacientes.Should().HaveCount(3);
        pacientes.Should().Contain(p => p.Nome == "João");
        pacientes.Should().Contain(p => p.Nome == "Maria");
        pacientes.Should().Contain(p => p.Nome == "Pedro");
    }

    [Fact]
    public async Task ExisteCpfAsync_Deve_Retornar_True_Quando_CPF_Existe(){
        const string cpf = "12345678900";
        await CriarPacienteAsync("João");
        var existe = await _repository.ExisteCpfAsync(cpf);
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task ExisteCpfAsync_Deve_Retornar_False_Quando_CPF_Nao_Existe(){
        const string cpfInexistente = "99999999999";
        var existe = await _repository.ExisteCpfAsync(cpfInexistente);
        existe.Should().BeFalse();
    }

    [Fact]
    public async Task ExisteCpfAsync_Deve_Ignorar_Paciente_Especificado_Quando_IgnorarId_Fornecido(){
        var paciente1 = await CriarPacienteAsync();
        await CriarPacienteAsync("Maria");
        var existe = await _repository.ExisteCpfAsync("12345678900", paciente1.Id);
        existe.Should().BeTrue();
    }

    [Fact]
    public async Task AtualizarAsync_Deve_Atualizar_Dados_Do_Paciente(){
        var paciente = await CriarPacienteAsync();
        var novoEndereco = new Endereco("87654321", "São Paulo", "Vila Madalena", "Rua Augusta, 456", "Sala 2");
        paciente.AtualizarDados("João Atualizado", new DateOnly(1985, 5, 15), "98765432100", SexoEnum.Masculino,
            novoEndereco, StatusEnum.Inativo);

        await _repository.AtualizarPacienteAsync(paciente);
        var pacienteAtualizado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteAtualizado.Should().NotBeNull();
        pacienteAtualizado!.Nome.Should().Be("João Atualizado");
        pacienteAtualizado.Cpf.Should().Be("98765432100");
        pacienteAtualizado.Status.Should().Be(StatusEnum.Inativo);
        pacienteAtualizado.Endereco.Cidade.Should().Be("São Paulo");
    }

    [Fact]
    public async Task InativarAsync_Deve_Alterar_Status_Para_Inativo(){
        var paciente = await CriarPacienteAsync();
        paciente.AlterarStatus(StatusEnum.Inativo);
        await _repository.InativarPacienteAsync(paciente);
        var pacienteInativado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteInativado.Should().NotBeNull();
        pacienteInativado!.Status.Should().Be(StatusEnum.Inativo);
    }

    [Fact]
    public async Task AtivarAsync_Deve_Alterar_Status_Para_Ativo(){
        var paciente = await CriarPacienteAsync();
        paciente.AlterarStatus(StatusEnum.Inativo);
        await Context.SaveChangesAsync();
        paciente.AlterarStatus(StatusEnum.Ativo);
        await _repository.AtivarPacienteAsync(paciente);
        var pacienteAtivado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteAtivado.Should().NotBeNull();
        pacienteAtivado!.Status.Should().Be(StatusEnum.Ativo);
    }

    [Fact]
    public async Task ObterTodos_Deve_Retornar_Pacientes_Com_Endereco(){
        await CriarPacienteAsync();
        var pacientes = _repository.ObterTodosPacientes().ToList();
        pacientes.Should().HaveCount(1);
        var paciente = pacientes.First();
        paciente.Endereco.Should().NotBeNull();
        paciente.Endereco.Cidade.Should().Be("Porto Alegre");
        paciente.Endereco.Bairro.Should().Be("Centro");
    }

    [Fact]
    public async Task ObterTodos_Deve_Retornar_Pacientes_Com_Atendimentos(){
        var paciente = await CriarPacienteAsync();
        await CriarAtendimentoAsync(paciente, "Primeira consulta");
        await CriarAtendimentoAsync(paciente, "Segunda consulta");
        var pacientes = _repository.ObterTodosPacientes().Include(p => p.Atendimentos).ToList();
        pacientes.Should().HaveCount(1);
        var pacienteComAtendimentos = pacientes.First();
        pacienteComAtendimentos.Atendimentos.Should().HaveCount(2);
    }
}