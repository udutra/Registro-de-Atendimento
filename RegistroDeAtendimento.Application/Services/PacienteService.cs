using FluentValidation;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Interfaces;
using RegistroDeAtendimento.Domain.Entities;
using RegistroDeAtendimento.Domain.Enums;
using RegistroDeAtendimento.Domain.Interfaces;
using RegistroDeAtendimento.Domain.ValueObjects;

namespace RegistroDeAtendimento.Application.Services;

public class PacienteService(IPacienteRepository repository, IValidator<CriarPacienteDto> criarValidator, 
    IValidator<AtualizarPacienteDto> atualizarValidator) : IPacienteService{
    
    public async Task<IEnumerable<Paciente>> ListarTodosAsync() => await repository.ObterTodosAsync();

    public async Task<IEnumerable<Paciente>> ListarAtivosAsync() => await repository.ObterAtivosAsync();
    
    public async Task<Paciente?> ObterPorIdAsync(Guid id) => await repository.ObterPorIdAsync(id);

    public async Task<bool> CriarAsync(CriarPacienteDto dto){
        var validation = await criarValidator.ValidateAsync(dto);
        if (!validation.IsValid)
            //return Result.Falha(validation.Errors.Select(e => e.ErrorMessage));
            return false;

        if (await repository.ExisteCpfAsync(dto.Cpf))
            //return Result.Falha("Já existe um paciente com esse CPF.");
            return false;

        var paciente = new Paciente(
            dto.Nome,
            dto.DataNascimento,
            dto.Cpf,
            dto.Sexo,
            new Endereco(dto.Cep, dto.Cidade, dto.Bairro, dto.Logradouro, dto.Complemento),
            dto.Status
        );

        await repository.AdicionarAsync(paciente);

        return true;
    }

    public async Task<bool> AtualizarAsync(Guid id, AtualizarPacienteDto dto){
        var paciente = await repository.ObterPorIdAsync(id);
        if (paciente is null)
            //return Result.Falha("Paciente não encontrado.");
            return false;
        
        if (await repository.ExisteCpfAsync(dto.Cpf, paciente.Id))
            //return Result.Falha("Já existe um paciente com esse CPF.");
            return false;

        var validation = await atualizarValidator.ValidateAsync(dto);

        if (!validation.IsValid)
            //return Result.Falha(validation.Errors.Select(e => e.ErrorMessage));
            return false;

        paciente.AtualizarDados(
            nome: dto.Nome,
            cpf: dto.Cpf,
            dataNascimento: dto.DataNascimento,
            sexo: dto.Sexo,
            endereco: new Endereco(
                cep: dto.Cep,
                cidade: dto.Cidade,
                bairro: dto.Bairro,
                logradouro: dto.Logradouro,
                complemento: dto.Complemento
            ),
            status: dto.Status
        );

        await repository.AtualizarAsync(paciente);

        //return Result.Sucesso();
        return true;
    }

    public async Task<bool> InativarAsync(Guid id){
        var paciente = await repository.ObterPorIdAsync(id);
        if (paciente is null)
            //return Result.Falha("Paciente não encontrado.");
            return false;
        paciente.Status = StatusEnum.Inativo;
        await repository.AtualizarAsync(paciente);
        //return Result.Sucesso();
        return true;
    }
}