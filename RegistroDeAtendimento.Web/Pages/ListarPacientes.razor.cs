using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;

namespace RegistroDeAtendimento.Web.Pages;

public partial class ListarPacientes : ComponentBase{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IPacienteService PacienteService { get; set; } = null!;
    private ListarPacientesDto ListarPacientesDto { get; set; } = new();
    private List<PacienteResponseDto>? ListPacientes{ get; set; } =[];
    private PacienteResponseDto Paciente { get; set; } = new();
    private MudForm _form;
    private bool IsBusy { get; set; }

    protected override Task OnInitializedAsync(){
        ListarPacientesDto.Sort = ConfigurationResponse.DefaultDirection;
        ListarPacientesDto.OrderBy = ConfigurationResponse.DefaultOrderBy;
        _ = BuscarPacientes();
        return base.OnInitializedAsync();
    }

    private async Task BuscarPacientes(){
        IsBusy = true;
        
        var response = await PacienteService.ListarPacientesAsync(ListarPacientesDto);

        if (response.IsSuccess){
            //Snackbar.Add($"Busca realizada com sucesso!", Severity.Success);
            ListPacientes = response.Data;
            IsBusy = false;
            StateHasChanged();
        }
        else{
            Snackbar.Add($"Erro ao buscar os pacientes: {response.Message}", Severity.Error);
        }
        
    }

    private async Task Inativar(Guid id){
        var response = await PacienteService.InativarPacienteAsync(id);
        if (response.IsSuccess){
            Snackbar.Add("Paciente Inativado com sucesso!", Severity.Success);
            await BuscarPacientes();
        }
        else{
            Snackbar.Add($"Erro ao Inativar o paciente: {response.Message}", Severity.Error);
        }
    }
    
    private async Task Ativar(Guid id){
        var response = await PacienteService.AtivarPacienteAsync(id);
        if (response.IsSuccess){
            Snackbar.Add("Paciente Ativado com sucesso!", Severity.Success);
            await BuscarPacientes();
        }
        else{
            Snackbar.Add($"Erro ao Ativar o paciente: {response.Message}", Severity.Error);
        }
    }

    private void Editar(PacienteResponseDto p){
        NavigationManager.NavigateTo($"/editar-paciente/{p.Id}");
    }

}