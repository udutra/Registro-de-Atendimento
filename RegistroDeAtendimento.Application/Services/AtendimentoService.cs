using FluentValidation;
using Microsoft.EntityFrameworkCore;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Application.Interfaces;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Exceptions;
using RegistroDeAtendimento.Domain.Interfaces;

namespace RegistroDeAtendimento.Application.Services;

public class AtendimentoService(IAtendimentoRepository repository, IPacienteRepository pacienteRepository, 
    IValidator<CriarAtendimentoDto> criarValidator, IValidator<AtualizarAtendimentoDto> atualizarValidator) : IAtendimentoService {
    public async Task<PagedResponse<List<AtendimentoResponseDto>>> ListarAtendimentosAsync(ListarAtendimentosDto dto){
        var query = repository.ObterTodosAtendimentos();

        if (dto.DataInicio.HasValue)
            query = query.Where(a => a.DataHora >= dto.DataInicio.Value);

        if (dto.DataFim.HasValue)
            query = query.Where(a => a.DataHora <= dto.DataFim.Value);

        if (dto.PacienteId.HasValue)
            query = query.Where(a => a.PacienteId == dto.PacienteId.Value);
        
        if (!string.IsNullOrWhiteSpace(dto.PacienteNome))
            query = query.Where(p => p.Paciente.Nome.Contains(dto.PacienteNome));
        
        if (!string.IsNullOrWhiteSpace(dto.PacienteCpf))
            query = query.Where(p => p.Paciente.Cpf.Contains(dto.PacienteCpf));

        if (dto.Status.HasValue)
            query = query.Where(a => a.Status == dto.Status.Value);

        var totalItems = await query.CountAsync();

        var list = await query
            .OrderByDescending(a => a.DataHora)
            .Skip((dto.Page - 1) * dto.PageSize)
            .Take(dto.PageSize)
            .Select(a => new AtendimentoResponseDto{
                Id = a.Id,
                PacienteId = a.PacienteId,
                PacienteNome = a.Paciente.Nome,
                PacienteCpf = a.Paciente.Cpf,
                DataHora = a.DataHora,
                Descricao = a.Descricao,
                Status = a.Status
            })
            .ToListAsync();

        return new PagedResponse<List<AtendimentoResponseDto>>(list, totalItems, dto.Page, dto.PageSize);
    }
    
    public async Task<Response<AtendimentoResponseDto?>> ObterAtendimentoPorIdAsync(Guid id){
        var atendimento = await repository.ObterAtendimentoPorIdAsync(id);

        if (atendimento != null){
            var atendimentoResponse = new AtendimentoResponseDto(){
                DataHora = atendimento.DataHora,
                Descricao = atendimento.Descricao,
                Id = atendimento.Id,
                PacienteId = atendimento.PacienteId,
                PacienteNome = atendimento.Paciente.Nome,
                PacienteCpf = atendimento.Paciente.Cpf,
                Status = atendimento.Status
            };
            
            return new Response<AtendimentoResponseDto?>(atendimentoResponse, 200, "Atendimento localizado com sucesso.");
            
        }

        return new Response<AtendimentoResponseDto?>(null, 404, "Atendimento não encontrado.");

    }
    
    public async Task<Response<AtendimentoResponseDto?>> CriarAtendimentoAsync(CriarAtendimentoDto dto){
        var validation = await criarValidator.ValidateAsync(dto);
        
        if (!validation.IsValid)
            return new Response<AtendimentoResponseDto?>(null, 400, string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));
        
        var paciente = await pacienteRepository.ObterPacientePorIdAsync(dto.PacienteId);
        
        if (paciente is null)
            return new Response<AtendimentoResponseDto?>(null, 404, "Paciente não encontrado.");
        
        var atendimento = new Atendimento(paciente, dto.DataHora, dto.Descricao, dto.Status);

        await repository.AdicionarAtendimentoAsync(atendimento);

        var atendimentoResponse = new AtendimentoResponseDto(){
            DataHora = atendimento.DataHora,
            Descricao = atendimento.Descricao,
            Id = atendimento.Id,
            PacienteId = atendimento.PacienteId,
            PacienteNome = atendimento.Paciente.Nome,
            PacienteCpf = atendimento.Paciente.Cpf,
            Status = atendimento.Status
        };
        
        return new Response<AtendimentoResponseDto?>(atendimentoResponse, 201, "Atendimento criado com sucesso.");
    }
    
    public async Task<Response<AtendimentoResponseDto?>> AtualizarAtendimentoAsync(Guid id, AtualizarAtendimentoDto dto){
        var atendimento = await repository.ObterAtendimentoPorIdAsync(id);

        if (atendimento is null)
            return new Response<AtendimentoResponseDto?>(null, 404, "Atendimento não encontrado.");

        var paciente = atendimento.Paciente;

        if (dto.PacienteId.HasValue && dto.PacienteId.Value != atendimento.PacienteId){
            var pacienteEncontrado = await pacienteRepository.ObterPacientePorIdAsync(dto.PacienteId.Value);
            if (pacienteEncontrado is null)
                return new Response<AtendimentoResponseDto?>(null, 404, "Novo paciente não encontrado.");

            paciente = pacienteEncontrado;
        }
        
        var validation = await atualizarValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            return new Response<AtendimentoResponseDto?>(null, 400, string.Join("; ", validation.Errors.Select(e => e.ErrorMessage)));

        try{
            atendimento.AtualizarDados(paciente, dto.DataHora ?? atendimento.DataHora, dto.Descricao ?? atendimento.Descricao);
        }
        catch (DomainException ex){
            return new Response<AtendimentoResponseDto?>(null, 400, ex.Message);
        }

        await repository.AtualizarAtendimentoAsync(atendimento);

        return new Response<AtendimentoResponseDto?>(null, 204, "Atendimento atualizado com sucesso.");
    }
    
    
    public async Task<Response<AtendimentoResponseDto?>> InativarAtendimentoAsync(Guid id){
        var atendimento = await repository.ObterAtendimentoPorIdAsync(id);
        if (atendimento is null)
            return new Response<AtendimentoResponseDto?>(null, 404, "Atendimento não encontrado.");

        if (atendimento.Status == StatusEnum.Inativo)
            return new Response<AtendimentoResponseDto?>(null, 200, "Atendimento ja está Inativo.");

        atendimento.AlterarStatus(StatusEnum.Inativo);
        
        await repository.AtualizarAtendimentoAsync(atendimento);
        return new Response<AtendimentoResponseDto?>(null, 204, "Atendimento inativado com sucesso.");
    }
    
    public async Task<Response<AtendimentoResponseDto?>> AtivarAtendimentoAsync(Guid id){
        var atendimento = await repository.ObterAtendimentoPorIdAsync(id);
        if (atendimento is null)
            return new Response<AtendimentoResponseDto?>(null, 404, "Atendimento não encontrado.");

        if (atendimento.Status == StatusEnum.Ativo)
            return new Response<AtendimentoResponseDto?>(null, 200, "Atendimento ja está Ativado.");

        atendimento.AlterarStatus(StatusEnum.Ativo);
        
        await repository.AtualizarAtendimentoAsync(atendimento);
        return new Response<AtendimentoResponseDto?>(null, 204, "Atendimento ativado com sucesso.");
    }
}