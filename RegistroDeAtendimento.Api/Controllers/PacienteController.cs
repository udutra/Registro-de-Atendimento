using Microsoft.AspNetCore.Mvc;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Interfaces;

namespace RegistroDeAtendimento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PacienteController(IPacienteService pacienteService) : ControllerBase{
    
    [HttpGet]
    public async Task<IActionResult> GetAll(){
        var pacientes = await pacienteService.ListarTodosAsync();
        return Ok(pacientes);
    }
    
    [HttpGet("ativos")]
    public async Task<IActionResult> GetAtivos(){
        var pacientes = await pacienteService.ListarAtivosAsync();
        return Ok(pacientes);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id){
        var paciente = await pacienteService.ObterPorIdAsync(id);
        if (paciente == null)
            return NotFound("Paciente não encontrado.");

        return Ok(paciente);
    }
    
    [HttpPost] public async Task<IActionResult> Post([FromBody] CriarPacienteDto dto)
    {
        var resultado = await pacienteService.CriarAsync(dto);

        if (!resultado)
            //return BadRequest(new { erros = resultado.Erros });
            return BadRequest();

        return CreatedAtAction(nameof(GetById), new { id = dto.Cpf }, dto); // ou retorne o ID criado
    }
    
    [HttpPut("{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AtualizarPacienteDto dto){
        var resultado = await pacienteService.AtualizarAsync(id, dto);

        if (!resultado)
            //return BadRequest(new{ erros = resultado.Erros });
            return BadRequest();

        return NoContent();
    }
    
    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        var resultado = await pacienteService.InativarAsync(id);

        if (!resultado)
            //return NotFound(new { erros = resultado.Erros });
            return NotFound();

        return NoContent();
    }
}