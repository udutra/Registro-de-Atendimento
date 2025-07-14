using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Interfaces;
using RegistroDeAtendimento.Shared.Application.Validators;

namespace RegistroDeAtendimento.Web.Pages;

public partial class RegistrarAtendimento : ComponentBase{
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private IAtendimentoService AtendimentoService { get; set; } = null!; 
    private MudForm _form;

    private CriarAtendimentoDto Atendimento { get; set; } = new();
    [Inject] CriarAtendimentoDtoValidator _criarAtendimentoDtoValidator { get; set; }

    private DateTime? _date{ get; set; }
    private TimeSpan? _time = new TimeSpan(00, 00, 00);
    
    private async Task Submit(){
        await _form.Validate();

        if (_date != null){
            
            
            Atendimento.DataHora = _date.Value.Date + _time.GetValueOrDefault();
            var validationResult = await _criarAtendimentoDtoValidator.ValidateAsync(Atendimento);
            if(_form.IsValid && validationResult.IsValid){
                
                var response = await AtendimentoService.CriarAtendimentoAsync(Atendimento);

                if (response.IsSuccess){
                    Snackbar.Add("Atendimento registrado com sucesso!", Severity.Success);
                }
                else{
                    Snackbar.Add($"Erro ao acionar o Atendimento: {response.Message}", Severity.Error);
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
        else{
            Snackbar.Add("Data inválida!", Severity.Error);
        }
    }
}