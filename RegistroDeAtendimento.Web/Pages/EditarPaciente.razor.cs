using Microsoft.AspNetCore.Components;
using MudBlazor;
using RegistroDeAtendimento.Core.Domain.Enums;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;
using RegistroDeAtendimento.Shared.Application.Validators;

namespace RegistroDeAtendimento.Web.Pages;

public partial class EditarPaciente : ComponentBase{
    [Parameter] public Guid Id{ get; set; }
    public string NomePaciente{ get; set; }
    private AtualizarPacienteDto AtualizarPacienteDto{ get; set; } = new();
    [Inject] private NavigationManager NavigationManager{ get; set; } = null!;
    [Inject] private ISnackbar Snackbar{ get; set; }
    private MudForm _form;
    [Inject] AtualizarPacienteDtoValidator _atualizarPacienteDtoValidator{ get; set; }
    [Inject] private IPacienteService ProdutoService{ get; set; } = null!;
    private SexoEnum SexoSelecionado{ get; set; }
    private DateTime? _date{ get; set; }




    protected override async Task<Task> OnInitializedAsync(){
        var response = await ProdutoService.ObterPacientePorIdAsync(Id);
        AtualizaPaciente(response);
        return base.OnInitializedAsync();
        
    }

    private void AtualizaPaciente(Response<PacienteResponseDto?> response){
        if (response.Data == null) return;
        AtualizarPacienteDto.Nome = response.Data.Nome;
        NomePaciente = response.Data.Nome;
        AtualizarPacienteDto.Cpf = response.Data.Cpf;

        AtualizarPacienteDto.DataNascimento = response.Data.DataNascimento;
        _date = response.Data.DataNascimento.ToDateTime(TimeOnly.MinValue);
        SexoSelecionado = response.Data.Sexo;
        AtualizarPacienteDto.Sexo = SexoSelecionado;
        AtualizarPacienteDto.Cep = response.Data.Cep;
        AtualizarPacienteDto.Cidade = response.Data.Cidade;
        AtualizarPacienteDto.Bairro = response.Data.Bairro;
        AtualizarPacienteDto.Logradouro = response.Data.Logradouro;
        AtualizarPacienteDto.Complemento = response.Data.Complemento;
        AtualizarPacienteDto.Status = response.Data.Status;
    }

    private async Task Atualizar(AtualizarPacienteDto paciente){
        await _form.Validate();
        if (_date != null){
            paciente.DataNascimento = DateOnly.FromDateTime(_date.Value);
            paciente.Sexo = SexoSelecionado;
            
            var validationResult = await _atualizarPacienteDtoValidator.ValidateAsync(paciente);
            if (_form.IsValid && validationResult.IsValid){
                var response = await ProdutoService.AtualizarPacienteAsync(Id, paciente);
                if (response.IsSuccess){
                    Snackbar.Add("Paciente atualizado com sucesso!", Severity.Success);
                    NavigationManager.NavigateTo($"/listar-pacientes");
                }
                else{
                    Snackbar.Add($"Erro ao atualizar o paciente: {response.Message}", Severity.Error);
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