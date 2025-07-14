using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using RegistroDeAtendimento.Core.Domain.Entities;
using RegistroDeAtendimento.Shared.Application.Dtos;
using RegistroDeAtendimento.Shared.Application.Dtos.Responses;
using RegistroDeAtendimento.Shared.Application.Interfaces;
namespace RegistroDeAtendimento.Web.Services;

public class PacienteService(IHttpClientFactory httpClientFactory) : IPacienteService{
    private readonly HttpClient _httpClient = httpClientFactory.CreateClient(Configuration.HttpClientName);
    
    private readonly JsonSerializerOptions _options = new(){
        PropertyNameCaseInsensitive = true,
        Converters = { new JsonStringEnumConverter() }
    };
    
    public Task<PagedResponse<List<Paciente>>> ListarPacientesAsync(ListarPacientesDto dto){
        throw new NotImplementedException();
    }
    public Task<Response<Paciente?>> ObterPacientePorIdAsync(Guid id){
        throw new NotImplementedException();
    }
    public async Task<Response<Paciente?>> CriarPacienteAsync(CriarPacienteDto dto){
        var response = await _httpClient.PostAsJsonAsync("api/paciente", dto);
        var json = await response.Content.ReadAsStringAsync();
        var responseContent = JsonSerializer.Deserialize<Response<Paciente?>>(json, _options);
        
        if (responseContent != null){
            responseContent.Code = (int)response.StatusCode;
        }

        return responseContent ?? new Response<Paciente?>(null, 500, "Erro interno no servidor");
    }
    public Task<Response<Paciente?>> AtualizarPacienteAsync(Guid id, AtualizarPacienteDto dto){
        throw new NotImplementedException();
    }
    public Task<Response<Paciente?>> InativarPacienteAsync(Guid id){
        throw new NotImplementedException();
    }
    public Task<Response<Paciente?>> AtivarPacienteAsync(Guid id){
        throw new NotImplementedException();
    }
}