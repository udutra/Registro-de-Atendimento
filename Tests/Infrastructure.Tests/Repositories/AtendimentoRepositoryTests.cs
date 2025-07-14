using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Infrastructure.Repositories;

namespace Infrastructure.Tests.Repositories;

public class AtendimentoRepositoryTests : TestBase
{
    private readonly AtendimentoRepository _repository;

    public AtendimentoRepositoryTests()
    {
        _repository = new AtendimentoRepository(Context);
    }

    [Fact]
    public async Task AdicionarAsync_Deve_Salvar_Atendimento_No_Banco()
    {
        var paciente = await CriarPacienteAsync();
        var atendimento = new Atendimento(paciente, DateTime.Now.AddMinutes(-10), "Consulta inicial", StatusEnum.Ativo);

        await _repository.AdicionarAtendimentoAsync(atendimento);

        var atendimentoSalvo = await Context.Atendimentos.Include(a => a.Paciente).FirstOrDefaultAsync(a => a.Id == atendimento.Id);
        atendimentoSalvo.Should().NotBeNull();
        atendimentoSalvo!.Descricao.Should().Be("Consulta inicial");
        atendimentoSalvo.PacienteId.Should().Be(paciente.Id);
        atendimentoSalvo.Status.Should().Be(StatusEnum.Ativo);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Atendimento_Quando_Existe()
    {
        var paciente = await CriarPacienteAsync();
        var atendimento = new Atendimento(paciente, DateTime.Now.AddMinutes(-20), "Retorno", StatusEnum.Ativo);
        Context.Atendimentos.Add(atendimento);
        await Context.SaveChangesAsync();

        var atendimentoEncontrado = await _repository.ObterAtendimentoPorIdAsync(atendimento.Id);
        atendimentoEncontrado.Should().NotBeNull();
        atendimentoEncontrado!.Id.Should().Be(atendimento.Id);
        atendimentoEncontrado.Descricao.Should().Be(atendimento.Descricao);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Null_Quando_Atendimento_Nao_Existe()
    {
        var idInexistente = Guid.NewGuid();
        var atendimentoEncontrado = await _repository.ObterAtendimentoPorIdAsync(idInexistente);
        atendimentoEncontrado.Should().BeNull();
    }

    [Fact]
    public async Task ObterTodos_Deve_Retornar_Todos_Os_Atendimentos()
    {
        var paciente = await CriarPacienteAsync();
        var atendimento1 = new Atendimento(paciente, DateTime.Now.AddMinutes(-30), "Primeira consulta", StatusEnum.Ativo);
        var atendimento2 = new Atendimento(paciente, DateTime.Now.AddMinutes(-20), "Segunda consulta", StatusEnum.Ativo);
        Context.Atendimentos.AddRange(atendimento1, atendimento2);
        await Context.SaveChangesAsync();

        var atendimentos = _repository.ObterTodosAtendimentos().ToList();
        atendimentos.Should().HaveCount(2);
        atendimentos.Should().Contain(a => a.Descricao == "Primeira consulta");
        atendimentos.Should().Contain(a => a.Descricao == "Segunda consulta");
    }

    [Fact]
    public async Task AtualizarAsync_Deve_Atualizar_Dados_Do_Atendimento()
    {
        var paciente = await CriarPacienteAsync();
        var atendimento = new Atendimento(paciente, DateTime.Now.AddMinutes(-40), "Consulta antiga", StatusEnum.Ativo);
        Context.Atendimentos.Add(atendimento);
        await Context.SaveChangesAsync();

        atendimento.AtualizarDados(paciente, DateTime.Now.AddMinutes(-10), "Consulta atualizada");
        await _repository.AtualizarAtendimentoAsync(atendimento);

        var atendimentoAtualizado = await Context.Atendimentos.FirstOrDefaultAsync(a => a.Id == atendimento.Id);
        atendimentoAtualizado.Should().NotBeNull();
        atendimentoAtualizado!.Descricao.Should().Be("Consulta atualizada");
        atendimentoAtualizado.DataHora.Should().BeCloseTo(DateTime.Now.AddMinutes(-10), TimeSpan.FromSeconds(5));
    }
    
    [Fact]
    public async Task InativarAsync_Deve_Alterar_Status_Para_Inativo(){
        var paciente = await CriarPacienteAsync();
        var atendimento = await CriarAtendimentoAsync(paciente,"Consulta de teste");
        atendimento.AlterarStatus(StatusEnum.Inativo);

        await _repository.AtualizarAtendimentoAsync(atendimento);
        var atendimentoInativado = await Context.Atendimentos.FirstOrDefaultAsync(p => p.Id == atendimento.Id);
        atendimentoInativado.Should().NotBeNull();
        atendimentoInativado!.Status.Should().Be(StatusEnum.Inativo);
    }

    [Fact]
    public async Task AtivarAsync_Deve_Alterar_Status_Para_Ativo(){
        var paciente = await CriarPacienteAsync();
        var atendimento = await CriarAtendimentoAsync(paciente,"Consulta de teste");
        atendimento.AlterarStatus(StatusEnum.Inativo);
        await _repository.AtualizarAtendimentoAsync(atendimento);
        atendimento.AlterarStatus(StatusEnum.Ativo);
        await _repository.AtualizarAtendimentoAsync(atendimento);
        var atendimentoAtivado = await Context.Atendimentos.FirstOrDefaultAsync(p => p.Id == atendimento.Id);
        atendimentoAtivado.Should().NotBeNull();
        atendimentoAtivado!.Status.Should().Be(StatusEnum.Ativo);
    }
} 