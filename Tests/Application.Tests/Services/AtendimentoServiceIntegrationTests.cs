using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Services;
using RegistroDeAtendimento.Application.Validators;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.ValueObjects;
using RegistroDeAtendimento.Infrastructure.Repositories;

namespace RegistroDeAtendimento.Tests.Services;

public class AtendimentoServiceIntegrationTests : TestBase
{
    private readonly AtendimentoService _service;

    public AtendimentoServiceIntegrationTests()
    {
        var atendimentoRepository = new AtendimentoRepository(Context);
        var pacienteRepository = new PacienteRepository(Context);
        IValidator<CriarAtendimentoDto> criarValidator = new CriarAtendimentoDtoValidator();
        IValidator<AtualizarAtendimentoDto> atualizarValidator = new AtualizarAtendimentoDtoValidator();
        _service = new AtendimentoService(atendimentoRepository, pacienteRepository, criarValidator, atualizarValidator);
    }

    [Fact]
    public async Task CriarAsync_Deve_Criar_Atendimento_Com_Dados_Validos()
    {
        var paciente = await CriarPacienteAsync();
        var dto = new CriarAtendimentoDto{
            PacienteId = paciente.Id,
            DataHora = DateTime.Now,
            Descricao = "Primeira consulta",
            Status = StatusEnum.Ativo
        };

        var resultado = await _service.CriarAtendimentoAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(201);
        resultado.Message.Should().Be("Atendimento criado com sucesso.");
        resultado.Data.Should().NotBeNull();
        resultado.Data!.PacienteId.Should().Be(paciente.Id);
    }

    [Fact]
    public async Task CriarAsync_Deve_Retornar_Erro_Quando_Paciente_Nao_Existe(){
        var dto = new CriarAtendimentoDto{
            PacienteId = Guid.CreateVersion7(),
            DataHora = DateTime.Now,
            Descricao = "Primeira consulta",
            Status = StatusEnum.Ativo
        };

        var resultado = await _service.CriarAtendimentoAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(404);
        resultado.Message.Should().Be("Paciente não encontrado.");
        resultado.Data.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Atendimento_Quando_Existe()
    {
        var atendimento = await CriarAtendimentoAsync();

        var resultado = await _service.ObterAtendimentoPorIdAsync(atendimento.Id);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(200);
        resultado.Message.Should().Be("Atendimento localizado com sucesso.");
        resultado.Data.Should().NotBeNull();
        resultado.Data!.Id.Should().Be(atendimento.Id);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Erro_Quando_Atendimento_Nao_Existe()
    {
        var idInexistente = Guid.NewGuid();

        var resultado = await _service.ObterAtendimentoPorIdAsync(idInexistente);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(404);
        resultado.Message.Should().Be("Atendimento não encontrado.");
        resultado.Data.Should().BeNull();
    }

    [Fact]
    public async Task AtualizarAsync_Deve_Atualizar_Atendimento_Com_Dados_Validos()
    {
        var atendimento = await CriarAtendimentoAsync();
        var dto = new AtualizarAtendimentoDto
        {
            PacienteId = atendimento!.PacienteId, // Corrigido: usa o paciente existente
            DataHora = DateTime.Now,
            Descricao = "Retorno agendado"
        };

        var resultado = await _service.AtualizarAtendimentoAsync(atendimento.Id, dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(204);
        resultado.Message.Should().Be("Atendimento atualizado com sucesso.");

        var atendimentoAtualizado = await Context.Atendimentos.FirstOrDefaultAsync(a => a.Id == atendimento.Id);
        atendimentoAtualizado.Should().NotBeNull();
        atendimentoAtualizado?.Descricao.Should().Be("Retorno agendado");
    }

    [Fact]
    public async Task ListarTodosAsync_Deve_Retornar_Atendimentos_Paginados()
    {
        var paciente = await CriarPacienteAsync();
        await CriarAtendimentoAsync(paciente.Id);
        await CriarAtendimentoAsync(paciente.Id);

        var dto = new ListarAtendimentosDto
        {
            Page = 1,
            PageSize = 1,
            OrderBy = OrderByAtendimentoEnum.DataHora,
            Sort = SortDirectionEnum.Asc
        };

        var resultado = await _service.ListarAtendimentosAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Data.Should().HaveCount(1);
        resultado.TotalCount.Should().BeGreaterThan(1);
        resultado.CurrentPage.Should().Be(1);
        resultado.PageSize.Should().Be(1);
    }

    private new async Task<Paciente> CriarPacienteAsync(string nome = "Paciente Teste", string cpf = "12345678900")
    {
        var paciente = new Paciente(nome, new DateOnly(1990, 1, 1), cpf, SexoEnum.Masculino,
            new Endereco("12345678", "Cidade", "Bairro", "Rua", ""), StatusEnum.Ativo);
        Context.Pacientes.Add(paciente);
        await Context.SaveChangesAsync();
        return paciente;
    }

    private async Task<Atendimento?> CriarAtendimentoAsync(Guid? pacienteId = null)
    {
        var paciente = pacienteId.HasValue
            ? await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == pacienteId.Value)
            : await CriarPacienteAsync();

        if (paciente == null) return null;
        
        var atendimento = new Atendimento(paciente, DateTime.Now, "Descrição", StatusEnum.Ativo);
        Context.Atendimentos.Add(atendimento);
        await Context.SaveChangesAsync();
        return atendimento;
    }
}