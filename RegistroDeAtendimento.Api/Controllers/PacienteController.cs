using Microsoft.AspNetCore.Mvc;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Interfaces;

namespace RegistroDeAtendimento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacienteController(IPacienteService pacienteService) : ControllerBase{
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery]ListarPacientesDto dto){
        var pacientes = await pacienteService.ListarPacientesAsync(dto);
        return Ok(pacientes);
    }

    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id){
        var paciente = await pacienteService.ObterPacientePorIdAsync(id);
        if (paciente.Code == 200)
            return Ok(paciente);

        return NotFound(paciente);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CriarPacienteDto dto){
        var paciente = await pacienteService.CriarPacienteAsync(dto);

        if (paciente.Data == null)
            return paciente.Code switch{
                409 => Conflict(paciente),
                _ => BadRequest(paciente)
            };

        return CreatedAtAction(nameof(GetById), new{ id = paciente.Data.Id }, paciente.Data);
    }

    [HttpPut("atualizar/{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AtualizarPacienteDto dto){
        var paciente = await pacienteService.AtualizarPacienteAsync(id, dto);
        
        if (!paciente.IsSuccess)
            return paciente.Code switch{
                404 => NotFound(paciente),
                409 => Conflict(paciente),
                _ => BadRequest(paciente)
            };

        return NoContent();
    }

    [HttpPatch("inativar/{id:guid}")]
    public async Task<IActionResult> Inativar(Guid id){
        var paciente = await pacienteService.InativarPacienteAsync(id);

        return paciente.Code switch{
            200 => Ok(paciente.Message),
            204 => NoContent(),
            _ => BadRequest(paciente)
        };
    }

    [HttpPatch("ativar/{id:guid}")]
    public async Task<IActionResult> Ativar(Guid id){
        var paciente = await pacienteService.AtivarPacienteAsync(id);

        return paciente.Code switch{
            200 => Ok(paciente.Message),
            204 => NoContent(),
            _ => BadRequest(paciente)
        };
    }
}