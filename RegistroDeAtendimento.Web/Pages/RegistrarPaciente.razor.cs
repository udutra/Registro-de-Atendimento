using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Interfaces;
using RegistroDeAtendimento.Shared.Application.Validators;

namespace RegistroDeAtendimento.Web.Pages;

public partial class RegistrarPaciente : ComponentBase{
    [Inject] ISnackbar Snackbar { get; set; }
    [Inject] private NavigationManager NavigationManager{ get; set; } = null!;
    [Inject] private IPacienteService PacienteService { get; set; } = null!; 
    private MudForm _form;

    private CriarPacienteDto Paciente { get; set; } = new();
    [Inject] CriarPacienteDtoValidator _criarPacienteDtoValidator { get; set; }

    private DateTime? _date{ get; set; }

    private async Task Submit(){
        await _form.Validate();

        if (_date != null){
            Paciente.DataNascimento = DateOnly.FromDateTime(_date.Value);
            var validationResult = await _criarPacienteDtoValidator.ValidateAsync(Paciente);
            if(_form.IsValid && validationResult.IsValid){
                
                var response = await PacienteService.CriarPacienteAsync(Paciente);

                if (response.IsSuccess){
                    Snackbar.Add("Paciente registrado com sucesso!", Severity.Success);
                    NavigationManager.NavigateTo($"/listar-pacientes");
                }
                else{
                    Snackbar.Add($"Erro ao acionar o paciente: {response.Message}", Severity.Error);
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