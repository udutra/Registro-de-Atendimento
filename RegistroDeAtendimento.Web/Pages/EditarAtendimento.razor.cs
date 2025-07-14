using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;
using RegistroDeAtendimento.Shared.Application.Validators;

namespace RegistroDeAtendimento.Web.Pages;

public partial class EditarAtendimento : ComponentBase{
    [Parameter] public Guid Id{ get; set; }
    private AtualizarAtendimentoDto AtualizarAtendimentoDto{ get; set; } = new();
    [Inject] private NavigationManager NavigationManager{ get; set; } = null!;
    [Inject] private ISnackbar Snackbar{ get; set; }
    private MudForm _form;
    [Inject] AtualizarAtendimentoDtoValidator _atualizarAtendimentoDtoValidator{ get; set; }
    [Inject] private IAtendimentoService AtendimentoService{ get; set; } = null!;
    private DateTime? _date{ get; set; }
    private TimeSpan? _time;
    
    protected override async Task<Task> OnInitializedAsync(){
        var response = await AtendimentoService.ObterAtendimentoPorIdAsync(Id);
        AtualizaAtendimento(response);
        return base.OnInitializedAsync();
    }
    
    private void AtualizaAtendimento(Response<AtendimentoResponseDto?> response){
        if (response.Data == null) return;
        
        _date ??= response.Data.DataHora.Date;
        _time ??= response.Data.DataHora.TimeOfDay;
        AtualizarAtendimentoDto.PacienteId = response.Data.PacienteId;
        AtualizarAtendimentoDto.Descricao = response.Data.Descricao;
        AtualizarAtendimentoDto.DataHora = _date.Value.Date + _time.GetValueOrDefault();
    }
    
    private async Task Atualizar(AtualizarAtendimentoDto atendimento){
        await _form.Validate();
        
        if (_date == null){
            Snackbar.Add("Data inválida!", Severity.Error);
            return;
        }

        if (_time == null){
            Snackbar.Add("Hora inválida!", Severity.Error);
            return;
        }
        var validationResult = await _atualizarAtendimentoDtoValidator.ValidateAsync(atendimento);
            
        
        if (_form.IsValid && validationResult.IsValid){
            var response = await AtendimentoService.AtualizarAtendimentoAsync(Id, atendimento);
            if (response.IsSuccess){
                Snackbar.Add("Atendimento atualizado com sucesso!", Severity.Success);
                NavigationManager.NavigateTo($"/listar-atendimentos");
            }
            else{
                Snackbar.Add($"Erro ao atualizar o atendimento: {response.Message}", Severity.Error);
            }
        }
        else
        {
            var erros = _form.Errors.Concat(validationResult.Errors.Select(e => e.ErrorMessage));
            foreach (var error in erros.Distinct()){
                Snackbar.Add(error, Severity.Error);
            }
        }
        
    }
}