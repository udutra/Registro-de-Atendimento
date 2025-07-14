using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;

namespace RegistroDeAtendimento.Web.Pages;

public partial class ListarAtendimento : ComponentBase{
    [Inject] private NavigationManager NavigationManager { get; set; } = null!;
    [Inject] private ISnackbar Snackbar { get; set; }
    [Inject] private IAtendimentoService AtendimentoService { get; set; } = null!;
    private ListarAtendimentosDto ListarAtendimentosDto { get; set; } = new();
    private List<AtendimentoResponseDto>? ListAtendimentos{ get; set; } =[];
    private AtendimentoResponseDto Atendimento { get; set; } = new();
    private MudForm _form;
    private DateTime? _dataInicial{ get; set; }
    private DateTime? _dataFinal{ get; set; }
    private bool IsBusy { get; set; }

    protected override Task OnInitializedAsync(){
        ListarAtendimentosDto.Sort = ConfigurationResponse.DefaultDirection;
        ListarAtendimentosDto.OrderBy = (OrderByAtendimentoEnum?)ConfigurationResponse.DefaultOrderBy;
        _ = BuscarAtendimentos();
        return base.OnInitializedAsync();
    }

    private async Task BuscarAtendimentos(){
        IsBusy = true;
        ListarAtendimentosDto.DataInicio = _dataInicial ?? null;
        ListarAtendimentosDto.DataFim = _dataFinal ?? null;
        var response = await AtendimentoService.ListarAtendimentosAsync(ListarAtendimentosDto);

        if (response.IsSuccess){
            ListAtendimentos = response.Data;
            IsBusy = false;
            StateHasChanged();
        }
        else{
            Snackbar.Add($"Erro ao buscar os atendimentos: {response.Message}", Severity.Error);
        }
        
    }

    private async Task Inativar(Guid id){
        var response = await AtendimentoService.InativarAtendimentoAsync(id);
        if (response.IsSuccess){
            Snackbar.Add("Atendimento Inativado com sucesso!", Severity.Success);
            await BuscarAtendimentos();
        }
        else{
            Snackbar.Add($"Erro ao Inativar o atendimento: {response.Message}", Severity.Error);
        }
    }
    
    private async Task Ativar(Guid id){
        var response = await AtendimentoService.AtivarAtendimentoAsync(id);
        if (response.IsSuccess){
            Snackbar.Add("Atendimento Ativado com sucesso!", Severity.Success);
            await BuscarAtendimentos();
        }
        else{
            Snackbar.Add($"Erro ao Ativar o atendimento: {response.Message}", Severity.Error);
        }
    }

    private void Editar(AtendimentoResponseDto a){
        NavigationManager.NavigateTo($"/editar-atendimento/{a.Id}");
    }
}