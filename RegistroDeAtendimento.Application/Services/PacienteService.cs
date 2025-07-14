using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Application.Interfaces;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace RegistroDeAtendimento.Application.Services;

public class PacienteService(IPacienteRepository repository, IValidator<CriarPacienteDto> criarValidator,
    IValidator<AtualizarPacienteDto> atualizarValidator) : IPacienteService{
    
    public async Task<PagedResponse<List<Paciente>>> ListarPacientesAsync(ListarPacientesDto dto){
        var query = repository.ObterTodosPacientes();

        if (!string.IsNullOrWhiteSpace(dto.Nome))
            query = query.Where(p => p.Nome.Contains(dto.Nome));

        if (!string.IsNullOrWhiteSpace(dto.Cpf))
            query = query.Where(p => p.Cpf == dto.Cpf);

        if (dto.Status.HasValue)
            query = query.Where(p => p.Status == dto.Status.Value);
        
        query = dto.OrderBy switch{
            OrderByPacienteEnum.Nome => dto.Sort == SortDirectionEnum.Asc
                ? query.OrderBy(p => p.Nome)
                : query.OrderByDescending(p => p.Nome),
            OrderByPacienteEnum.DataNascimento => dto.Sort == SortDirectionEnum.Asc
                ? query.OrderBy(p => p.DataNascimento)
                : query.OrderByDescending(p => p.DataNascimento),
            OrderByPacienteEnum.Cpf => dto.Sort == SortDirectionEnum.Asc
                ? query.OrderBy(p => p.Cpf)
                : query.OrderByDescending(p => p.Cpf),
            _ => dto.Sort == SortDirectionEnum.Asc
                ? query.OrderBy(p => p.Id)
                : query.OrderByDescending(p => p.Id)
        };

        var totalItems = await query.CountAsync();

        var list = await query
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .ToListAsync();

        return new PagedResponse<List<Paciente>>(list, totalItems, dto.Page, dto.PageSize);
    }

    public async Task<Response<Paciente?>> ObterPacientePorIdAsync(Guid id){
        var paciente = await repository.ObterPacientePorIdAsync(id);

        return paciente == null
            ? new Response<Paciente?>(null, 404, "Paciente não encontrado.")
            : new Response<Paciente?>(paciente, 200, "Paciente localizado com sucesso.");
    }

    public async Task<Response<Paciente?>> CriarPacienteAsync(CriarPacienteDto dto){
        var validation = await criarValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return new Response<Paciente?>(null, 400, string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        if (await repository.ExisteCpfAsync(dto.Cpf))
            return new Response<Paciente?>(null, 409, "Já existe um paciente com esse CPF.");

        var paciente = new Paciente(dto.Nome, dto.DataNascimento, dto.Cpf, dto.Sexo, new Endereco(dto.Cep, dto.Cidade,
            dto.Bairro, dto.Logradouro,
            dto.Complemento), dto.Status);

        await repository.AdicionarPacienteAsync(paciente);

        return new Response<Paciente?>(paciente, 201, "Paciente criado com sucesso.");
    }

    public async Task<Response<Paciente?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto){
        var paciente = await repository.ObterPacientePorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null, 404, "Paciente não encontrado.");

        if (dto.Cpf != null && await repository.ExisteCpfAsync(dto.Cpf, id))
            return new Response<Paciente?>(null, 409, "Já existe um paciente com esse CPF.");

        var validation = await atualizarValidator.ValidateAsync(dto);

        if (!validation.IsValid)
            return new Response<Paciente?>(null, 400, string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        paciente.AtualizarDados(
            !string.IsNullOrWhiteSpace(dto.Nome) ? dto.Nome : paciente.Nome,
            dto.DataNascimento ?? paciente.DataNascimento,
            !string.IsNullOrWhiteSpace(dto.Cpf) ? dto.Cpf : paciente.Cpf,
            dto.Sexo ?? paciente.Sexo,
            new Endereco(
                !string.IsNullOrWhiteSpace(dto.Cep) ? dto.Cep : paciente.Endereco.Cep,
                !string.IsNullOrWhiteSpace(dto.Cidade) ? dto.Cidade : paciente.Endereco.Cidade,
                !string.IsNullOrWhiteSpace(dto.Bairro) ? dto.Bairro : paciente.Endereco.Bairro,
                !string.IsNullOrWhiteSpace(dto.Logradouro) ? dto.Logradouro : paciente.Endereco.Logradouro,
                !string.IsNullOrWhiteSpace(dto.Complemento) ? dto.Complemento : paciente.Endereco.Complemento
            )
        );
        
        await repository.AtualizarPacienteAsync(paciente);

        return new Response<Paciente?>(null, 204, "Paciente atualizado com sucesso.");
    }

    public async Task<Response<Paciente?>> InativarPacienteAsync(Guid id){
        var paciente = await repository.ObterPacientePorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null, 404, "Paciente não encontrado.");

        if (paciente.Status != StatusEnum.Ativo)
            return new Response<Paciente?>(null, 200, "Paciente já está Inativo.");
        
        paciente.AlterarStatus(StatusEnum.Inativo);
        await repository.AtualizarPacienteAsync(paciente);
        return new Response<Paciente?>(null, 204, "Paciente inativado com sucesso.");
    }

    public async Task<Response<Paciente?>> AtivarPacienteAsync(Guid id){
        var paciente = await repository.ObterPacientePorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null, 404, "Paciente não encontrado.");

        if (paciente.Status != StatusEnum.Inativo)
            return new Response<Paciente?>(null, 200, "Paciente já está Ativo.");

        paciente.AlterarStatus(StatusEnum.Ativo);
        await repository.AtualizarPacienteAsync(paciente);
        return new Response<Paciente?>(null, 204, "Paciente ativado com sucesso.");
    }
}