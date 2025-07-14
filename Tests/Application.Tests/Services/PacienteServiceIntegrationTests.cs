using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Application.Services;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Infrastructure.Repositories;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Validators;

namespace RegistroDeAtendimento.Tests.Services;

public class PacienteServiceIntegrationTests : TestBase{
    private readonly PacienteService _service;

    public PacienteServiceIntegrationTests(){
        var repository = new PacienteRepository(Context);
        IValidator<CriarPacienteDto> criarValidator = new CriarPacienteDtoValidator();
        IValidator<AtualizarPacienteDto> atualizarValidator = new AtualizarPacienteDtoValidator();
        _service = new PacienteService(repository, criarValidator, atualizarValidator);
    }

    [Fact]
    public async Task CriarAsync_Deve_Criar_Paciente_Com_Dados_Validos(){
        var dto = new CriarPacienteDto{
            Nome = "Maria Silva",
            DataNascimento = new DateOnly(1985, 5, 15),
            Cpf = "98765432100",
            Sexo = SexoEnum.Feminino,
            Cep = "87654321",
            Cidade = "São Paulo",
            Bairro = "Vila Madalena",
            Logradouro = "Rua Augusta, 456",
            Complemento = "Sala 2",
            Status = StatusEnum.Ativo
        };

        var resultado = await _service.CriarPacienteAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(201);
        resultado.Message.Should().Be("Paciente criado com sucesso.");
        resultado.Data.Should().NotBeNull();
        resultado.Data!.Nome.Should().Be("Maria Silva");
        resultado.Data.Cpf.Should().Be("98765432100");
        resultado.Data.Sexo.Should().Be(SexoEnum.Feminino);
        resultado.Data.Cidade.Should().Be("São Paulo");
    }

    [Fact]
    public async Task CriarAsync_Deve_Retornar_Erro_Quando_CPF_Ja_Existe(){
        var cpf = "12345678900";
        await CriarPacienteAsync("João", cpf);

        var dto = new CriarPacienteDto{
            Nome = "Maria Silva",
            DataNascimento = new DateOnly(1985, 5, 15),
            Cpf = cpf,
            Sexo = SexoEnum.Feminino,
            Cep = "87654321",
            Cidade = "São Paulo",
            Bairro = "Vila Madalena",
            Logradouro = "Rua Augusta, 456",
            Complemento = "Sala 2",
            Status = StatusEnum.Ativo
        };

        var resultado = await _service.CriarPacienteAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(409);
        resultado.Message.Should().Be("Já existe um paciente com esse CPF.");
        resultado.Data.Should().BeNull();
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Paciente_Quando_Existe(){
        var paciente = await CriarPacienteAsync();

        var resultado = await _service.ObterPacientePorIdAsync(paciente.Id);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(200);
        resultado.Message.Should().Be("Paciente localizado com sucesso.");
        resultado.Data.Should().NotBeNull();
        resultado.Data!.Id.Should().Be(paciente.Id);
        resultado.Data.Nome.Should().Be(paciente.Nome);
    }

    [Fact]
    public async Task ObterPorIdAsync_Deve_Retornar_Erro_Quando_Paciente_Nao_Existe(){
        var idInexistente = Guid.NewGuid();

        var resultado = await _service.ObterPacientePorIdAsync(idInexistente);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(404);
        resultado.Message.Should().Be("Paciente não encontrado.");
        resultado.Data.Should().BeNull();
    }

    [Fact]
    public async Task ListarTodosAsync_Deve_Retornar_Pacientes_Paginados(){
        await CriarPacienteAsync("João", "11111111111");
        await CriarPacienteAsync("Maria", "22222222222");
        await CriarPacienteAsync("Pedro", "33333333333");

        var dto = new ListarPacientesDto(){
            Page = 1,
            PageSize = 2,
            OrderBy = OrderByPacienteEnum.Nome,
            Sort = SortDirectionEnum.Asc
        };
        
        var resultado = await _service.ListarPacientesAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.TotalCount.Should().Be(3);
        resultado.CurrentPage.Should().Be(1);
        resultado.PageSize.Should().Be(2);
    }

    [Fact]
    public async Task ListarAtivosAsync_Deve_Retornar_Apenas_Pacientes_Ativos(){
        var paciente1 = await CriarPacienteAsync("João", "11111111111");
        var paciente2 = await CriarPacienteAsync("Maria", "22222222222");
        var paciente3 = await CriarPacienteAsync("Pedro", "33333333333");

        paciente2.AlterarStatus(StatusEnum.Inativo);
        await Context.SaveChangesAsync();

        var dto = new ListarPacientesDto(){
            Status = StatusEnum.Ativo,
            Page = 1,
            PageSize = 2,
            OrderBy = OrderByPacienteEnum.Nome,
            Sort = SortDirectionEnum.Asc
        };
        
        var resultado = await _service.ListarPacientesAsync(dto);
        
        resultado.Should().NotBeNull();
        resultado.Data.Should().HaveCount(2);
        resultado.Data.Should().Contain(p => p.Nome == "João");
        resultado.Data.Should().Contain(p => p.Nome == "Pedro");
        resultado.Data.Should().NotContain(p => p.Nome == "Maria");
    }

    [Fact]
    public async Task AtualizarAsync_Deve_Atualizar_Paciente_Com_Dados_Validos(){
        var paciente = await CriarPacienteAsync();
        var dto = new AtualizarPacienteDto{
            Nome = "João Atualizado",
            DataNascimento = new DateOnly(1985, 5, 15),
            Cpf = "98765432100",
            Sexo = SexoEnum.Masculino,
            Cep = "87654321",
            Cidade = "São Paulo",
            Bairro = "Vila Madalena",
            Logradouro = "Rua Augusta, 456",
            Complemento = "Sala 2",
            Status = StatusEnum.Inativo
        };
        var resultado = await _service.AtualizarPacienteAsync(paciente.Id, dto);
        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(204);
        resultado.Message.Should().Be("Paciente atualizado com sucesso.");
        var pacienteAtualizado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteAtualizado.Should().NotBeNull();
        pacienteAtualizado!.Nome.Should().Be("João Atualizado");
        pacienteAtualizado.Cpf.Should().Be("98765432100");
        pacienteAtualizado.Endereco.Cidade.Should().Be("São Paulo");
    }

    [Fact]
    public async Task AtualizarAsync_Deve_Retornar_Erro_Quando_Paciente_Nao_Existe(){
        var idInexistente = Guid.NewGuid();
        var dto = new AtualizarPacienteDto{
            Nome = "João Atualizado",
            DataNascimento = new DateOnly(1985, 5, 15),
            Cpf = "98765432100",
            Sexo = SexoEnum.Masculino,
            Cep = "87654321",
            Cidade = "São Paulo",
            Bairro = "Vila Madalena",
            Logradouro = "Rua Augusta, 456",
            Complemento = "Sala 2",
            Status = StatusEnum.Ativo
        };

        var resultado = await _service.AtualizarPacienteAsync(idInexistente, dto);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(404);
        resultado.Message.Should().Be("Paciente não encontrado.");
    }

    [Fact]
    public async Task InativarAsync_Deve_Inativar_Paciente_Ativo(){
        var paciente = await CriarPacienteAsync();

        var resultado = await _service.InativarPacienteAsync(paciente.Id);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(204);
        resultado.Message.Should().Be("Paciente inativado com sucesso.");

        var pacienteInativado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteInativado.Should().NotBeNull();
        pacienteInativado!.Status.Should().Be(StatusEnum.Inativo);
    }

    [Fact]
    public async Task AtivarAsync_Deve_Ativar_Paciente_Inativo(){
        var paciente = await CriarPacienteAsync();
        paciente.AlterarStatus(StatusEnum.Inativo);
        await Context.SaveChangesAsync();

        var resultado = await _service.AtivarPacienteAsync(paciente.Id);

        resultado.Should().NotBeNull();
        resultado.Code.Should().Be(204);
        resultado.Message.Should().Be("Paciente ativado com sucesso.");

        var pacienteAtivado = await Context.Pacientes.FirstOrDefaultAsync(p => p.Id == paciente.Id);
        pacienteAtivado.Should().NotBeNull();
        pacienteAtivado!.Status.Should().Be(StatusEnum.Ativo);
    }

    [Fact]
    public async Task ListarTodosAsync_Deve_Ordenar_Por_Nome_Descendente(){
        await CriarPacienteAsync("Ana", "11111111111");
        await CriarPacienteAsync("Carlos", "22222222222");
        await CriarPacienteAsync("Bruno", "33333333333");

        var dto = new ListarPacientesDto(){
            Page = 1,
            PageSize = 3,
            OrderBy = OrderByPacienteEnum.Nome,
            Sort = SortDirectionEnum.Desc
        };
        
        var resultado = await _service.ListarPacientesAsync(dto);

        resultado.Should().NotBeNull();
        resultado.Data.Should().HaveCount(3);
        resultado.Data?[0].Nome.Should().Be("Carlos");
        resultado.Data?[1].Nome.Should().Be("Bruno");
        resultado.Data?[2].Nome.Should().Be("Ana");
    }
}