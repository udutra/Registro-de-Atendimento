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

public class PacienteService(IPacienteRepository repository, IValidator<CriarPacienteDto> criarValidator, IValidator<AtualizarPacienteDto> atualizarValidator) 
    : IPacienteService{

    public async Task<PagedResponse<List<Paciente>>> ListarTodosAsync(int page, int itemsPerPage, OrderByPacienteEnum orderBy, SortDirectionEnum sort){
        var query = repository.ObterTodos();

        query = orderBy switch{
            OrderByPacienteEnum.Nome => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Nome)  : query.OrderByDescending(p => p.Nome),
            OrderByPacienteEnum.DataNascimento => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.DataNascimento)  : query.OrderByDescending(p => p.DataNascimento),
            OrderByPacienteEnum.Cpf => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Cpf)  : query.OrderByDescending(p => p.Cpf),
            _ => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id)
        };
        
        var totalItems = await query.CountAsync();
        
        var list = await query
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync();
        
        return new PagedResponse<List<Paciente>>(list, totalItems, page, itemsPerPage);
    }

    public async Task<PagedResponse<List<Paciente>>> ListarAtivosAsync(int page, int itemsPerPage, OrderByPacienteEnum orderBy, SortDirectionEnum sort){
        var query = repository.ObterTodos().Where(p => p.Status == StatusEnum.Ativo);
        
        query = orderBy switch{
            OrderByPacienteEnum.Nome => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Nome)  : query.OrderByDescending(p => p.Nome),
            OrderByPacienteEnum.DataNascimento => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.DataNascimento)  : query.OrderByDescending(p => p.DataNascimento),
            OrderByPacienteEnum.Cpf => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Cpf)  : query.OrderByDescending(p => p.Cpf),
            _ => sort == SortDirectionEnum.Asc ? query.OrderBy(p => p.Id) : query.OrderByDescending(p => p.Id)
        };
        
        var totalItems = await query.CountAsync();
        
        var list = await query
            .Skip((page - 1) * itemsPerPage)
            .Take(itemsPerPage)
            .ToListAsync();
        
        return new PagedResponse<List<Paciente>>(list, totalItems, page, 200);
    }

    public async Task<Response<Paciente?>> ObterPorIdAsync(Guid id){
        var paciente = await repository.ObterPorIdAsync(id);

        return paciente == null ? new Response<Paciente?>(null,404, "Paciente não encontrado.") : 
            new Response<Paciente?>(paciente,200, "Paciente localizado com sucesso.");
    }

    public async Task<Response<Paciente?>> CriarAsync(CriarPacienteDto dto){
        var validation = await criarValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return new Response<Paciente?>(null,400, validation.Errors.Select(e => e.ErrorMessage).ToString());

        if (await repository.ExisteCpfAsync(dto.Cpf))
            return new Response<Paciente?>(null,409, "Já existe um paciente com esse CPF.");

        var paciente = new Paciente(dto.Nome, dto.DataNascimento, dto.Cpf, dto.Sexo, new Endereco(dto.Cep, dto.Cidade, dto.Bairro, dto.Logradouro, 
                dto.Complemento), dto.Status);

        await repository.AdicionarAsync(paciente);

        return new Response<Paciente?>(paciente,201, "Paciente criado com sucesso.");
    }

    public async Task<Response<Paciente?>> AtualizarAsync(Guid id, AtualizarPacienteDto dto){
        var paciente = await repository.ObterPorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null,404, "Paciente não encontrado.");
        
        if (await repository.ExisteCpfAsync(dto.Cpf, paciente.Id))
            return new Response<Paciente?>(null,409, "Já existe um paciente com esse CPF.");

        var validation = await atualizarValidator.ValidateAsync(dto);

        if (!validation.IsValid)
            return new Response<Paciente?>(null,400, validation.Errors.Select(e => e.ErrorMessage).ToString());

        paciente.AtualizarDados(nome: dto.Nome, cpf: dto.Cpf, dataNascimento: dto.DataNascimento, sexo: dto.Sexo, endereco: new Endereco(cep: dto.Cep,
                cidade: dto.Cidade, bairro: dto.Bairro, logradouro: dto.Logradouro, complemento: dto.Complemento), status: dto.Status);

        await repository.AtualizarAsync(paciente);

        return new Response<Paciente?>(null,204, "Paciente atualizado com sucesso.");
    }

    public async Task<Response<Paciente?>> InativarAsync(Guid id){
        var paciente = await repository.ObterPorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null,404, "Paciente não encontrado.");
        
        if (paciente.Status != StatusEnum.Ativo) 
            return new Response<Paciente?>(null, 200, "Paciente ja está Inativo.");
        
        paciente.Status = StatusEnum.Inativo;
        await repository.InativarAsync(paciente);
        return new Response<Paciente?>(null,204, "Paciente inativado com sucesso.");
    }
    
    public async Task<Response<Paciente?>> AtivarAsync(Guid id){
        var paciente = await repository.ObterPorIdAsync(id);
        if (paciente is null)
            return new Response<Paciente?>(null,404, "Paciente não encontrado.");

        if (paciente.Status != StatusEnum.Inativo) 
            return new Response<Paciente?>(null, 200, "Paciente ja está Ativo.");
        
        paciente.Status = StatusEnum.Ativo;
        await repository.AtivarAsync(paciente);
        return new Response<Paciente?>(null,204, "Paciente ativado com sucesso.");
    }
}