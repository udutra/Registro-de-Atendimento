using Microsoft.AspNetCore.Mvc;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Dtos.Responses;
using RegistroDeAtendimento.Application.Interfaces;
using RegistroDeAtendimento.Domain.Enums;

namespace RegistroDeAtendimento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacienteController(IPacienteService pacienteService) : ControllerBase{
    
    [HttpGet]
    public async Task<IActionResult> GetAll(int page = ConfigurationResponse.DefaultCurrentPage, int itemsPerPage = ConfigurationResponse.DefaultPageSize, 
        OrderByPacienteEnum orderBy = ConfigurationResponse.DefaultOrderBy, SortDirectionEnum sort = ConfigurationResponse.DefaultDirection){
        var pacientes = await pacienteService.ListarTodosAsync(page, itemsPerPage, orderBy, sort);
        return Ok(pacientes);
    }
    
    [HttpGet("ativos")]
    public async Task<IActionResult> GetAtivos(int page = ConfigurationResponse.DefaultCurrentPage, int itemsPerPage = ConfigurationResponse.DefaultPageSize, 
        OrderByPacienteEnum orderBy = ConfigurationResponse.DefaultOrderBy, SortDirectionEnum sort = ConfigurationResponse.DefaultDirection){
        var pacientes = await pacienteService.ListarAtivosAsync(page, itemsPerPage, orderBy, sort);
        return Ok(pacientes);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id){
        var paciente = await pacienteService.ObterPorIdAsync(id);
        if (paciente.Code == 200)
            return Ok(paciente);
            
        return NotFound(paciente);
    }
    
    [HttpPost] 
    public async Task<IActionResult> Post([FromBody] CriarPacienteDto dto){
        var paciente = await pacienteService.CriarAsync(dto);

        if (paciente.Data == null)
            return paciente.Code switch{
                409 => Conflict(paciente),
                _ => BadRequest(paciente)
            };

        return CreatedAtAction(nameof(GetById), new { id = paciente.Data.Id }, paciente.Data);
    }
    
    [HttpPut("atualizar/{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AtualizarPacienteDto dto){
        var paciente = await pacienteService.AtualizarAsync(id, dto);

        if (paciente.Data == null)
            return paciente.Code switch{
                404 => NotFound(paciente),
                409 => Conflict(paciente),
                _ => BadRequest(paciente)
            };

        return NoContent();
    }
    
    [HttpPatch("inativar/{id:guid}")]
    public async Task<IActionResult> Inativar(Guid id){
        var paciente = await pacienteService.InativarAsync(id);

        if (paciente.Data == null)
            return paciente.Code switch{
                200 => Ok(paciente.Message),
                _ => BadRequest(paciente)
            };

        return NoContent();
    }
    
    [HttpPatch("ativar/{id:guid}")]
    public async Task<IActionResult> Ativar(Guid id){
        var paciente = await pacienteService.AtivarAsync(id);

        if (paciente.Data == null)
            return paciente.Code switch{
                200 => Ok(paciente.Message),
                _ => BadRequest(paciente)
            };

        return NoContent();
    }
}