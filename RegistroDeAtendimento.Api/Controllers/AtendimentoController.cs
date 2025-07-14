using Microsoft.AspNetCore.Mvc;
using RegistroDeAtendimento.Application.Dtos;
using RegistroDeAtendimento.Application.Interfaces;
namespace RegistroDeAtendimento.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AtendimentoController(IAtendimentoService atendimentoService) : ControllerBase{
    
    [HttpGet]
    public async Task<IActionResult> GetAll([FromQuery] ListarAtendimentosDto dto){
        var atendimentos = await atendimentoService.ListarAtendimentosAsync(dto);
        return Ok(atendimentos);
    }
    
    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetById(Guid id){
        var atendimento = await atendimentoService.ObterAtendimentoPorIdAsync(id);
        if (atendimento.Code == 200)
            return Ok(atendimento);

        return NotFound(atendimento);
    }
    
    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CriarAtendimentoDto dto){
        var atendimento = await atendimentoService.CriarAtendimentoAsync(dto);

        if (atendimento.Data == null)
            return atendimento.Code switch{
                404 => NotFound(atendimento),
                _ => BadRequest(atendimento)
            };

        return CreatedAtAction(nameof(GetById), new{ id = atendimento.Data.Id }, atendimento.Data);
    }
    
    [HttpPut("atualizar/{id:guid}")]
    public async Task<IActionResult> Put(Guid id, [FromBody] AtualizarAtendimentoDto dto){
        var atendimento = await atendimentoService.AtualizarAtendimentoAsync(id, dto);

        if (!atendimento.IsSuccess)
            return atendimento.Code switch{
                404 => NotFound(atendimento),
                _ => BadRequest(atendimento)
            };

        return NoContent();
    }
    
    [HttpPatch("inativar/{id:guid}")]
    public async Task<IActionResult> Inativar(Guid id){
        var atendimento = await atendimentoService.InativarAtendimentoAsync(id);
        
        return atendimento.Code switch{
            200 => Ok(atendimento.Message),
            204 => NoContent(),
            _ => BadRequest(atendimento)
        };

    }

    [HttpPatch("ativar/{id:guid}")]
    public async Task<IActionResult> Ativar(Guid id){
        var atendimento = await atendimentoService.AtivarAtendimentoAsync(id);

        return atendimento.Code switch{
            200 => Ok(atendimento.Message),
            204 => NoContent(),
            _ => BadRequest(atendimento)
        };
    }
}